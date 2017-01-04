using UnityEngine;
using System.Collections;

public class SM_Wait : StateMachineBehaviour 
{
    public BotController botController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        if ((botController.moveTargetObj.transform.position - botController.transform.position).sqrMagnitude <= botController.navMeshAgent.stoppingDistance*2)
        {
            Vector3 tempPosition = ((botController.transform.position - Vector3.back * 3) - botController.transform.position);
            botController.navMeshAgent.SetDestination(tempPosition);
            botController.aiMessage = "SM_Move Wait";
        }
        else
        {
            botController.aiMessage = "SM_Wait";
        }

        
    }

}
