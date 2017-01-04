using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SM_FindRearmPlace : StateMachineBehaviour {

    public BotController botController;
    //private List<FactoryState> arrayFactoryState = new List<FactoryState>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        botController.aiMessage = "SM_FindRearmPlace";

        if (botController.repearState != null)
        {
            botController.moveTargetPosition = botController.repearState.transform.position;
            botController.moveTargetObj = botController.repearState.gameObject;
            botController.navMeshAgent.SetDestination(botController.moveTargetPosition);

            if ((botController.moveTargetObj.transform.position - botController.transform.position).sqrMagnitude < 50)
            {
                if (botController.repearState.botID == 0 || botController.repearState.botID == botController.GetInstanceID())
                {
                    animator.SetTrigger("Wait");
                    botController.navMeshAgent.ResetPath();
                }
                else
                {
                    animator.SetTrigger("MoveRearm");
                }
            }
            else
            {
                animator.SetTrigger("MoveRearm");
            }
        }
        else
        {
            animator.SetTrigger(FindFactoryArray(animator, LevelController.arrayFactoryState));
        }
    }

    string FindFactoryArray(Animator animator, List<FactoryState> arrayFactoryState, string findFactory = "CheckState")
    {
        arrayFactoryState = arrayFactoryState.OrderByDescending(obj => (botController.transform.position - obj.transform.position).sqrMagnitude).ToList();

        int i = arrayFactoryState.Count;

        while (i > 0)
        {
            i--;

            if (arrayFactoryState[i].captureState.team == botController.team)
            {
                findFactory = "CheckState";
                i = 0;
            }
        }

        return findFactory;
    }
}
