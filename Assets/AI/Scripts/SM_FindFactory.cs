using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SM_FindFactory : StateMachineBehaviour
{
    private BotController botController;
    //private List<FactoryState> arrayFactoryState = new List<FactoryState>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        botController.aiMessage = "SM_FindFactory";

        GameObject factory = FindFactoryArray(animator, LevelController.arrayFactoryState);

        if (factory != null)
        {
            
                botController.moveTargetPosition = factory.transform.position;
                botController.moveTargetObj = factory;

                if (botController.navMeshAgent.destination != botController.moveTargetPosition)
                {
                    botController.navMeshAgent.SetDestination(botController.moveTargetPosition);
                }


            if (botController.captureState != null)
            {
                if (botController.captureState.botID == 0 || botController.captureState.botID == botController.GetInstanceID())
                {
                    animator.SetTrigger("MoveCapture");
                }
                else
                {
                    animator.SetTrigger("Wait");
                    botController.navMeshAgent.ResetPath();
                }
            }
            else
            {
                animator.SetTrigger("MoveCapture");
            }
        }
        else
        {
            animator.CrossFade("CreateCharacter", 0.1f);
        }
    }

    GameObject FindFactoryArray(Animator animator, List<FactoryState> arrayFactoryState, GameObject factory = null)
    {
        arrayFactoryState = arrayFactoryState.OrderByDescending(obj => (botController.transform.position - obj.transform.position).sqrMagnitude).ToList();
        
        int i = arrayFactoryState.Count;

        while (i > 0)
        {
            i--;

            if (arrayFactoryState[i].captureState.team != botController.team)
            {
                factory = arrayFactoryState[i].gameObject;
                i = 0;
            }
        }

        return factory;
    }
}
