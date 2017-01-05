using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModulReactor : ModulBasys
{
    public float powerDefault = 100;
    private BotController botController;

    //Debug
    public List<ModulBasys> modulBasys = new List<ModulBasys>();

    void OnEnable()
    {
        base.OnEnable();
        botController = GetComponentInParent<BotController>();
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
            
        }
    }

    public override ModulReactor GetModulReactor()
    {
        return this;
    }
}
