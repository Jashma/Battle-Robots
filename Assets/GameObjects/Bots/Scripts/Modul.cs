using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Modul : ModulBasys
{
    void OnEnable()
    {
        base.OnEnable();
    }

    public override Modul GetModulController()
    {
        return this;
    }

    public override bool GetDamage(float damage, float power, bool showFlyHit)
    {
        base.GetDamage(damage, power, showFlyHit);
        return false;
    }

    //Debug
    void Update()
    {
        if (GetComponentInChildren<MessageOnHead>() != null)
        {
            if (GetComponentInChildren<MessageOnHead>().showMessageOnHead == true)
            {
                GetComponentInChildren<MessageOnHead>().value = healthModul;
            }
        }
    }
    //End Debug
}
