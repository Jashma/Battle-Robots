using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//Debug
public enum SM_BotState
{
    Disable,
    Dead,
    PlayerControl,
    OffLine,
    AiControl,
    Deploed,
}

public class BotController : MonoBehaviour 
{
    public SM_BotState botState;
    public SM_BotCharacter SM_botCharacter;
    public CharacterController characterController;
    public BodyRotation bodyRotation;//Скрипт управляющий башней
    public Animator animator;
    public Animator stateMashine;
    public NavMeshAgent navMeshAgent;
    public NavMeshObstacle navMeshObstacle;
    public CaptureState captureState;//Скрипт управляущий захватом базы или завода
    public RepearState repearState;//Скрипт отвечающий за ремонт, находится на обьекте место ремонта
    public CheckVisibleMesh checkVisibleMesh;
    public float health;//Здоровье бота
    public float speedWalk;
    public float speedRotate;

    public Team team;
    public string aiMessage;//Сообщенние выводится на UI панеле бота о состоянии АИ
    public Vector3 moveTargetPosition;//Координаты конечной точки пути для аи
    public Vector3 rotateDirection;//Направление, куда нужно повернутся боту
    public Vector3 moveDirection;//Направление, куда нужно идти боту
    public GameObject moveTargetObj;//Текущий обьект цель движения
    public GameObject enemyObj;//Текущий враг - цель
    public GameObject repearPlaceObj;//Обьект место ремонта. Определяется при прохождении мимо такого обьекта
    public NavMeshPath path;

    public int neadRepear;//флаг, бот начал ремонтироватся
    public int neadReload;//флаг, бот начал перезаряжатся
    public bool startChangeModule;//флаг, бот начал перезаряжатся
    public bool startCapture;//флаг, бот начал захват

    public int alarm;//текущее время, сколько бот находится в состоянии боя.
    public float timeRadarFound;//текущее время, сколько прошло со времени обнаружения радаром противника.
    public float timeVisualFound;//текущее время, сколько прошло со времени обнаружения визуально противником.

    //public LayerMask enemyFindlayerMask;//Маска слоев для поиска врага
    private GameObject ui_IndicatorObj;
    private GameObject ui_RadarIconBot;

    float moveFront;
    float moveSide;

    //Текущие модули
    public List<ModulScr> modulScr;
    public List<GunController> gunController;
    public BodyController bodyController;
    public RadarController radarController;

    void OnEnable()
    {
        Initialise();
    }

    public void Initialise()
    {
        speedWalk = 1.7f;
        speedRotate = 120;
        name = "Bot " + team;

        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        if (navMeshObstacle == null)
        {
            navMeshObstacle = GetComponent<NavMeshObstacle>();
        }

        if (checkVisibleMesh == null)
        {
            checkVisibleMesh = GetComponentInChildren<CheckVisibleMesh>();
        }

        if (characterController == null)
        {
            characterController = GetComponentInChildren<CharacterController>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (stateMashine == null)
        {
            stateMashine = transform.FindChild("Ai").GetComponent<Animator>();
            stateMashine.gameObject.SetActive(false);
        }

        navMeshAgent.speed = speedWalk;
        navMeshAgent.acceleration = 10;
        navMeshAgent.angularSpeed = speedRotate;
        health = 100;
        ActivateModul();


        if (team == Team.Red)
        {
            LevelController.arrayBotControllerRed.Add(this);
        }

        if (team == Team.Blue)
        {
            LevelController.arrayBotControllerBlue.Add(this);
        }
    }

    void FixedUpdate()
    {
        if (botState == SM_BotState.PlayerControl)
        {
            MoveBotPlayer(PlayerController.moveDirection);
        }

        if (botState == SM_BotState.AiControl)
        {
            BotMoveAi(navMeshAgent.desiredVelocity);
        }

        if (transform.position.y - LevelController.terrain.SampleHeight(transform.position) > 1)
        {
            characterController.Move(Physics.gravity * Time.deltaTime);
            animator.SetBool("Action", true);
        }
        else
        {
            if (botState == SM_BotState.Deploed)
            {
                navMeshAgent.enabled = true;
                stateMashine.gameObject.SetActive(true);
                botState = SM_BotState.AiControl;

                //Debug
                if (ui_IndicatorObj == null)
                {
                    if (GameObject.Find("UI") != null)
                    {
                        EnableIndicator();
                    }
                }
                //Debug end
            }

            if (animator.GetBool("Action") == true)
            {
                animator.SetBool("Action", false);
            }
        }
    }

    void MoveBotPlayer(Vector3 velocity)
    {
        navMeshAgent.velocity = animator.deltaPosition;
        moveFront = velocity.z;

        if (velocity.x != 0)
        {
            Quaternion rotation = Quaternion.LookRotation(transform.TransformDirection(new Vector3(velocity.x, 0, 0)));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, speedRotate * Time.deltaTime);

            if (velocity.z == 0)
            {
                moveFront = 1;
            }
        }

        animator.SetFloat("Forward", moveFront, 0.1f, Time.deltaTime);
        animator.SetFloat("Side", velocity.y, 0.1f, Time.deltaTime);

        Vector3 startDraw = transform.position + Vector3.up;
        Vector3 endDraw = transform.position + velocity;
        Debug.DrawRay(startDraw, endDraw - startDraw + Vector3.up, Color.black);

    }

