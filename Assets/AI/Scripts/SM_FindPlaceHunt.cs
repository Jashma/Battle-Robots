using UnityEngine;
using System.Collections;

public class SM_FindPlaceHunt : StateMachineBehaviour 
{
    public BotController botController;
    public bool getMovePoint;
    public float radius;
    public GameObject tmpObject;
    //public Terrain currentTerrain;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (botController == null) { botController = animator.GetComponentInParent<BotController>(); }
        //if (currentTerrain == null) { currentTerrain = GameObject.Find("LevelController").GetComponent<LevelController>().currentTerrain; }

        botController.aiMessage = "SM_FindPlaceHunt";

        if ((botController.moveTargetPosition - botController.transform.position).sqrMagnitude <= botController.navMeshAgent.stoppingDistance)
        {
            radius = botController.navMeshAgent.radius;
            float randomX = Random.Range((LevelController.terrain.terrainData.size.x / 3) * -1, LevelController.terrain.terrainData.size.x / 3);
            float randomZ = Random.Range((LevelController.terrain.terrainData.size.z / 3) * -1, LevelController.terrain.terrainData.size.z / 3);
            
            //botController.moveTargetPosition = CheckPosition(radius, new Vector3(randomX, botController.transform.position.y + 0.2f, randomZ), botController.enemyFindlayerMask);

            botController.moveTargetPosition = new Vector3(randomX, botController.transform.position.y, randomZ);
            botController.navMeshAgent.SetDestination(botController.moveTargetPosition);
            botController.aiMessage = "Create New Path";

            if (botController.navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                botController.aiMessage = "Invalid path";
                botController.moveTargetPosition = botController.transform.position;
                botController.navMeshAgent.SetDestination(botController.moveTargetPosition);
            }
        }
        else
        {
            botController.aiMessage = botController.navMeshAgent.pathStatus.ToString();

            if (botController.navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                botController.moveTargetPosition = botController.transform.position;
                botController.navMeshAgent.SetDestination(botController.moveTargetPosition);
            }
        }
    }

    Vector3 CheckPosition(float radius, Vector3 position, LayerMask layerMask)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position + new Vector3(0, radius, 0), radius, layerMask);

        if (hitColliders.Length > 0)
        {
            return botController.transform.position;  
        }
        else
        {
            return position;
        }
    }

}
