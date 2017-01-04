using UnityEngine;
using System.Collections;

public class SM_MovePath : StateMachineBehaviour 
{
    public BotController botController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); } 
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        botController.navMeshAgent.speed = botController.speedWalk;
        botController.navMeshAgent.angularSpeed = botController.speedRotate;

        if (botController.navMeshAgent.hasPath)
        {
            if ((botController.navMeshAgent.destination - botController.transform.position).sqrMagnitude <= botController.navMeshAgent.stoppingDistance)
            {
                animator.SetBool("MovePath", false);
                botController.navMeshAgent.ResetPath();
            }
        }
    }
}
