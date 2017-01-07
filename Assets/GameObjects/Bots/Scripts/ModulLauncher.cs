using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModulLauncher : ModulBasys
{
    public int battary = 1;
    public int maxAmmo = 4;//Максимальное количество снарядов в магазине
    public float damage = 1;//Сила повреждений
    public float powerMin = 2;//Сила пробития минимальная
    public float powerMax = 4;//Сила пробития максимальная
    public float correct = 1;//Разброс
    
    public bool startReload = false;
    //[HideInInspector]
    public int ammo = 0;//Текущее количество снарядов в магазине
    public LayerMask layerMask;
    public Vector3[] rocketPosition;
    public List<ProjectileController> rocketArray = new List<ProjectileController>(1);

    private AudioClip shootClip;//Звук выстрела
    private AudioSource audioSorce;//Источние звука (компонент находится на коренном обьекте)

    private Transform parentTransform;
    private Ray rayCollision;
    private RaycastHit hitCollision;

    private float energy;
    public float EnergyValue
    {
        get { return energy; }

        set
        {
            energy = Mathf.Clamp(value, 0, energyMaxValue);
        }
    }

    void OnEnable()
    {
        base.OnEnable();

        //Заргужает звук выстрела
        if (shootClip == null)
        {
            shootClip = Resources.Load("Sound/Shoot4") as AudioClip;
        }
        //Находит компонент источник звука
        if (audioSorce == null)
        {
            audioSorce = GetComponentInParent<AudioSource>();
        }

        energyMaxValue = battary * 1000;
        EnergyValue = battary * 1000;
        
        parentTransform = transform.parent;
        InstanceRocket();
        ammo = rocketArray.Count;//Полный магазин
    }

    void InstanceRocket()
    {
        for (int i = 0; i < rocketPosition.Length; i++)
        {
            GameObject tmpObj = Instantiate(Resources.Load("prefabs/Weapon/ShellRocket")) as GameObject;
            tmpObj.transform.SetParent(transform);
            tmpObj.transform.localPosition = rocketPosition[i];
            rocketArray.Add(tmpObj.GetComponent<ProjectileController>());
        }
    }

    void Update()
    {
        if (modulStatus == ModulStatus.On)
        {
            //Выполняем перезарядку
            if (startReload == true)
            {
                Reload();
                startReload = false;
            }
        }
    }

    public override ModulLauncher GetLauncherController()
    {
        return this;
    }

    public override Vector3 GetGunTarget()
    {
        rayCollision = new Ray(parentTransform.position, parentTransform.forward * GameController.distToHit);
        //Debug.DrawRay(pointShoot, transform.forward * 500f, Color.blue);

        if (Physics.Raycast(rayCollision, out hitCollision, GameController.distToHit, layerMask))
        {
            return hitCollision.point;
            //supportMessage = hitCollision.collider.name;
        }
        else
        {
            return parentTransform.TransformPoint(Vector3.forward * GameController.distToHit);
            //supportMessage = "No Collision";
        }
    }

    public override void Shoot(bool showFlyHit)//Выстрел
    {
        if (ammo > 0 && EnergyValue >= energyMinToAction)
        {
            if (rocketArray[ammo - 1].readyToShoot == true)
            {
                rocketArray[ammo - 1].Shoot(parentTransform.TransformPoint(Vector3.forward * 500), showFlyHit);
                ammo -= 1;//Уменьшам количество патронов в магазине
                ActionModul(energyMinToAction);
                return;
            }
        }
    }

    void Reload()//Выполняем перезарядку
    {
        //-=перезарядка=-//
        foreach (ProjectileController controller in rocketArray)
        {
            if (controller.readyToReload == true)
            {
                controller.Reload();
                ammo++;
            }
        }
    }

    public override void ReloadWeapon()
    {
        startReload = true;
    }

    public override void ActionModul(float clearEnergy)
    {
        base.ActionModul(clearEnergy);
        EnergyValue -= clearEnergy;
    }

    public override float ReloadEnergy(float reloadEnergy)
    {
        if (EnergyValue < energyMaxValue)
        {
            EnergyPower = (reloadEnergy / 100) * energyReloadQuoue;
            EnergyValue += EnergyPower;
            return EnergyPower;
        }

        return 0;
    }

    public override bool GetDamage(float damage, float power, bool showFlyHit)
    {
        base.GetDamage(damage, power, showFlyHit);
        return false;
    }
}
