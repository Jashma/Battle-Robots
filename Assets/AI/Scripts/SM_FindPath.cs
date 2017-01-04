using UnityEngine;
using System.Collections;

public class SM_FindPath : StateMachineBehaviour 
{
    public BotController botController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        if (!botController.navMeshAgent.hasPath)
        {
            NavMesh.CalculatePath(botController.transform.position, botController.moveTargetPosition, NavMesh.AllAreas, botController.path);

            if (botController.path.status == NavMeshPathStatus.PathComplete)
            {
                if (botController.moveTargetPosition != Vector3.zero)
                {
                    botController.navMeshAgent.SetDestination(botController.moveTargetPosition);
                    animator.SetBool("MovePath", true);
                }
                else
                {
                    animator.SetBool("MovePath", false);
                    botController.navMeshAgent.ResetPath();
                }
            }
            else
            {
                animator.SetBool("MovePath", false);
                botController.navMeshAgent.ResetPath();
            }
        }
    }
	
}
