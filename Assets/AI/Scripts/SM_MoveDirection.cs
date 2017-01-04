using UnityEngine;
using System.Collections;

public class SM_MoveDirection : StateMachineBehaviour 
{
    public BotController botController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        botController.aiMessage = "SM_MoveDirection";

        if (botController.captureState != null && botController.captureState == botController.moveTargetObj.GetComponentInChildren<CaptureState>())
        {
            if (botController.captureState.botID == 0 || botController.captureState.botID == botController.GetInstanceID())
            {
                botController.captureState.botID = botController.GetInstanceID();
            }

            if (botController.captureState.botID == botController.GetInstanceID())
            {
                if ((botController.moveTargetPosition - botController.transform.position).sqrMagnitude <= botController.navMeshAgent.stoppingDistance)
                {
                    animator.SetTrigger("StartCapture");
                    botController.aiMessage = "StartCapture";
                }
                else
                {
                    botController.aiMessage = "Too Far StartCapture";
                    animator.SetTrigger("CheckState");
                }
            }
            else
            {
                if (botController.captureState.botID == 0)
                {
                    botController.aiMessage = "StartCapture Id == 0";
                    animator.SetTrigger("CheckState");
                }
                else
                {
                    botController.aiMessage = "StartCapture plase not empty";
                    animator.SetTrigger("Wait");
                    botController.navMeshAgent.ResetPath();
                }
            }
        }
        else
        {
            botController.aiMessage = "No captureState or captureState is other";
            animator.SetTrigger("CheckState");
        }
    }

}
