using UnityEngine;
using System.Collections;

public class ModulBody : ModulBasys
{
    public float speedRotateDefault = 3;//Скорость вращения
    public float speedRotateCurrent;
    public float limitRotationAngle = 0;//Ограничение вращения

    [HideInInspector]public Transform rotateTransform;
    [HideInInspector]public Vector3 bodyTarget;
    private BotController botController;
    private Quaternion directionBody;//вектор, куда нужно вращать башню
    private float oldRotationY;

    void OnEnable()
    {
        base.OnEnable();
        botController = GetComponentInParent<BotController>();
        botController.pointSniper = transform.FindChild("PointSniper");
        FindBodyTarget();
    }

    void FindBodyTarget()
    {
        foreach (Transform findTransform in botController.GetComponentsInChildren<Transform>())
        {
            if (findTransform.name == "BodyController")
            {
                rotateTransform = findTransform;
                return;
            }
        }
    }

    public override bool GetDamage(float damage, float power, bool showFlyHit)
    {
        base.GetDamage(damage, power, showFlyHit);
        return false;
    }

    void Update()
    {
        if (modulStatus == ModulStatus.On)
        {
            GetDirection();
            RotateBody();
        }
    }

    public override ModulBody GetBodyModul()
    {
        return this;
    }

    public override void SetLookTarget(Vector3 pointTarget)
    {
        bodyTarget = pointTarget;
    }

    void GetDirection()
    {
        directionBody = Quaternion.LookRotation(bodyTarget - rotateTransform.position);
    }

    float SpeedRotation()
    {
        speedRotateCurrent = (EnergyPower * speedRotateDefault) * Time.deltaTime;
        return (EnergyPower * speedRotateDefault) * Time.deltaTime;
    }

    /*
    void ClampRotation()
    {
        if (limitRotationAngle <= 0)
        {
            //если угол между башней и корпусом меньше ограничения угла
            if (Vector3.Angle(rotateTransform.forward, botController.transform.forward) < limitRotationAngle)
            {
                RotateBody();
            }
            else
            {//Если угол больше ограничения но угол между векторами камеры и танка меньше ограничения
                if (Vector3.Angle((bodyTarget - botController.transform.position), botController.transform.forward) < limitRotationAngle)
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
    */

    void RotateBody()
    {
        rotateTransform.rotation = Quaternion.RotateTowards(rotateTransform.rotation, directionBody, SpeedRotation());
        rotateTransform.localEulerAngles = new Vector3(0, rotateTransform.localEulerAngles.y, 0);

        if (oldRotationY != Mathf.RoundToInt(rotateTransform.localEulerAngles.y))
        {
            oldRotationY = Mathf.RoundToInt(rotateTransform.localEulerAngles.y);
            ActionModul(EnergyPower);
            
        }

        //Debug.DrawLine(botController.transform.position, transform.TransformPoint(Vector3.forward * 100), Color.yellow);
        //Debug.DrawLine(transform.position, bodyTarget, Color.red);
        //float tmpDist = Vector3.Distance(parentTransform.transform.position, bodyTarget);
        //Debug.DrawLine(parentTransform.transform.position, parentTransform.transform.TransformPoint(Vector3.forward * tmpDist), Color.green);

    }

    public override float ReloadEnergy(float reloadEnergy)
    {
        if (EnergyPower < (reloadEnergy / 100) * energyReloadQuoue)
        {
            EnergyPower = (reloadEnergy / 100) * energyReloadQuoue;
            return EnergyPower;
        }

        return 0;
    }
}
