using UnityEngine;
using System.Collections;

public class SM_ProcessReArm : StateMachineBehaviour {

    public BotController botState;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botState == null) { botState = animator.GetComponentInParent<BotController>(); }

        botState.aiMessage = "SM_ProcessReload";
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botState.moveTargetObj != null)
        {
            if ((botState.moveTargetObj.transform.position - botState.transform.position).sqrMagnitude <= botState.navMeshAgent.stoppingDistance)
            {
                botState.bodyRotation.targetTransform = null;
                //ReloadWeapon(botState.gunReloadLeftModul, botState.gunStateLeftModul);
                //ReloadWeapon(botState.gunReloadRightModul, botState.gunStateRightModul);

                if (botState.neadReload == 0)
                {
                    botState.moveTargetObj.GetComponent<RepearState>().botID = 0;
                    botState.moveTargetObj = null;
                    animator.SetTrigger("CheckState");
                }
            }
            else
            {
                animator.SetTrigger("MoveRepear");
            }
        }
        else
        {
            animator.SetTrigger("CheckState");
        }
    }

    /*
    void ReloadWeapon(GunReload gunReload, GunController gunState)
    {
        if (gunReload != null && gunState != null)
        {
            gunState.ammo += 1;

            if (gunState.ammo > gunState.maxAmmo)
            {
                gunState.ammo = gunState.maxAmmo;
            }
        }
    }
     */ 
}
