using UnityEngine;
using System.Collections;

public class SM_ProcessRepear : StateMachineBehaviour 
{
    public BotController botController;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        botController.aiMessage = "SM_ProcessRepear";

        if (botController.repearState != null && botController.repearState == botController.moveTargetObj.GetComponent<RepearState>())
        {
            if ((botController.moveTargetObj.transform.position - botController.transform.position).sqrMagnitude <= botController.navMeshAgent.stoppingDistance)
            {
                if (botController.health < 100)
                {
                    botController.StartRepearAnimation();
                }
                else
                {
                    botController.AnalisHealth();
                    botController.animator.SetBool("Repear", false);
                     if (botController.repearState.botID == botController.GetInstanceID())
                    {
                        botController.repearState.botID = 0;
                    }

                    animator.SetTrigger("CheckState");
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
