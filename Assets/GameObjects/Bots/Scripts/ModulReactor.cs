using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModulReactor : ModulBasys
{
    public float power = 100;
    public float energyPowerCurrent;
    private BotController botController;
    private int valueUseEnergyModul;

    //Debug
    public List<ModulBasys> modulBasys = new List<ModulBasys>();

    void OnEnable()
    {
        base.OnEnable();
        botController = GetComponentInParent<BotController>();
    }

    public override void startReactor(ModulBasys[] modulController)
    {
        valueUseEnergyModul = 0;
        modulBasys.Clear();

        foreach (ModulBasys modul in modulController)
        {
            modul.modulReactor = this;

            if (modul.energyMinToAction > 0)
            {
                valueUseEnergyModul++;
                modulBasys.Add(modul);
            }
        }
    }

    public override ModulReactor GetReactorModul()
    {
        return this;
    }

    public override bool GetDamage(float damage, float power, bool showFlyHit)
    {
        base.GetDamage(damage, power, showFlyHit);
        return false;
    }

    void LateUpdate()
    {
        if (modulStatus == ModulStatus.On)
        {
            ReloadAllEnergy();
        }
    }

    public override ModulReactor GetModulReactor()
    {
        return this;
    }

    public override float ReloadEnergy(float reloadEnergy)
    {
        EnergyPower = (reloadEnergy / 100) * energyReloadQuoue;

        return EnergyPower;
    }

    private void ReloadAllEnergy()
    {
        energyPowerCurrent = power;

        foreach (ModulBasys modul in botController.modulController)
        {
            energyPowerCurrent -= modul.ReloadEnergy(energyPowerCurrent);
        }
    }
}
