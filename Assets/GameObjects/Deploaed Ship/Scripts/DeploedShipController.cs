using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeploedShipController : MonoBehaviour 
{
    public bool autoClearCurrentBot;//Очистка значения количества созданных ботов
    public bool enableDeploed;//Разрешаем десантирование,когда на позиции
    //public int currentBot;//Количество созданных ботов
    public int maxBot;//Максимално разрешенное количество ботов
    public bool autoCreateBot;//Автоматическое создание ботов
    public int[] queueBot;//Очередь на десантирование
    public int nextBotType;//Тип бота в очереди
    public GameObject[] spawnObjPrefab;//Префабы ботов
    public GameObject[] botObj;//Загруженые боты
    public bool inDeploedPosition;//На позиции десантирования

    public Vector3 deploedPosition;//Позиция, где совершать десантирование
    public Vector3 defaultDeploedPosition;//Позиция по умолчанию, где совершать десантирование
    public float speedMove;
    public float speedRotate;
    public float flyZoneRadius;
    public float flyHeight;
    public float timeCheck;
    public float deploedRadius;
    public float deploedHeight;

    public bool startDeploed;
    private Vector3 targetPosition;
    private Quaternion direction;
    private Vector2 shipPosition;
    private Vector2 endPosition;
    private Vector3 startPosition;
    private Vector3 endPisition;
    private float distanceToGround;
    public float distanceToDeploed;
    public Team team;
    public LayerMask layerMask;

    private int tempCountBot = 0; //Количество ботов конкретного типа
    private int indexBot = 0;//шндекс массива spawnObjPrefab

    //Для дебага//
    public string debugText;
    public bool showDebug = true;
    public Text ConsolMessage;
    public GameObject player;
    public Collider[] hitColliders;
    private Vector3 gizmoPosition;
    private float gizmoRadius;
    private Color gizmoColor; 
	void Start() 
    {
        InstanceBot();//Создаем всех ботов в игре
        player = GameObject.Find("Player");
        queueBot = new int[maxBot];
        transform.parent = null;
        startPosition = defaultDeploedPosition;
        deploedPosition = defaultDeploedPosition;
        transform.position = startPosition + Vector3.left * (flyZoneRadius + 50) + Vector3.up * flyHeight;
        StartCoroutine(ClearBot(10));
        
        autoCreateBot = true;
	}
	
    void InstanceBot()
    {
        botObj = new GameObject[maxBot * spawnObjPrefab.Length];//Количество ботов каждого вида равна максимальному количеству ботов в игре
        indexBot = 0;
        tempCountBot = 0;
        //Когда tempCountBot становится равным maxBot то indexBot увеличивается на 1 и создаются следующий тип ботов
        for (int i = 0; i < maxBot * spawnObjPrefab.Length; i++)
        {
            spawnObjPrefab[indexBot].SetActive(false);
            botObj[i] = Instantiate(spawnObjPrefab[indexBot], Vector3.left * i, Quaternion.identity) as GameObject;
            botObj[i].SetActive(false);
            botObj[i].GetComponent<BotController>().team = team;
            botObj[i].GetComponent<BotController>().botState = SM_BotState.OffLine;
            botObj[i].name = botObj[i].name + botObj[i].GetComponent<BotController>().team.ToString() + botObj[i].GetComponent<BotController>().botState.ToString();
            tempCountBot++;

            if (tempCountBot == maxBot)
            {
                tempCountBot = 0;
                indexBot++;
            }
        }
    }

	void LateUpdate() 
    {
        DirectionFly();
        Fly();
        ShowDebugText();
        ClearBot();
	}

    void ShowDebugText()
    {
        if (showDebug == true)
        {
            if (UI_Controller.uiState == UI_State.InGame)
            {
                if (ConsolMessage == null)
                {
                    if (GameObject.Find("DebugMessageText").GetComponent<Text>() != null)
                    {
                        ConsolMessage = GameObject.Find("DebugMessageText").GetComponent<Text>();
                    }
                }
                else
                {
                    ConsolMessage.text = debugText;
                }
            }
        }
    }

    void DirectionFly()
    {
        shipPosition = new Vector2(transform.position.x, transform.position.z);//Текущая позиция
        endPosition = new Vector2(deploedPosition.x, deploedPosition.z);//Позиция цели
        distanceToDeploed = Vector2.Distance(shipPosition, endPosition);

        if (distanceToDeploed > flyZoneRadius)
        {
            targetPosition = deploedPosition + Vector3.up * deploedHeight;
            debugText = "Цель десантирования";

        }
        else
        {
            if (distanceToDeploed < deploedRadius)
            {
                inDeploedPosition = true;
                debugText = "На позиции десантирования";

                if (enableDeploed == true && startDeploed == false)
                {
                    if (PlayerController.startGame == true)
                    {
                        debugText = "Начинаю десантирование";
                        startDeploed = true;
                        // -= Создание ботов из очереди =- //
                        if (spawnObjPrefab.Length > 0)
                        {
                            StartCoroutine(DeploedBot(timeCheck));
                        }
                        else
                        {
                            debugText = "Некого десантирoвать";
                        }
                    }
                }

                if (transform.position.y != flyHeight)
                {
                    debugText = "Набираю высоту";
                    targetPosition = transform.TransformPoint(Vector3.forward * flyZoneRadius);
                    targetPosition = new Vector3(targetPosition.x, flyHeight, targetPosition.z);
                }
            }
            else
            {
                startDeploed = false;
            }
        }

    }

    void Fly()
    {
        if (targetPosition - transform.position != Vector3.zero)
        {
            direction = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, direction, speedRotate * Time.deltaTime);
        }

        transform.position += transform.forward * (speedMove + (distanceToDeploed * 0.5f)) * Time.deltaTime;
        Debug.DrawLine(transform.position, targetPosition, Color.blue);
        Debug.DrawLine(defaultDeploedPosition, defaultDeploedPosition + Vector3.up * deploedHeight, Color.blue);
    }

    private IEnumerator DeploedBot(float time)
    {
        // -=Авто заполнение очереди =- //
        if (autoCreateBot == true)
        {
            //if (currentBot < maxBot)
            {
                if (queueBot.Length > 0 && queueBot[0] == 0)
                {
                    nextBotType = Random.Range(1, spawnObjPrefab.Length + 1);
                    queueBot[0] = nextBotType;
                }
            }
        }

        // -= Создание ботов из очереди =- //
        if (queueBot.Length > 0 && queueBot[0] > 0)
        {
            startPosition = transform.position;
            endPisition = new Vector3(startPosition.x, LevelController.terrain.SampleHeight(startPosition) + 0.5f, startPosition.z);
            distanceToGround = Vector3.Distance(startPosition, endPisition);

            if (checkRayCollision(startPosition, endPisition, distanceToGround) == false)
            {
                if (spawnObjPrefab[queueBot[0] - 1] != null)
                {
                    indexBot = queueBot[0] - 1;//Выбирает тип бота по индексу масива префабов
                    //Debug.Log("indexBot= "+ spawnObjPrefab[indexBot].name);
                    bool deploed = false;
                    for (int i = 0; i < maxBot; i++)//
                    {
                        if (deploed == false)
                        {
                            if (botObj[(indexBot * maxBot) + i].activeSelf == false)
                            {
                                //Time.timeScale = 0.01f;
                                //Debug.Log("NewBot= " + botObj[(indexBot * maxBot) + i].name + " Type= " + indexBot);
                                botObj[(indexBot * maxBot) + i].SetActive(true);
                                botObj[(indexBot * maxBot) + i].transform.position = transform.position;
                                botObj[(indexBot * maxBot) + i].GetComponent<NavMeshObstacle>().enabled = false;
                                botObj[(indexBot * maxBot) + i].GetComponent<BotController>().botState = SM_BotState.Deploed;
                                
                                //currentBot++;
                                ClearQueue();
                                deploed = true;
                            }
                        }
                    }
                }
                else
                {
                    debugText = "Некого десантирвать";
                }
            }
        }

        yield return new WaitForSeconds(time);

        if (distanceToDeploed < deploedRadius)
        {
            StartCoroutine(DeploedBot(time));
        }
    }

    bool checkRayCollision(Vector3 myPosition, Vector3 targetPosition, float distanceToGround)
    {
        bool collision = false;
        Ray rayCollision = new Ray(myPosition, targetPosition - myPosition);
        RaycastHit hitCollision;

        if (Physics.Raycast(rayCollision, out hitCollision, distanceToGround, layerMask))
        {
            Debug.DrawRay(myPosition, hitCollision.point - myPosition, Color.red, 1);
            //GameObject debugText = Instantiate(Resources.Load("Prefabs/DebugText"), hitCollision.point, Quaternion.identity) as GameObject;
            //debugText.GetComponent<UI_DebugText>().spacePosition = hitCollision.point;
            //debugText.GetComponent<UI_DebugText>().debugMessage = hitCollision.collider.name;
            collision = true;
        }
        else
        {
            
            if (CheckOverlapSphere(targetPosition) == true)
            {
                Debug.DrawRay(myPosition, targetPosition - myPosition, Color.black, 1);
                collision = true;
            }
            else
            {
                Debug.DrawRay(myPosition, targetPosition - myPosition, Color.green, 1);
            }
             
            Debug.DrawRay(myPosition, targetPosition - myPosition, Color.green, 1);
        }

        return collision;
    }

    bool CheckOverlapSphere(Vector3 targetPosition)
    {
        bool overlapCollision = false;
        float radius = 3f;
        Vector3 center = targetPosition + Vector3.up * radius;
        hitColliders = Physics.OverlapSphere(center, radius, layerMask);
        gizmoPosition = center;
        gizmoRadius = radius;

        if (hitColliders.Length > 0)
        {
            overlapCollision = true;
            //GameObject debugText = Instantiate(Resources.Load("Prefabs/User Interface/DebugText"), hitColliders[0].transform.position, Quaternion.identity) as GameObject;
            //debugText.GetComponent<UI_DebugText>().spacePosition = hitColliders[0].transform.position;
            //debugText.GetComponent<UI_DebugText>().debugMessage = hitColliders[0].gameObject.name;
            //gizmoColor = Color.red;
        }
        else
        {
            //gizmoColor = Color.green;
        }

        return overlapCollision;
    }

    void ClearQueue()
    {
        for (int i = 0; i < queueBot.Length; i++)
        {
            if (i + 1 < queueBot.Length)
            {
                queueBot[i] = queueBot[i + 1];
            }
            else
            {

                queueBot[i] = 0;
            }
        }
    }

    private IEnumerator ClearBot(float time)
    {
        yield return new WaitForSeconds(time * Time.deltaTime);

        if (autoClearCurrentBot == true)
        {
            //if (currentBot > 0)
            {
                //currentBot--;
            }
        }

        StartCoroutine(ClearBot(100));
    }

    void ClearBot()
    {
        if (autoClearCurrentBot == true)
        {
            /*
            if (team == Team.Red)
            {
                if (LevelController.arrayBotControllerRed.Count < currentBot)
                {
                    currentBot--;
                }
            }

            if (team == Team.Blue)
            {
                if (LevelController.arrayBotControllerBlue.Count < currentBot)
                {
                    currentBot--;
                }
            }
             */ 
        }
    }

    public void ClearDeploedPosition()
    {
        Debug.Log("ClearDeploedPosition");
        deploedPosition = defaultDeploedPosition;
    }

    public void AddBotQuoue(int botType)
    {
        for (int e = 0; e < queueBot.Length; e++)
        {
            if (queueBot[e] == 0)
            {
                queueBot[e] = nextBotType = botType+1;
                return;
            }
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = gizmoColor;
        //Gizmos.DrawSphere(gizmoPosition, gizmoRadius);
    }
}
