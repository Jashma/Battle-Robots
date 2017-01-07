using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum SM_BotState
{
    None,
    Destroy,
    PlayerControl,
    Hide,
    Deploed,
    PlayerFollow,
}

public enum SM_BotAction
{
    None,
    Repear,
    Capture,
    ChangeModul,
    Wait,
    Move,
}

public class BotController : MonoBehaviour
{
    public SM_BotState botState;
    public SM_BotAction botAction;
    public ModulBasys[] modulController;

    public float health = 10;
    public Team team;
    public float botHeight = 3;
    [HideInInspector]public float speedRotate;
    [HideInInspector]public float timeRadarFound;
    [HideInInspector]public float timeVisualFound;
    [HideInInspector]public float alarm;
    [HideInInspector]public Transform thisTransform;
    [HideInInspector]public Transform pointSniper;
    [HideInInspector]public GameObject enemyObj;
    private Animator animator;
    private CharacterController characterController;
    [SerializeField] float groundCheckDistance = 0.1f;
    private bool isGrounded;
    public SupportBotController supportBotController;
    private List<BotController> myBotControllerList = new List<BotController>();
    public string debugMessage;
    private MessageOnHeadBot messageOnHead;
     
    //Debug
    int arrayIndex;
    string startName;
    private Vector3 spawnPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponentInChildren<CharacterController>();
        messageOnHead = GetComponentInChildren<MessageOnHeadBot>();
        thisTransform = transform;
        ChangeState(4);//Deploed
        //Debug
        startName = name;
        spawnPosition = transform.position;
    }

    void Initialise()
    {
        if (team == Team.Friend)
        {
            GameController.arrayBotControllerFriendTeam.Add(this);
            myBotControllerList = GameController.arrayBotControllerFriendTeam;
        }

        if (team == Team.Enemy)
        {
            GameController.arrayBotControllerEnemyTeam.Add(this);
            myBotControllerList = GameController.arrayBotControllerEnemyTeam;
        }

        ChangeState();
    }

    private void CheckMessageOnHeadState()
    {
        if (botState == SM_BotState.PlayerControl || botState == SM_BotState.PlayerFollow)
        {
            messageOnHead.showMessageOnHead = false;
        }
        else
        {
            messageOnHead.showMessageOnHead = true;
        }
    }

    private void Update()
    {
        CheckGroundStatus();
        ClearAlarm();//Уменьшаем значение тревоги и обнуляем ссылку на текущего врага
        SetBodyTarget();//Назначает точку цели для башни
        Move();//Передвижение
        ProcessAction();//Выполняем действия
        CheckMessageOnHeadState();

        //Debug
        if (arrayIndex != indexBotControllerList(myBotControllerList))
        {
            arrayIndex = indexBotControllerList(myBotControllerList); 
        }

        if (GetComponentInChildren<MessageOnHead>() != null)
        {
            GetComponentInChildren<MessageOnHead>().value = health;
        }

        //End debug
        if (botState == SM_BotState.Deploed && isGrounded == true)
        {
            ActivateModul();
            Initialise();
        }

        name = startName + " " + arrayIndex;
        debugMessage = botState.ToString();
    }

    public void ChangeState(int nextState = 0)
    {
        botState = (SM_BotState)nextState;

        if (botState == SM_BotState.Destroy)
        {
            Destroy();
        }
    }

    public void ChangeAction(int nextState = 0)
    {
        botAction = (SM_BotAction)nextState;
    }

    public void ActivateModul()
    {
        modulController = GetComponentsInChildren<ModulBasys>();

        foreach (ModulBasys modul in modulController)
        {
            modul.startReactor(modulController);
            modul.modulStatus = ModulStatus.On;
        }

        //ReCalculate();
    }

    void Move()
    {
        Vector3 move = Vector3.zero;

        if (botState == SM_BotState.PlayerControl)
        {
            move = PlayerController.moveDirection;
        }

        if (move != Vector3.zero)
        {
            botAction = SM_BotAction.Move;
        }

        if (move.x != 0)
        {
            Quaternion rotation = Quaternion.LookRotation(thisTransform.TransformDirection(new Vector3(move.x, 0, 0)));
            thisTransform.rotation = Quaternion.RotateTowards(thisTransform.rotation, rotation, speedRotate * Time.deltaTime);

            if (move.z == 0)
            {
                move.z = 1;
            }
        }

        UpdateAnimator(move);
    }

    void UpdateAnimator(Vector3 move)
    {
        animator.SetFloat("Forward", move.z, 0.1f, Time.deltaTime);
        animator.SetFloat("Side", move.x, 0.1f, Time.deltaTime);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        Debug.DrawLine(thisTransform.position + (Vector3.up * 0.1f), thisTransform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance), Color.red);
