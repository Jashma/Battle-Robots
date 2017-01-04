using UnityEngine;
using System.Collections;

public class ChangeModul : SupportBotController
{

    void Start()
    {
        thisTransform = transform;
        targetPosition = thisTransform.position;
        action = SM_BotAction.ChangeModul;
    }

    void FixedUpdate()
    {
        Move();
    }

    public override void ProcessAction(BotController botController)
    {
        if (botController.botAction == action)
        {
            base.ProcessAction();
            targetPosition = botController.thisTransform.position;

            //Вращаем бота в сторону игрока
            Quaternion direction = Quaternion.LookRotation(PlayerController.Instance.transform.position - botController.thisTransform.position);
            botController.thisTransform.rotation = Quaternion.RotateTowards(botController.thisTransform.rotation, direction, botController.speedRotate * Time.deltaTime);
            botController.thisTransform.localEulerAngles = new Vector3(0, botController.thisTransform.localEulerAngles.y, 0);
        }
    }
}
