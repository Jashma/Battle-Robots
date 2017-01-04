using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum PlayerState
{
    None,
    Spectator,
    Follow,
    BotControl,
    Deploed,
}

public class PlayerController : MonoBehaviour 
{
    public static BotController botController;
    public static PlayerState playerState;
    public static Team playerTeam;
    public static Team enemyTeam;
    public static bool startGame = false;

    public static List<BotController> arrayBotController;
    public static BaseState baseController;

    public float moveSpeed;
    public float acceleration;

    private CharacterController characterController;
    private float frontDirection;
    private float sideDirection;
    private float upDirection;
    private float rotateDirection;
    public static Vector3 moveDirection;

    private Ray rayForward;
    public static RaycastHit hitForwardCollision;
    public LayerMask forwardLayerMask;
    private bool pushFire1 = false;
    private bool pushFire2 = false;
    private int indexBot = 0;

    //Debug
    public Vector3 moveDirectionDB;
    public BotController botControllerDB;
    public  PlayerState playerStateDB;
    public bool startGameDB;
    public List<BotController> arrayBotControllerDB;

    void Awake()
    {
        playerState = PlayerState.None;
    }

    void Start()
    {
        GameObject.Find("UI").GetComponent<UI_Controller>().ChangeState("ChangeTeam");
    }

    public void PlayerStart(string team = "") 
    {
        if (team != "")
        {
            playerTeam = (Team)Enum.Parse(typeof(Team), team);

            name = "Player";
            characterController = GetComponent<CharacterController>();

            if (playerTeam == Team.Red)
            {
                arrayBotController = LevelController.arrayBotControllerRed;
                baseController = GameObject.Find("BaseRed").GetComponent<BaseState>();
            }

            if (PlayerController.playerTeam == Team.Blue)
            {
                arrayBotController = LevelController.arrayBotControllerBlue;
                baseController = GameObject.Find("BaseBlue").GetComponent<BaseState>();
            }

            PlayerController.startGame = true;

            ChangePlayerState("Deploed");
            GameObject.Find("UI").GetComponent<UI_Controller>().ChangeState("DeploedMenu");
            
        }
	}
	
	void FixedUpdate () 
    {
        if (UI_Controller.uiState == UI_State.InGame)
        {
            if (playerState != PlayerState.Deploed)
            {
                InputControl();
                GetForwardCollision();
            }

            
            if (playerState == PlayerState.BotControl)
            {
                if (botController != null && botController.botState != SM_BotState.PlayerControl)
                {
                    botController.botState = SM_BotState.PlayerControl;
                }
            }

            if (playerState == PlayerState.Deploed)
            {
                if (botController != null && botController.botState == SM_BotState.PlayerControl)
                {
                    botController.botState = SM_BotState.OffLine;
                }
            }

            if (playerState == PlayerState.Follow)
            {
                if (botController == null)
                {
                    GetNextBot();
                }
            }
            

            if (playerState == PlayerState.Spectator)
            {
                PlayerMove();
            }
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        //Debug
        arrayBotControllerDB = arrayBotController;
        botControllerDB = botController;
        moveDirectionDB = moveDirection;
        playerStateDB = playerState;
        startGameDB = startGame;
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
                    if (playerState == PlayerState.Follow)
                    {
                        indexBot++;
                        GetNextBot();
                    }

                    //Выстрелы
                    if (playerState == PlayerState.BotControl)
                    {
                        if (botController != null)
                        {
                            foreach (GunController gunController in botController.gunController)
                            {
                                if (gunController.weaponIsOn == true)
                                {
                                    gunController.Shoot();
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
                    if (playerState == PlayerState.BotControl)
                    {
                        if (botController != null)
                        {
                            if (GetComponent<PlayerMouseLook>().sniperLook == true)
                            {
                                GetComponent<PlayerMouseLook>().sniperLook = false;
                                transform.parent = null;
                            }
                            else
                            {
                                GetComponent<PlayerMouseLook>().sniperLook = true;
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


    void KeyCodF5()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (botController != null)
            {
               botController.DestroyBot();
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

            if (playerState == PlayerState.Follow)
            {
                botController = arrayBotController[indexBot];
                return;
            }

            indexBot++;
        }

        {
            ChangePlayerState("Spectator");
        }
    }

    void GetControllBot()
    {
        botController.botState = SM_BotState.PlayerControl;
        botController.enemyObj = null;
        botController.startCapture = false;
        botController.animator.SetBool("Repear", false);
        botController.animator.SetBool("Reload", false);
        botController.moveTargetObj = null;
        botController.stateMashine.gameObject.SetActive(false);
        botController.navMeshAgent.ResetPath();
        botController.navMeshAgent.enabled = false;
        botController.navMeshObstacle.enabled = true;
        botController.moveTargetPosition = Vector3.zero;
    }

    void ClearControlBot(bool clearBotController = false)
    {
        if (botController.botState != SM_BotState.Dead)
        {
            botController.stateMashine.gameObject.SetActive(true);
            botController.navMeshObstacle.enabled = false;
            botController.navMeshAgent.enabled = true;
            botController.botState = SM_BotState.AiControl;
        }
    }

    public void ChangePlayerState(string nextState)
    {

        if (playerState == PlayerState.BotControl)
        {
            ClearControlBot(false);
        }

        if (playerState == PlayerState.Follow)
        {
            if (nextState == "BotControl")
            {
                GetControllBot();
            }
        }

        if (nextState == "Spectator")
        {
            botController = null;
        }

        if (nextState == "Deploed")
        {
            botController = null;
        }

        if (nextState != "")
        {
            playerState = (PlayerState)Enum.Parse(typeof(PlayerState), nextState);
        }
    }

    void GetForwardCollision()
    {
        rayForward = new Ray(transform.position, transform.TransformDirection(Vector3.forward * 500));
        Physics.Raycast(rayForward, out hitForwardCollision, 500, forwardLayerMask);
        {
            if (!Physics.Raycast(rayForward, out hitForwardCollision, 500, forwardLayerMask))
            { 
                hitForwardCollision.point = transform.TransformPoint(Vector3.forward * 500); 
            }
        }
    }

    //private IEnumerator DisableAxis(float time)
    //{
        //yield return new WaitForSeconds(time * Time.deltaTime);
        //isAxisInUse = false;
    //}
}
