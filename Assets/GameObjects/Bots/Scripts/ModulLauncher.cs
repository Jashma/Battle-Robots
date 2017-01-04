using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModulLauncher : ModulBasys
{
    public int maxAmmo = 4;//Максимальное количество снарядов в магазине
    public float damage = 1;//Сила повреждений
    public float powerMin = 2;//Сила пробития минимальная
    public float powerMax = 4;//Сила пробития максимальная
    public float correct = 1;//Разброс
    public float energyValue;
    public bool startReload = false;
    //[HideInInspector]
    public int ammo = 0;//Текущее количество снарядов в магазине
    public LayerMask layerMask;
    [HideInInspector]
    public Vector3[] rocketPosition;
    public List<ProjectileController> rocketArray = new List<ProjectileController>(1);

    private AudioClip shootClip;//Звук выстрела
    private AudioSource audioSorce;//Источние звука (компонент находится на коренном обьекте)

    private Transform parentTransform;
    private Ray rayCollision;
    private RaycastHit hitCollision;

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

        parentTransform = transform.parent;
        FindAllRocket();
        //maxAmmo = rocketArray.Count;
        ammo = rocketArray.Count;//Полный магазин
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
        if (ammo > 0 && energyValue >= energyMinToAction)
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
        //if (ammo <= 0 && maxAmmo > ammo)
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
    }

    void FindAllRocket()
    {
        foreach (Transform nextTransform in GetComponentsInChildren<Transform>())
        {
            if (nextTransform.name == "ShellRocket")
            {
                rocketArray.Add(nextTransform.GetComponentInChildren<ProjectileController>());
            }

        }
    }

    public override bool ActionModul(float clearEnergy)
    {
        if (energyValue >= clearEnergy)
        {
            energyValue -= clearEnergy;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ReloadWeapon()
    {
        startReload = true;
    }

    public override float ReloadEnergy(float reloadEnergy)
    {
        /*
        if (energyValue < energyMaxValue)
        {
            reloadEnergy = (reloadEnergy / 100) * energyReloadQuoue;

            energyValue = energyValue + reloadEnergy;

            if (energyValue > energyMaxValue) { energyValue = energyMaxValue; }
            return reloadEnergy;
        }
        */
        return 0;
    }

    public override bool GetDamage(float damage, float power, bool showFlyHit)
    {
        base.GetDamage(damage, power, showFlyHit);
        return false;
    }
}
