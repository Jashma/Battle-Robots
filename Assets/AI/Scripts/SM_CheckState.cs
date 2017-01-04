using UnityEngine;
using System.Collections;

public class SM_CheckState : StateMachineBehaviour
{
    public BotController botController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        botController.EnableWeaponModul();
        botController.aiMessage = "SM_CheckState";
        botController.AnalisReload();
        botController.AnalisHealth();

        float repear = Random.Range(1, 101);
        float reload = Random.Range(1, 101);

        if (botController.neadRepear >= repear && botController.repearPlaceObj != null)
        {
            animator.SetTrigger("FindRepear");
        }
        else
        {
            if (botController.neadReload >= reload && botController.repearPlaceObj != null)
            {
                animator.SetTrigger("FindReload");
            }
            else
            {
                if (botController.enemyObj == null)
                {
                    animator.SetTrigger(CheckCharacter());
                }
                else
                {
                    animator.SetTrigger("Fight");
                    botController.aiMessage = "Fight";
                }
            }
        }
    }
        

    string CheckCharacter(string character = "FreeHunt")
    {
        if (botController.SM_botCharacter == SM_BotCharacter.CaptureFactory)
        {
            character = "FindFactory";
            botController.aiMessage = "FindFactory";
        }

        if (botController.SM_botCharacter == SM_BotCharacter.FreeHunt)
        {
            character = "FreeHunt";
            botController.aiMessage = "FreeHunt";
        }

        if (botController.SM_botCharacter == SM_BotCharacter.CaptureBase)
        {
            character = "FindBase";
            botController.aiMessage = "FindBase";
        }

        return character;
    }
}
