using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum PlayerState
{
    None,
    Spectator,
    FollowBot,
    ControlBot,
    Deploed,
    ChangeModul,
}

public class PlayerController : MonoBehaviour 
{
    public static BotController botController;
    public static PlayerState playerState;
    public static Transform playerTransform;
    public static Team playerTeam;
    public static Team enemyTeam;
    public static bool inGame = false;
    public static float mouseSence = 5;//Значение в пределах 0 - 10 
    public static List<BotController> arrayBotController;
    public static GameObject playerBase;
    public static Vector3 moveDirection;
    public static RaycastHit hitForwardCollision;
    public float moveSpeed = 10;
    public LayerMask forwardLayerMask;

    private CharacterController characterController;
    private float frontDirection;
    private float sideDirection;
    private float upDirection;
    private float rotateDirection;
    private Ray rayForward;
    
    private bool pushFire1 = false;
    private bool pushFire2 = false;
    private int oldKeyAlfa = 0;
    private int keyAlfa = 0;
    private int indexBot = 0;

    //Debug
    public Vector3 moveDirectionDB;
    public BotController botControllerDB;
    public  PlayerState playerStateDB;
    public bool startGameDB;
    public Team playerTeamDB;
    public List<BotController> arrayBotControllerDB;

    public static PlayerController Instance { get; private set; }

    void Awake()
    {
        playerState = PlayerState.None;
        playerTransform = GetComponent<Transform>();
        Instance = this;
    }

    public void PlayerStart()
    {
        playerTeam = Team.Friend;
        name = "Player";
        characterController = GetComponent<CharacterController>();

        PlayerController.enemyTeam = Team.Enemy;
        arrayBotController = GameController.arrayBotControllerFriendTeam;
        PlayerController.inGame = true;
        ChangePlayerState(1); //Spectator
    }

    void FixedUpdate()
    {
        if (playerState != PlayerState.Deploed)
        {
            InputControl();
            GetForwardCollision();
        }

        if (playerState == PlayerState.Spectator)
        {
            if (UI_Controller.uiState == UI_State.InGame)
            {
                PlayerMove();
            }
        }

        //Debug
        arrayBotControllerDB = arrayBotController;
        botControllerDB = botController;
        moveDirectionDB = moveDirection;
        playerStateDB = playerState;
        playerTeamDB = playerTeam;
    }

