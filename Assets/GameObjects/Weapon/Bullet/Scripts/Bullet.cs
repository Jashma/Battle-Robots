using UnityEngine;
using System.Collections;

public class Bullet : ProjectileBasys
{
    private bool hitUnit = false;

    void OnEnable()
    {
        hitUnit = false;
        currentFlyDistance = flyDistance;
        speed = energyCurrent * 0.1f;
        //Debug.Log("speed = " + speed + "  energyCurrent = " + energyCurrent);
    }

    void Start()
    {
        flyTransform = transform.parent;
    }

    void Update()
    {
        hitUnit = CheckHit(flyTransform.position, flyTransform, speed);
        if (hitUnit == true)
        {
            Hit();
        }

        CheckIsLive();
    }

    void LateUpdate()
    {
        Fly(flyTransform, speed);
    }

    public override void CheckIsLive()
    {
        if (hitUnit == true || currentFlyDistance < 0)
        {
            SendMessageUpwards("PostEffect");
            currentFlyDistance = 0;
        }
    }
}
