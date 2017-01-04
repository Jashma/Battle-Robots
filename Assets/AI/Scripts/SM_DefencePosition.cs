using UnityEngine;
using System.Collections;

public class SM_DefencePosition : StateMachineBehaviour 
{
    public BotController botController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        botController.aiMessage = "SM_DefencePosition";

    }
	
}
