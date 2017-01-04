using UnityEngine;
using System.Collections;

public class ModulGunController : ModulBasys
{
    public float speedRotateGun = 100;//сглаживание(скорость) вращения башни
    public rotateAxis axis;
    public float maximumAngleGun = 25;//ограничение поворота орудия
    public float minimumAngleGun = 25;//ограничение поворота орудия
    public float rangeAngleGun = 25;//ограничение по горизонтали
    private float tempAngleGunX = 0;
    private float tempAngleGunY = 0;
    private Quaternion directionGun;
    private Vector3 gunTarget;
    private float oldRotationY;

    void OnEnable()
    {
        base.OnEnable();
        gunTarget = transform.TransformPoint(Vector3.forward * 500);
        thisTransform = transform;
    }

    void LateUpdate()
    {
        if (modulStatus == ModulStatus.On)
        {
            GetDirection();
            Rotation();
        }
    }

    public override void SetLookTarget(Vector3 pointTarget)
    {
        gunTarget = pointTarget;
    }

    void GetDirection()
    {
        //вектор, куда нужно вращать башню
        directionGun = Quaternion.LookRotation(gunTarget - thisTransform.position);
    }

    float SpeedRotation()
    {
        return (EnergyPower * speedRotateGun) * Time.deltaTime;
    }

    void Rotation()
    {
        thisTransform.rotation = Quaternion.RotateTowards(thisTransform.rotation, directionGun, SpeedRotation());

        tempAngleGunX = 0;
        tempAngleGunY = 0;
        //
        if (axis == rotateAxis.AxisX || axis == rotateAxis.AxisXY)
        {
            tempAngleGunX = thisTransform.localEulerAngles.x;
            if (thisTransform.localEulerAngles.x > 180f)
            {
                //если значение Х больше 180, то используем ограничение между 360 и максимальним ограничением
                tempAngleGunX = Mathf.Clamp(tempAngleGunX, 360f - maximumAngleGun, 360f);
            }
            else
            {
                if (thisTransform.localEulerAngles.x < 180f)
                {
                    //если значение Х меньше 180, то ограничение используем между 0 и минимальным ограничением
                    tempAngleGunX = Mathf.Clamp(tempAngleGunX, 0f, minimumAngleGun);
                }
            }
        }
        //
        if (axis == rotateAxis.AxisY || axis == rotateAxis.AxisXY)
        {
            tempAngleGunY = thisTransform.localEulerAngles.y;
            if (thisTransform.localEulerAngles.y > 180f)
            {
                //если значение Y больше 180, то используем ограничение между 360 и максимальним ограничением
                tempAngleGunY = Mathf.Clamp(tempAngleGunY, 360f - rangeAngleGun, 360f);
            }
            else
            {
                if (thisTransform.localEulerAngles.y < 180f)
                {
                    //если значение Y меньше 180, то ограничение используем между 0 и минимальным ограничением
                    tempAngleGunY = Mathf.Clamp(tempAngleGunY, 0f, rangeAngleGun);
                }
            }
        }

        //присваиваем орудию новое значение Х и Y
        thisTransform.localEulerAngles = new Vector3(tempAngleGunX, tempAngleGunY, 0);

        if (oldRotationY != Mathf.RoundToInt(thisTransform.localEulerAngles.y))
        {
            oldRotationY = Mathf.RoundToInt(thisTransform.localEulerAngles.y);
        }
    }

    public override bool GetDamage(float damage, float power, bool showFlyHit)
    {
        base.GetDamage(damage, power, showFlyHit);
        return false;
    }

    public override float ReloadEnergy(float reloadEnergy)
    {
        EnergyPower = (reloadEnergy / 100) * energyReloadQuoue;

        return EnergyPower;
    }
}
