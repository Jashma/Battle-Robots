using UnityEngine;
using System.Collections;

public class SM_CheckDistance : StateMachineBehaviour 
{
    public BotController botController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        if (Vector3.Distance(botController.transform.position, botController.moveTargetPosition) < 10)
        {
            animator.SetBool("EndState", true);
        }
        else
        {
            animator.SetBool("EndState", false);
        }
	}

}
