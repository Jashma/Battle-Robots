using UnityEngine;
using System.Collections;

public class RepearState : MonoBehaviour 
{
    public LayerMask layerMask;
    public int botID;
    private Ray rayCollision;
    private RaycastHit hitCollision;

    void OnTriggerEnter(Collider other)
    {
        float distance = Vector3.Distance (transform.position, other.transform.position + Vector3.down);

        if (other.tag == "Radar")
        {
            // -= Проверяем лучем столкновение от камеры до этого бота =- //
            rayCollision = new Ray(transform.position, (other.transform.position + Vector3.down) - transform.position);
			
            // -= Если луч столкнулся с колайдером =- //
			if (Physics.Raycast(rayCollision, out hitCollision, distance, layerMask))
            {
                if (hitCollision.collider.gameObject == other.transform.parent.gameObject)
                {
                    Debug.DrawRay(transform.position, ((other.transform.position + Vector3.down) - transform.position), Color.green, 5);
                    other.GetComponentInParent<BotController>().repearPlaceObj = this.gameObject;
                }
            }
        }
    }
}
