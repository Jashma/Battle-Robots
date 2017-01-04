using UnityEngine;
using System.Collections;

public class ModulPelvic :  ModulBasys
{
    public float speedMove;
    public float currentSpeedMove;

    private BotController botController;
    private Animator animator;
    private Transform moveTransform;
    private Vector3 oldPosition;

    void OnEnable()
    {
        base.OnEnable();
        botController = GetComponentInParent<BotController>();
        animator = GetComponentInParent<Animator>();
        moveTransform = botController.transform;
    }

    void Update()
    {
        if (modulStatus == ModulStatus.On)
        {
            CheckMove();
        }
    }

    float SpeedMove()
    {
        return EnergyPower * 0.1f;
    }

    private void CheckMove()
    {
        animator.speed = SpeedMove();
        botController.speedRotate = SpeedMove() * 50;
        speedMove = animator.speed;

        if (moveTransform.position != oldPosition)
        {
            oldPosition = moveTransform.position;
        }
    }

    public override ModulPelvic GetModulPelvic()
    {
        return this;
    }

    public override bool GetDamage(float damage, float power, bool showFlyHit)
    {
        base.GetDamage(damage, power, showFlyHit);
        return false;
    }

    public override float ReloadEnergy(float reloadEnergy)
    {
        EnergyPower = (reloadEnergy / 100) * energyReloadQuoue;

        //energyPower = Mathf.Clamp(energyPower, 0, energyMaxValue);

        return EnergyPower;
    }
}
