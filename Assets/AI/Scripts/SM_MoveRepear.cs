using UnityEngine;
using System.Collections;

public class SM_MoveRepear : StateMachineBehaviour 
{
    public BotController botState;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botState == null) { botState = animator.GetComponentInParent<BotController>(); }

        botState.aiMessage = "SM_MoveRepear";

        if (botState.repearState != null && botState.repearState == botState.moveTargetObj.GetComponent<RepearState>())
        {
            if (botState.repearState.botID == 0 || botState.repearState.botID == botState.GetInstanceID())
            {
                botState.repearState.botID = botState.GetInstanceID();
            }

            if (botState.repearState.botID == botState.GetInstanceID())
            {
                if ((botState.moveTargetObj.transform.position - botState.transform.position).sqrMagnitude <= botState.navMeshAgent.stoppingDistance)
                {
                    animator.SetTrigger("StartRepear");
                }
                else
                {
                    animator.SetTrigger("FindRepear");
                }
            }
            else
            {
                animator.SetTrigger("Wait");
                botState.navMeshAgent.ResetPath();
            }
        }
        else
        {
            animator.SetTrigger("FindRepear");
        }
    }
}
