using UnityEngine;
using System.Collections;

public class Rocket : ProjectileBasys
{
    private bool hitUnit = false;
    public float radius = 5;
    public float rocketSpeed = 500;

    void OnEnable()
    {
        hitUnit = false;
        currentFlyDistance = flyDistance;
        speed = rocketSpeed;
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
        Debug.DrawLine(flyTransform.position, flyTransform.TransformPoint(Vector3.forward * 500), Color.green);
    }

    public override void CheckIsLive()
    {
        if (hitUnit == true || currentFlyDistance < 0)
        {
            SendMessageUpwards("PostEffect");
            currentFlyDistance = 0;
        }
    }

    public override void Hit()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, layerMask);

        foreach (Collider collider in hitColliders)
        {
            ModulBasys modulBasys = collider.GetComponent<ModulBasys>();

            if (modulBasys != null)
            {
                float distance = Vector3.Distance(flyTransform.position, modulBasys.transform.position);
                float power = Random.Range(powerMin, powerMax + 1);
                float dmg = damage;
                power -= distance;
                dmg -= distance;
                modulBasys.GetDamage(Mathf.Clamp(dmg, 0, 100), Mathf.Clamp(power , 0, 100) , showFlyHit);
            }
        }
    }
}
