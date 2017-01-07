using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModulReactor : ModulBasys
{
    private BotController botController;
    private int valueUseEnergyModul;
    public float lastEnergy;
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
            ReloadEnergy();
            ReloadAllEnergy();
        }
    }

    public override ModulReactor GetModulReactor()
    {
        return this;
    }

    private void ReloadEnergy()
    {
        lastEnergy = EnergyPower;
        EnergyPower = energyMaxValue;
    }

    private void ReloadAllEnergy()
    {
        foreach (ModulBasys modul in botController.modulController)
        {
            if (modul.modulStatus == ModulStatus.On)
            {
                EnergyPower -= modul.ReloadEnergy(EnergyPower);
            }
        }
    }
}
