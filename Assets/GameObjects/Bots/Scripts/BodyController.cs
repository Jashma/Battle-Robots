using UnityEngine;
using System.Collections;

public class BodyController : MonoBehaviour 
{
    public float speedRotateBody;
    public float rotationAngle;
    public Vector3 pointSniper;

    void OnEnable()
    {
        GetComponentInParent<BotController>().bodyController = this;
    }
}
