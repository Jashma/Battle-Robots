using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class SM_CharacterFight : StateMachineBehaviour 
{
    public BotController botState;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botState == null) { botState = animator.GetComponentInParent<BotController>(); }

    }
}
