using UnityEngine;
using System.Collections;

public class SM_FightMoveDirection : StateMachineBehaviour 
{
    public BotController botController;
    private float radarDistance = 30;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }

        botController.aiMessage = "SM_FightMoveDirection";

        if (botController.enemyObj != null)
        {
            Vector3 startRay = botController.transform.position + new Vector3(0, botController.characterController.height / 2, 0);
            Vector3 endRay = botController.enemyObj.transform.position + new Vector3(0, botController.characterController.height / 2, 0);
            float distance = Vector3.Distance(startRay, endRay);

            if (distance >= radarDistance)
            {
                botController.moveTargetPosition = botController.enemyObj.transform.position;
                botController.navMeshAgent.SetDestination(botController.moveTargetPosition);
            }
            else
            {
                botController.moveTargetPosition = RandomDirection();
                botController.navMeshAgent.SetDestination(botController.moveTargetPosition);
            }
        }
    }

    Vector3 RandomPosition()
    {
        Vector3 randomPlace = new Vector3(Random.Range(10, -10), 0, Random.Range(1, -1));
        return botController.transform.position + randomPlace;
    }

    Vector3 RandomDirection()
    {
        Vector3 rndDirection = Vector3.zero;
        int rnd = Random.Range(0,3);

        if (rnd == 0)
        {
            rndDirection = botController.transform.TransformPoint(Vector3.back * 10);
        }

        if (rnd == 1)
        {
            rndDirection = botController.transform.TransformPoint(Vector3.right * 10);
        }

        if (rnd == 2)
        {
            rndDirection = botController.transform.TransformPoint(Vector3.left * 10);
        }

        return rndDirection;
    }

}
