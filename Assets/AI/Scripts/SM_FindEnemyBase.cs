using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SM_FindEnemyBase : StateMachineBehaviour 
{
    private BotController botController;
    //private List<BaseState> arrayBaseState = new List<BaseState>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        botController.aiMessage = "SM_FindEnemyBase";

        GameObject enemyBase = FindBaseArray(animator, LevelController.arrayBaseState);

        if (enemyBase != null)
        {
            botController.moveTargetPosition = enemyBase.GetComponentInChildren<CaptureState>().transform.position;
            botController.moveTargetObj = enemyBase;

            if (botController.navMeshAgent.destination != botController.moveTargetPosition)
            {
                botController.navMeshAgent.SetDestination(botController.moveTargetPosition);
            }

            if (botController.captureState != null)
            {
                if (botController.captureState.botID == 0 || botController.captureState.botID == botController.GetInstanceID())
                {
                    animator.SetTrigger("MoveCapture");
                    botController.aiMessage = "Capture Place Empty MoveCapture";
                }
                else
                {
                    animator.SetTrigger("Wait");
                    botController.navMeshAgent.ResetPath();
                    botController.aiMessage = "Capture Place Close, Wait";
                }
            }
            else
            {
                animator.SetTrigger("MoveCapture");
                botController.aiMessage = "No CaptureState, MoveCapture";
            }
        }
        else
        {
            animator.CrossFade("CreateCharacter", 0.1f);
            botController.aiMessage = "CreateCharacter";
        }
    }

    GameObject FindBaseArray(Animator animator, List<BaseState> arrayBaseState, GameObject enemyBase = null)
    {
        int i = arrayBaseState.Count;

        while (i > 0)
        {
            i--;

            if (arrayBaseState[i].captureState.team != botController.team)
            {
                enemyBase = arrayBaseState[i].gameObject;
                i = 0;
            }
        }

        return enemyBase;
    }
}