    void PlayerMove()
    {
        characterController.Move(transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
    }

    void InputControl()
    {
        moveDirection = Vector3.zero;
        moveDirection += Vector3.forward * Input.GetAxis("Front");
        moveDirection += Vector3.right * Input.GetAxis("Side");
        moveDirection += Vector3.up * Input.GetAxis("Vertical");
        KeyAxisFire1();//Левая кнопка мыши
        KeyAxisFire2();//Правая кнопка мыши
        KeyCodF5();
        keyAlfa = GetKeyAlfa();


        //Вкл\Откл модуля
        if (UI_Controller.uiState == UI_State.InGame)
        {
            if (playerState == PlayerState.ControlBot)
            {
                if (keyAlfa != oldKeyAlfa)
                {
                    oldKeyAlfa = keyAlfa;
                    KeyAxisChangeWeapon(keyAlfa);
                }
            }
        }
    }

    int GetKeyAlfa()
    {
        /*
        for (int i = 0; i < (int)ModulTypeDinamic.ArrayLeight; i++)
        {
            if (Input.GetAxis("ChangeModul" + i.ToString()) != 0)
            {
                return i;
            }
        }
        */
        return 0;
        
    }

    void KeyAxisFire1()
    {
        //Левая кнопка мыши
        if (Input.GetAxis("Fire1") != 0)
        {
            if (pushFire1 == false)
            {
                if (UI_Controller.uiState == UI_State.InGame)
                {
                    //Переключение между роботами
                    if (playerState == PlayerState.FollowBot)
                    {
                        indexBot++;
                        GetNextBot();
                    }

                    //Выстрелы
                    if (playerState == PlayerState.ControlBot)
                    {
                        if (botController != null )
                        {
                            if (UI_Controller.uiInGameMenu == UI_InGameMenu.None)
                            {
                                foreach (ModulBasys modul in botController.modulController)
                                {
                                    if (modul.modulStatus == ModulStatus.On)
                                    {
                                        modul.Shoot(GameController.showHitInformation);
                                    }
                                }
                            }
                        }
                    }
                }

                pushFire1 = true;
            }
        }
        else
        {
            if (pushFire1 == true)
            {
                pushFire1 = false;
            }
        }
    }

    void KeyAxisFire2()
    {
        //Правая кнопка мыши
        if (Input.GetAxis("Fire2") != 0)
        {
            if (pushFire2 == false)
            {
                if (UI_Controller.uiState == UI_State.InGame)
                {
                    if (playerState == PlayerState.ControlBot)
                    {
                        if (botController != null)
                        {
                            if (GetComponent<PlayerMouseLook>().mouseState == MouseState.SniperLook)
                            {
                                GetComponent<PlayerMouseLook>().ChangeLookState(0);
                                transform.parent = null;
                            }
                            else
                            {
                                GetComponent<PlayerMouseLook>().ChangeLookState(1);
                            }
                        }
                    }
                }

                pushFire2 = true;
            }
        }
        else
        {
            if (pushFire2 == true)
            {
                pushFire2 = false;
            }
        }
    }

    void KeyAxisChangeWeapon(int numberWeapon = 0)
    {
        foreach (ModulBasys controller in PlayerController.botController.modulController)
        {
            /*
            if (controller.modulTypeDinamic == (ModulTypeDinamic)numberWeapon)
            {
                if (controller.modulStatus == ModulStatus.On)
                {
                    controller.modulStatus = ModulStatus.Off;
                }
                else
                {
                    controller.modulStatus = ModulStatus.On;
                }
            }
            */
        }
    }

    void KeyCodF5()//SelfDestroy
    {
        if (Input.GetKeyUp(KeyCode.F5))
        {
            if (botController != null)
            {
                botController.ChangeState(1);//"Destroy"
            }
        }
    }

    void GetNextBot()
    {
        int tmpIndex = arrayBotController.Count;

        while (tmpIndex > 0)
        {
            tmpIndex--;

            if (indexBot > arrayBotController.Count - 1)
            {
                indexBot = 0;
            }

            if (playerState == PlayerState.FollowBot)
            {
                botController = arrayBotController[indexBot];
                botController.ChangeState(5);//"PlayerFollow"
                return;
            }

            indexBot++;
        }

        {
            ChangePlayerState(1);//Spectator
        }
    }

    void GetControllBot()
    {
        botController.ChangeState(2);//"PlayerControl"
    }

    void ClearControlBot()
    {
        if (botController != null)
        {
            botController.ChangeState();//"None"
            botController.ChangeAction();//"None"
        }
    }

    public void ChangePlayerState(int nextState = 0)
    {
        playerState = (PlayerState)nextState;

        if (playerState == PlayerState.Spectator)
        {
            ClearControlBot();
            botController = null;
        }

        if (playerState == PlayerState.FollowBot)
        {
            ClearControlBot();
            GetNextBot();
        }

        if (playerState == PlayerState.ControlBot)
        {
            GetControllBot();
        }

        GetComponent<PlayerMouseLook>().ChangeLookState(0);
    }

    void GetForwardCollision()
    {
        rayForward = new Ray(transform.position, transform.TransformDirection(Vector3.forward * 500));

        //rayForward = new Ray(transform.TransformDirection(Vector3.forward * 2), transform.TransformDirection(Vector3.forward * 500));

        Physics.Raycast(rayForward, out hitForwardCollision, 500, forwardLayerMask);
        {
            if (!Physics.Raycast(rayForward, out hitForwardCollision, 500, forwardLayerMask))
            { 
                hitForwardCollision.point = transform.TransformPoint(Vector3.forward * 500);

                //if (playerState == PlayerState.ControlBot || playerState == PlayerState.FollowBot)
                {
                    //UI_ConsolMessageController.Instance.SetNewMessage("");
                }
            }
            else
            {
               // if (playerState == PlayerState.ControlBot || playerState == PlayerState.FollowBot)
                {
                   // UI_ConsolMessageController.Instance.SetNewMessage(hitForwardCollision.collider.name);
                }
            }
        }
    }
}