    void BotMoveAi(Vector3 velocity)
    {
        if (animator.applyRootMotion == true)
        {
            if (alarm > 8 && enemyObj != null)
            {
                if (velocity != Vector3.zero)
                {
                    navMeshAgent.angularSpeed = 0;
                    Quaternion rotation = Quaternion.LookRotation(enemyObj.transform.position - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, speedRotate * Time.deltaTime);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }
                else
                {
                    navMeshAgent.angularSpeed = 0;
                }
            }
            else
            {
                navMeshAgent.angularSpeed = speedRotate;
            }

            navMeshAgent.velocity = Vector3.zero;
            moveFront = Vector3.Dot(transform.forward, velocity.normalized);
            moveSide = Vector3.Dot(transform.right, velocity.normalized);
           
        }

        animator.SetFloat("Forward", moveFront, 0.1f, Time.deltaTime);
        animator.SetFloat("Side", moveSide, 0.1f, Time.deltaTime);
        
        Vector3 startDraw = transform.position + Vector3.up;
        Vector3 endDraw = transform.position + velocity;
        Debug.DrawRay(startDraw, endDraw - startDraw + Vector3.up, Color.blue);
    }

    public void AnalisHealth()
    {
        neadRepear = 100 - Mathf.RoundToInt(health);
    }

    public void AnalisReload()
    {
        /*
        if (gunStateLeftModul != null)
        {
            allMaxAmmo = gunStateLeftModul.maxAmmo;
            allCurrentAmmo = gunStateLeftModul.ammo;
        }

        if (gunStateRightModul != null)
        {
            allMaxAmmo += gunStateRightModul.maxAmmo;
            allCurrentAmmo += gunStateRightModul.ammo;
        }

        neadReload = allMaxAmmo - allCurrentAmmo;

        allMaxAmmo = 0;
        allCurrentAmmo = 0;
         */ 
    }

    public void EnableWeaponModul()
    {
        /*
        if (gunStateRightModul != null && gunStateRightModul.weaponIsOn == false)
        {
            //Debug.Log("EnableRightWeaponModul");
            gunStateRightModul.weaponIsOn = true;
        }

        if (gunStateLeftModul != null && gunStateLeftModul.weaponIsOn == false)
        {
            //Debug.Log("EnableLeftWeaponModul");
            gunStateLeftModul.weaponIsOn = true;
        }
         */ 
    }

    public void StartCaptureAnimation()
    {
        if (captureState != null)
        {
            if ((captureState.transform.position - transform.position).sqrMagnitude <= navMeshAgent.stoppingDistance)
            {
                animator.SetBool("Capture", true);
                ProcessCapture(0.1f);
                bodyRotation.targetTransform = null;
            }
            else
            {
                aiMessage = "Слишком далеко";
                animator.SetBool("Capture", false);
            }
        }
    }

    void ProcessCapture(float value = 0.1f)
    {
        if (captureState != null)
        {
            if (captureState.team != team)
            {
                bodyRotation.targetTransform = null;
                captureState.capture += value;

                if (captureState.capture >= 100)
                {
                    captureState.team = team;
                    captureState.capture = 0;
                    aiMessage = "Захвачено";
                    animator.SetBool("Capture", false);
                }
            }
            else
            {
                aiMessage = "Захвачено";
                animator.SetBool("Capture", false);
            }
        }

    }

    public void StartReloadAnimation()
    {
        if (repearState != null)
        {
            if ((repearState.transform.position - transform.position).sqrMagnitude <= navMeshAgent.stoppingDistance)
            {
                animator.SetBool("Reload", true);
                bodyRotation.targetTransform = null;
            }
            else
            {
                aiMessage = "Слишком далеко";
                animator.SetBool("Reload", false);
            }
        }
    }

