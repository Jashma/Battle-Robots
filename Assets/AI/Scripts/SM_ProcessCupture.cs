using UnityEngine;
using System.Collections;

public class SM_ProcessCupture : StateMachineBehaviour {

    public BotController botState;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botState == null) { botState = animator.GetComponentInParent<BotController>(); }

        botState.aiMessage = "SM_ProcessCapture";

        if (botState.captureState != null && botState.captureState == botState.moveTargetObj.GetComponentInChildren<CaptureState>())
        {
            if ((botState.moveTargetPosition - botState.transform.position).sqrMagnitude < botState.navMeshAgent.stoppingDistance)
            {
                if (botState.captureState.team != botState.team)
                {
                    botState.aiMessage = "Start Animation Capture";
                    botState.StartCaptureAnimation();
                }
                else
                {
                    botState.aiMessage = "Capture object team = my team";

                    botState.animator.SetBool("Capture", false);
                    if (botState.captureState.botID == botState.GetInstanceID())
                    {
                        botState.captureState.botID = 0;
                    }

                    animator.SetTrigger("CheckState");
                }
            }
            else
            {
                botState.aiMessage = "Big Distance To Base " + Vector3.Distance(botState.moveTargetObj.transform.position, botState.transform.position).ToString("f2");

                animator.SetTrigger("MoveCapture");

                if (botState.startCapture == true)
                {
                    botState.startCapture = false;

                    if (botState.captureState.botID == botState.GetInstanceID())
                    {
                        botState.captureState.botID = 0;
                    }
                }
            }
        }
    }
}
