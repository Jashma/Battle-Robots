using UnityEngine;
using System.Collections;

public class SM_CreateCharacter : StateMachineBehaviour 
{
    private BotController botController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        botController.aiMessage = "SM_CreateCharacter";

        int rnd = Random.Range(0,3);

        if (rnd == 0)
        {
            botController.SM_botCharacter = SM_BotCharacter.CaptureFactory;
        }

        if (rnd == 1)
        {
            botController.SM_botCharacter = SM_BotCharacter.FreeHunt;
            botController.moveTargetPosition = botController.transform.position;
        }

        if (rnd == 2)
        {
            botController.SM_botCharacter = SM_BotCharacter.CaptureBase;
        } 
    }
}