    public void StartRepearAnimation()
    {
        if (repearState != null)
        {
            if ((repearState.transform.position - transform.position).sqrMagnitude <= navMeshAgent.stoppingDistance)
            {
                animator.SetBool("Repear", true);

                //ProcessRepear(0.1f);
                //CheckReloadWepon(0.1f);

                bodyRotation.targetTransform = null;
                //ProcessReloadWeapon(gunReloadLeftModul, gunStateLeftModul, 0.1f);
                //ProcessReloadWeapon(gunReloadRightModul, gunStateRightModul, 0.1f);
            }
            else
            {
                aiMessage = "Слишком далеко";
                animator.SetBool("Repear", false);
            }
        }
    }

    public void ChangeState(string state = "")
    {
        if (state != "")
        {
            botState = (SM_BotState)Enum.Parse(typeof(SM_BotState), state);

            if (state == "OffLine")
            {
                animator.SetBool("StartAction", true);
                botState = SM_BotState.OffLine;
            }

            if (state == "PlayerControl")
            {
                animator.SetBool("StartAction", false);
                botState = SM_BotState.PlayerControl;
            }
        }
    }

    public void DestroyBot()//Вызывается из скрипта пули при попадании по боту
    {
        enemyObj = null;
        startCapture = false;
        animator.SetBool("Repear", false);
        animator.SetBool("Reload", false);
        animator.SetFloat("Forward", 0);
        animator.SetFloat("Side", 0);
        animator.SetInteger("Dead", UnityEngine.Random.Range(1, 4));  //проигрываем анимацию смерти
        moveTargetObj = null;
        stateMashine.gameObject.SetActive(false);
        moveTargetPosition = Vector3.zero;
        botState = SM_BotState.Dead;
        ui_IndicatorObj.SetActive(false);

        if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled) 
        { 
            navMeshAgent.ResetPath();
            navMeshAgent.enabled = false;
            navMeshObstacle.enabled = true;
        }

        if (captureState != null && captureState.botID == GetInstanceID()) 
        {
            if (startCapture == true && captureState.capture > 0)
            { 
                captureState.capture = 0;
                startCapture = false;
            }

            captureState.botID = 0;
            captureState = null;
        }

        if (repearState != null && repearState.botID == GetInstanceID())
        {
            repearState.botID = 0;
            repearState = null;
        }
        
        if (PlayerController.playerState == PlayerState.BotControl && PlayerController.botController == this)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().ChangePlayerState("Follow");
            GameObject.Find("Player").GetComponent<PlayerMouseLook>().sniperLook = false;
        }

        StartCoroutine(DisableSelf(100 * Time.deltaTime));
    }

    public void Destroy()
    { 
    }

    void ActivateModul()
    {
        foreach (ModulScr modulState in GetComponentsInChildren<ModulScr>())
        {
            modulState.botController = this;
            modulState.EnableModule();
        }
    }

    void EnableIndicator()
    {
        if (ui_IndicatorObj == null)
        {
            ui_IndicatorObj = Instantiate(Resources.Load("Prefabs/User Interface/BotIndicator")) as GameObject;
            ui_IndicatorObj.transform.SetParent(GameObject.Find("UI").transform);
            ui_IndicatorObj.GetComponent<UI_BotIndicator>().botController = this;
        }
        else
        {
            ui_IndicatorObj.SetActive(true);
        }
    }

    private IEnumerator DisableSelf(float time = 10)
    {
        yield return new WaitForSeconds(time);

        if (team == Team.Red)
        {
            for (int i = 0; i < LevelController.arrayBotControllerRed.Count; i++)
            {
                if (LevelController.arrayBotControllerRed[i] == this)
                {
                    LevelController.arrayBotControllerRed.RemoveAt(i);
                }
            }
        }

        if (team == Team.Blue)
        {
            for (int i = 0; i < LevelController.arrayBotControllerBlue.Count; i++)
            {
                if (LevelController.arrayBotControllerBlue[i] == this)
                {
                    LevelController.arrayBotControllerBlue.RemoveAt(i);
                }
            }
        }

        if (PlayerController.botController == this)
        {
            PlayerController.botController = null;
        }

        transform.position = Vector3.down * 10;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        botState = SM_BotState.Disable;
        name = name + team.ToString() + botState.ToString();
        animator.SetInteger("Dead", 0);
        StartSpawn();
    }

    void StartSpawn()
    {
        //if (GameObject.Find("DeploedShip") == null)
        {
            if (team == Team.Blue)
            {
                if (GameObject.Find("BaseBlue").GetComponent<BaseState>().enableShip == false)
                {
                    GameObject.Find("BaseBlue").GetComponent<BaseState>().StartSpawn(this.gameObject);
                }
            }

            if (team == Team.Red)
            {
                if (GameObject.Find("BaseRed").GetComponent<BaseState>().enableShip == false)
                {
                    GameObject.Find("BaseRed").GetComponent<BaseState>().StartSpawn(this.gameObject);
                }
            }
        }
    }

}
