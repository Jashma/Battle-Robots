using UnityEngine;
using System.Collections;

public class BodyRotation : MonoBehaviour 
{
    private BotController botController;
    public BodyController bodyController;
    private Quaternion directionBody;//вектор, куда нужно вращать башню
    public Vector3 bodyTarget;
    public Transform targetTransform;
    

    void OnEnable()
    {
        botController = GetComponentInParent<BotController>();
        botController.bodyRotation = this;
    }

    void LateUpdate()
    {
        if (botController.botState == SM_BotState.PlayerControl || botController.botState == SM_BotState.AiControl)
        {
            if (bodyController == null)
            {
                bodyController = GetComponentInChildren<BodyController>();
            }
            else
            {
                GetTarget();
                ClampRotation();
                RotateBody();
            }
        }
    }

    void GetTarget()
    {
        if (botController.botState == SM_BotState.AiControl)
        {
            if (botController.enemyObj != null && botController.alarm > 5)
            {
                targetTransform = botController.enemyObj.transform;
            }
            else
            {
                targetTransform = null;
            }

            if (targetTransform == null)
            {
                bodyTarget = botController.transform.TransformPoint(Vector3.forward * 100);
            }
            else
            {
                bodyTarget = targetTransform.position + new Vector3(0, botController.characterController.height / 2, 0);
            }
        }

        if (botController.botState == SM_BotState.PlayerControl)
        {
            if (botController.startChangeModule == false)
            {
                bodyTarget = PlayerController.hitForwardCollision.point;
            }
            else
            {
                bodyTarget = botController.transform.TransformPoint(Vector3.forward * 100);
            }
        }
    }

    void ClampRotation()
    {
        directionBody = Quaternion.LookRotation(bodyTarget - transform.position);

        if (bodyController.rotationAngle <= 0)
        {
            //если угол между башней и корпусом меньше ограничения угла
            if (Vector3.Angle(transform.forward, botController.transform.forward) < bodyController.rotationAngle)
            {
                RotateBody();
            }
            else
            {//Если угол больше ограничения но угол между векторами камеры и танка меньше ограничения
                if (Vector3.Angle((bodyTarget - botController.transform.position), botController.transform.forward) < bodyController.rotationAngle)
                {
                    RotateBody();
                }
            }
        }
        else
        {
            RotateBody();
        }
    }

    void RotateBody()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, directionBody, bodyController.speedRotateBody * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, directionBody, (speedRotateBody * 0.1f) * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

        //Debug.DrawLine(botController.transform.position, transform.TransformPoint(Vector3.forward * 100), Color.yellow);
        //Debug.DrawLine(transform.position, bodyTarget, Color.red);
        //float tmpDist = Vector3.Distance(parentTransform.transform.position, bodyTarget);
        //Debug.DrawLine(parentTransform.transform.position, parentTransform.transform.TransformPoint(Vector3.forward * tmpDist), Color.green);
        
    }
}