#endif

        if (Physics.Raycast(thisTransform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            characterController.Move(Physics.gravity * Time.deltaTime * 3);
        }
    }

    void SetBodyTarget()
    {
        Vector3 targetPoint;

        if (botState == SM_BotState.PlayerControl)
        {
            targetPoint = PlayerController.hitForwardCollision.point;

            if (botAction == SM_BotAction.Repear)
            {
                targetPoint = thisTransform.TransformPoint(Vector3.forward * 500);
            }

            if (botAction == SM_BotAction.ChangeModul)
            {
                targetPoint = thisTransform.TransformPoint(Vector3.forward * 500);
            }

            if (botAction == SM_BotAction.Capture)
            {
                targetPoint = thisTransform.TransformPoint(Vector3.forward * 500);
            }
        }
        else
        {
            targetPoint = thisTransform.TransformPoint(Vector3.forward * 500);
        }

        foreach (ModulBasys modul in modulController)
        {
            modul.SetLookTarget(targetPoint);
        }
    }

    void ClearAlarm()//Уменьшаем значение тревоги и обнуляем ссылку на текущего врага
    {
        if (alarm > 0)
        {
            alarm--;
        }
        else
        {
            enemyObj = null;
        }
    }

    void ProcessAction()
    {
        if (supportBotController != null)
        {
            supportBotController.ProcessAction(this);
        }
        else
        {
            botAction = SM_BotAction.Wait;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SupportBotController>() != null)
        {
            if (other.GetComponent<SupportBotController>().team == team)
            {
                supportBotController = other.GetComponent<SupportBotController>();
            }
        }

        if (other.GetComponent<Rigidbody>() != null)
        {
            Debug.Log("<Rigidbody>() != null");
            other.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SupportBotController>() != null && supportBotController == other.GetComponent<SupportBotController>())
        {
            supportBotController = null;
        }
    }

    void Destroy()
    {
        animator.SetInteger("Dead", UnityEngine.Random.Range(1, 4));

        if (PlayerController.playerState == PlayerState.ControlBot && PlayerController.botController == this)
        {
            PlayerController.Instance.ChangePlayerState(2);//Follow
        }
    }

    void HideBot()
    {
        animator.SetInteger("Dead", 0);
        ClearBotList();
        
        if (PlayerController.playerState == PlayerState.FollowBot && PlayerController.botController == this)
        {
            PlayerController.Instance.ChangePlayerState(2);//Follow
        }

        ChangeState(3);//Hide
        Spawn();
    }

    void ClearBotList()
    {
        for (int i = 0; i < myBotControllerList.Count; i++)
        {
            if (myBotControllerList[i] == this)
            {
                myBotControllerList.RemoveAt(i);
            }
        }
    }

    int indexBotControllerList(List<BotController> botControllerList)
    {
        int index = 0;

        for (int i = 0; i< botControllerList.Count; i++)
        {
            if (botControllerList[i] == this)
            {
                index = i;
            }
        }

        return index;
    }

    //Debug
    void Spawn()
    {
        transform.position = spawnPosition;
        ChangeState(4);//Deploed
    }

    public ModulBasys GetModulByStatus(ModulType type)
    {
        foreach (ModulBasys modul in modulController)
        {
            if (modul.modulType == type)
            {
                return modul;
            }
        }

        return null;
    }

    void ReCalculate()
    {
        //float sum = GetModulEnergySum() - (modulController[0].energyReloadQuoue);
        //float sumToDistribut = 100 - (modulController[0].energyReloadQuoue);
        //float mLocalHash = sumToDistribut / sum;

        float sum = GetModulEnergySum();
        //Debug.Log(GetModulEnergySum());
        float mLocalHash = 100 / GetModulEnergySum();

        foreach (ModulBasys modul in modulController)
        {
            {
                modul.energyReloadQuoue *= mLocalHash;
            }
        }
    }

    float GetModulEnergySum()
    {
        float sum = 0;

        for (int i = 0; i < modulController.Length; i++)
        {
            if (modulController[i].modulStatus == ModulStatus.On)
            {
                sum += modulController[i].energyReloadQuoue;
            }
        }

        return sum;
    }
}
