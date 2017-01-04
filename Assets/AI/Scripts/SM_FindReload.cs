using UnityEngine;
using System.Collections;

public class SM_FindReload : StateMachineBehaviour
{
    public BotController botController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        botController.aiMessage = "SM_FindReload";

        if (botController.repearPlaceObj != null)
        {
            botController.moveTargetPosition = botController.repearPlaceObj.transform.position;
            botController.moveTargetObj = botController.repearPlaceObj;
            if (botController.navMeshAgent.destination != botController.moveTargetPosition)
            {
                botController.navMeshAgent.SetDestination(botController.moveTargetPosition);
            }

            if (botController.repearState != null)
            {
                if (botController.repearState.botID == 0 || botController.repearState.botID == botController.GetInstanceID())
                {
                    animator.SetTrigger("MoveRepear");
                }
                else
                {
                    animator.SetTrigger("Wait");
                    botController.navMeshAgent.ResetPath();
                }
            }
            else
            {
                animator.SetTrigger("MoveRepear");
            }
        }
        else
        {
            animator.SetTrigger("CheckState");
        }
    }

}
