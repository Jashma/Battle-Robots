using UnityEngine;
using System.Collections;

public class SM_ProcessReload : StateMachineBehaviour 
{
    public BotController botController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        botController.aiMessage = "SM_ProcessReload";
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController.repearState != null && botController.repearState == botController.moveTargetObj.GetComponent<RepearState>())
        {
            if ((botController.moveTargetObj.transform.position - botController.transform.position).sqrMagnitude <= botController.navMeshAgent.stoppingDistance)
            {
                botController.AnalisReload();

                if (botController.neadReload > 0)
                {
                    botController.StartReloadAnimation();
                }
                else
                {
                    botController.animator.SetBool("Reload", false);
                    if (botController.repearState.botID == botController.GetInstanceID())
                    {
                        botController.repearState.botID = 0;
                    }

                    animator.SetTrigger("CheckState");
                }
            }
            else
            {
                botController.animator.SetBool("Reload", false);
                if (botController.repearState.botID == botController.GetInstanceID())
                {
                    botController.repearState.botID = 0;
                }

                animator.SetTrigger("MoveRepear");
            }
        }
        else
        {
            animator.SetTrigger("CheckState");
        }
    }
}
