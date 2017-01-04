using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RadarController : MonoBehaviour 
{
    public BotController selfBotController;
    public List<BotController> arrayEnemyBotController = new List<BotController>();
    public List<BotController> arrayEnemyRadarFound = new List<BotController>();
    public float timeCheck = 5;//Промежуток времени, через который делается проверка радаром ботов противника
    public float radarLength = 50;//Дальность действия радара
    public bool startCheck = false;//Для UI_RadarController флаг, что сделана проверка
    public LayerMask enemyFindlayerMask;//Маска слоев для поиска врага
    private float timeReaction = 0.5f;//Время реакции на вражеского бота
    private GameObject enemyObj;
    private Ray rayCollision;
    private RaycastHit hitCollision;
    private Vector3 startRay;
    private Vector3 endRay;

    void OnEnable()
    {
        if (selfBotController == null) 
        { 
            selfBotController = GetComponentInParent<BotController>();
            selfBotController.radarController = this;
        }
        
        if (selfBotController.team == Team.Red) 
        { 
            arrayEnemyBotController = LevelController.arrayBotControllerBlue; 
        }

        if (selfBotController.team == Team.Blue) 
        { 
            arrayEnemyBotController = LevelController.arrayBotControllerRed; 
        }

        StartCoroutine(StartCheck(timeCheck)); 
    }

    private IEnumerator StartCheck(float time)
    {
        yield return new WaitForSeconds(time);

        if (selfBotController.botState == SM_BotState.AiControl || selfBotController.botState == SM_BotState.PlayerControl)
        {
            startCheck = true;//Для UI_RadarController флаг, что сделана проверка

            CheckBotList();
            ClearAlarm();//Уменьшаем значение тревоги и обнуляем ссылку на текущего врага
        }

        StartCoroutine(StartCheck(timeCheck));
        StartCoroutine(EndCheck(timeCheck + 0.1f));
    }

    void CheckBotList()
    {
        float centrOfBot = selfBotController.characterController.height / 2;//Находим "середину" бота, что бы туда проверить лучом

        //Сортируем список врагов по расстоянию до этого бота, первым в списке будет ближайший
        arrayEnemyBotController = arrayEnemyBotController.OrderByDescending(obj => ((selfBotController.transform.position + (Vector3.up * centrOfBot)) - (obj.transform.position + (Vector3.up * centrOfBot))).sqrMagnitude).ToList();

        enemyObj = null;//Обнуляем локальный обьект врага

        for (int i = 0; i < arrayEnemyBotController.Count; i++)//Проходим по массиву врагов
        {
            if (arrayEnemyBotController[i].gameObject.activeSelf == true)//Если враг включен
            {
                if (arrayEnemyBotController[i].botState != SM_BotState.Dead)//Если враг не мертв
                {
                    //Радар
                    CheckRadar(arrayEnemyBotController[i]);
                    
                    //Проверяем лучом на прямую видимость
                    enemyObj = CheckEnemyBot(arrayEnemyBotController[i]);

                    //Назначаем себе врага
                    SetEnemy(enemyObj);
                }
                else
                {
                    //Если враг уже мертв, то сбрасываем тревогу
                    if (selfBotController.enemyObj == enemyObj)
                    {
                        //selfBotController.alarm = 0;
                    }
                }
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
        endRay = enemyBot.transform.position + (Vector3.up * enemyBot.characterController.height / 2);//Середина бота

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
                ClearAction();//Отменяем все выполняемые действия
                selfBotController.stateMashine.CrossFade("CheckState", Random.Range(0.1f, timeReaction));//Сбрасываем текущее действие аи
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

    void ClearAlarm()//Уменьшаем значение тревоги и обнуляем ссылку на текущего врага
    {
        if (selfBotController.alarm > 0)
        {
            selfBotController.alarm--;
        }
        else
        {
            selfBotController.enemyObj = null;
        }
    }

    void ClearAction()//Отменяем все выполняемые действия
    {
        if (selfBotController.captureState != null && selfBotController.captureState.botID == selfBotController.GetInstanceID())
        {
            if (selfBotController.startCapture == true && selfBotController.captureState.capture > 0)
            {
                selfBotController.captureState.capture = 0;
                selfBotController.startCapture = false;
            }

            selfBotController.captureState.botID = 0;
            selfBotController.captureState = null;
        }

        if (selfBotController.repearState != null && selfBotController.repearState.botID == selfBotController.GetInstanceID())
        {
            selfBotController.repearState.botID = 0;
            selfBotController.repearState = null;
            selfBotController.animator.SetBool("Repear", false);
            selfBotController.animator.SetBool("Reload", false);
        }
    }

    private IEnumerator EndCheck(float time)//Для класса UI_RadarController. Означает что проверка закончена и проигрывать анимацию радара не нужно. 
    {
        yield return new WaitForSeconds(time);
        startCheck = false; 
    }
}
