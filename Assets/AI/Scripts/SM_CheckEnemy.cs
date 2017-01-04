using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SM_CheckEnemy : StateMachineBehaviour 
{
    public BotController botController;
    public GameObject enemyObj;
    public static List<BotController> arrayEnemyBotState = new List<BotController>();

    /*
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        CheckBotList(animator);
        ClearAlarm(animator);
    }

    void CheckBotList(Animator animator)
    {
        if (botController.team == Team.Red) { arrayEnemyBotState = LevelController.arrayBotControllerBlue; }
        if (botController.team == Team.Blue) { arrayEnemyBotState = LevelController.arrayBotControllerRed; }

        Vector3 tempVector = new Vector3(0, botController.characterController.height / 2, 0);

        arrayEnemyBotState = arrayEnemyBotState.OrderByDescending(obj => ((botController.transform.position + tempVector) - (obj.transform.position + tempVector)).sqrMagnitude).ToList();

        enemyObj = null;

        for (int i = 0; i < arrayEnemyBotState.Count; i++)
        {
            {
                enemyObj = CheckEnemyBot(arrayEnemyBotState[i]);
            }

            if (enemyObj != null)
            {
                if (arrayEnemyBotState[i].botState != SM_BotState.Dead)
                {
                    if (botController.enemyObj == null)
                    {
                        ClearAction(animator);
                        animator.CrossFade("CheckState", Random.Range(0.1f, 3));
                    }

                    if (botController.enemyObj != enemyObj)
                    {
                        
                        botController.enemyObj = enemyObj;
                        botController.alarm = 10;
                    }
                    else
                    {
                        botController.alarm = 10;
                    }
                }
                else
                {
                    if (botController.enemyObj == enemyObj)
                    {
                        botController.alarm = 0;
                    }
                }
            }
        }
    }

    GameObject CheckEnemyBot(BotController enemyBot)
    {
        if (enemyBot != botController)
        {
            Vector3 startRay = botController.transform.position + new Vector3(0, botController.characterController.height / 2, 0);
            Vector3 endRay = enemyBot.transform.position + new Vector3(0, enemyBot.characterController.height / 2, 0);
            Ray rayCollision = new Ray(startRay, endRay - startRay);
            RaycastHit hitCollision;
            float distance = Vector3.Distance(startRay, endRay);

            if (botController.radarLength >= distance)
            {
                enemyBot.timeRadarFound = Time.time + 2;
            }

            
            if (!Physics.Raycast(rayCollision, out hitCollision, distance, botController.enemyFindlayerMask))
            {
                Debug.DrawRay(startRay, endRay - startRay, Color.grey, 0.3f);
                
                enemyBot.timeVisualFound = Time.time + 2;
                return enemyBot.gameObject;
            }
            else
            {
                Debug.DrawRay(startRay, hitCollision.point - startRay, Color.white, 0.3f);
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    void ClearAlarm(Animator animator)
    {
        if (botController.alarm > 0)
        {
            botController.alarm--;
        }
        else
        {
            botController.enemyObj = null;
        }
    }

    void ClearAction(Animator animator)
    {
        if (botController.captureState != null && botController.captureState.botID == botController.GetInstanceID())
        {
            if (botController.startCapture == true && botController.captureState.capture > 0)
            {
                botController.captureState.capture = 0;
                botController.startCapture = false;
            }

            botController.captureState.botID = 0;
            botController.captureState = null;
        }

        if (botController.repearState != null && botController.repearState.botID == botController.GetInstanceID())
        {
            botController.repearState.botID = 0;
            botController.repearState = null;
            botController.animator.SetBool("Repear", false);
            botController.animator.SetBool("Reload", false);
        }
    }
    */
}
