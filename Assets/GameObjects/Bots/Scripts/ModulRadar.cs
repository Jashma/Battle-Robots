using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ModulRadar : ModulBasys
{
    [HideInInspector]
    public BotController selfBotController;
    [HideInInspector]
    public List<BotController> arrayEnemyBotController = new List<BotController>();
    [HideInInspector]
    public List<BotController> arrayEnemyRadarFound = new List<BotController>();
    [HideInInspector]
    public bool startCheck = false;//Для UI_RadarController флаг, что сделана проверка

    public float timeCheck = 1;//Промежуток времени, через который делается проверка радаром ботов противника
    public float radarLength = 50;//Дальность действия радара
    public LayerMask enemyFindlayerMask;//Маска слоев для поиска врага

    private float timeReaction = 0.5f;//Время реакции на вражеского бота
    private GameObject enemyObj;
    private Ray rayCollision;
    private RaycastHit hitCollision;
    private Vector3 startRay;
    private Vector3 endRay;
    private float nextTime;

    void OnEnable()
    {

        if (selfBotController == null)
        {
            selfBotController = GetComponentInParent<BotController>();
        }

        if (selfBotController.team == Team.Friend)
        {
            arrayEnemyBotController = GameController.arrayBotControllerEnemyTeam;
        }

        if (selfBotController.team == Team.Enemy)
        {
            arrayEnemyBotController = GameController.arrayBotControllerFriendTeam;
        }

        nextTime = Time.time + timeCheck;
    }

    void Update()
    {
        if (nextTime <= Time.time)
        {
            nextTime = Time.time + timeCheck;
            startCheck = true;//Для UI_RadarController флаг, что сделана проверка
            CheckBotList();
        }
        else
        {
            startCheck = false;
        }

    }

    void CheckBotList()
    {
        float centrOfBot = selfBotController.botHeight / 2;//Находим "середину" бота, что бы туда проверить лучом

        //Сортируем список врагов по расстоянию до этого бота, первым в списке будет ближайший
        arrayEnemyBotController = arrayEnemyBotController.OrderByDescending(obj => ((selfBotController.transform.position + (Vector3.up * centrOfBot)) - (obj.transform.position + (Vector3.up * centrOfBot))).sqrMagnitude).ToList();

        enemyObj = null;//Обнуляем локальный обьект врага

        for (int i = 0; i < arrayEnemyBotController.Count; i++)//Проходим по массиву врагов
        {
            if (arrayEnemyBotController[i].gameObject.activeSelf == true)//Если враг включен
            {
                //Радар
                CheckRadar(arrayEnemyBotController[i]);

                //Проверяем лучом на прямую видимость
                enemyObj = CheckEnemyBot(arrayEnemyBotController[i]);

                //Назначаем себе врага
                SetEnemy(enemyObj);
            }
        }
    }

    void CheckRadar(BotController enemyBot)//Радар
    {
        if (PlayerController.botController != null && PlayerController.botController == selfBotController)
        {
            if (radarLength >= Vector3.Distance(transform.position, enemyBot.transform.position))
            {
                enemyBot.timeRadarFound = Time.time + 1;
                //У каждого бота в классе BotController если метод сброса значения timeRadarFound
            }
        }
    }

    GameObject CheckEnemyBot(BotController enemyBot)
    {
        startRay = transform.position;
        endRay = enemyBot.transform.position + (Vector3.up * enemyBot.botHeight / 2);//Середина бота

        //Прямая видимость
        if (Physics.Linecast(startRay, endRay, enemyFindlayerMask))
        {
            //Debug.DrawLine(startRay, endRay, Color.red, 0.3f);//Если луч столкнулся с преградой, то возвращаем нуль
            return null;
        }
        else
        {
            //Debug.DrawLine(startRay, endRay, Color.green, 0.3f);
            //Если луч не столкнулся с преградой, то увеличиваем значение "засвета бота" и возвращаем его как обьект - враг
            enemyBot.timeVisualFound = Time.time + 1;
            return enemyBot.gameObject;
        }
    }

    void SetEnemy(GameObject enemy)//Назначаем себе врага
    {
        if (enemy != null)
        {
            selfBotController.alarm = 10;//Повышаем значение "Тревоги"

            //Если у бота нет врага 
            if (selfBotController.enemyObj == null)
            {
                selfBotController.enemyObj = enemy;//Назначем ссылку на врага
            }
            else
            {
                //Если враг не тот же, что и обнаруженый, назначаем его врагом, повышаем значение "Тревоги"
                if (selfBotController.enemyObj != enemy)
                {
                    selfBotController.enemyObj = enemy;
                }
            }
        }
    }

    public override ModulRadar GetRadarController()
    {
        return this;
    }

    public override bool GetDamage(float damage, float power, bool showFlyHit)
    {
        base.GetDamage(damage, power, showFlyHit);
        return false;
    }
}
