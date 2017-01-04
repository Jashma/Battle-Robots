using UnityEngine;
using System.Collections;

public abstract class SupportBotController : MonoBehaviour
{
    public Team team;
    protected bool readyAction = false;
    private float moveSpeed = 10;
    private float rotateSpeed = 70;
    public SM_BotAction action;
    protected Vector3 targetPosition;
    protected Transform thisTransform;
    

    public virtual void ProcessAction(BotController botController = null)
    {
        if (readyAction == true)
        {
            thisTransform.eulerAngles += Vector3.up * rotateSpeed * Time.deltaTime;
        }
    }

    public virtual void Move()
    {
        Vector3 direction = (targetPosition - thisTransform.position);
        GetComponent<Rigidbody>().velocity = direction;

        if (Vector3.Distance(thisTransform.position, targetPosition) < 1)
        {
           readyAction = true;
        }
        else
        {
            readyAction = false;
        }
    }
}
