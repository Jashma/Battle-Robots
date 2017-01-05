using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModulGun : ModulBasys 
{
    public int maxAmmo = 100;//Максимальное количество снарядов в магазине
    public float damage = 1;//Сила повреждений
    public float powerMin = 2;//Сила пробития минимальная
    public float powerMax = 4;//Сила пробития максимальная
    public float correct = 1;//Разброс
    public int ammo;//Текущее количество снарядов в магазине
    public float energyValue;//Запас энергии с которой вылетает снаряд
    public LayerMask layerMask;

    private AudioClip shootClip;//Звук выстрела
    private AudioSource audioSorce;//Источние звука (компонент находится на коренном обьекте)
    private GameObject muzzleFire;//Вспышка выстрела
    private List<ProjectileController> bulletArray = new List<ProjectileController>(1);
    private Ray rayCollision;
    private RaycastHit hitCollision;
    private Transform parentTransform;

    void OnEnable()
    {
        base.OnEnable();
        ammo = maxAmmo;//Полный магазин

        //Загружает обьект вспышки выстрела
        if (muzzleFire == null) 
        {
            muzzleFire = transform.FindChild("MuzzleFire").gameObject;
        }
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

        //Заргужает обьект пулю
        if (maxAmmo > 0)
        {
            InstanceBullet();
        }

        parentTransform = transform.parent;

        //Debug
        damage = Random.Range(4, 8);
    }

    public override Vector3 GetGunTarget()
    {
        rayCollision = new Ray(parentTransform.position, parentTransform.forward * GameController.distToHit);

        if (Physics.Raycast(rayCollision, out hitCollision, GameController.distToHit, layerMask))
        {
            return hitCollision.point;
        }
        else
        {
            return parentTransform.TransformPoint(Vector3.forward * GameController.distToHit);
        }
    }

    public override ModulGun GetGunController()
    {
        return this;
    }

    public override void Shoot(bool showFlyHit)//Выстрел
    {
        if (ammo > 0)
        {
            for (int i = 0; i < bulletArray.Count; i++)
            {
                if (bulletArray[i].readyToReload == true) { bulletArray[i].Reload(); }

                if (bulletArray[i].readyToShoot == true)
                {
                    bulletArray[i].projectilBasys.damage = damage;
                    bulletArray[i].projectilBasys.powerMin = powerMin;
                    bulletArray[i].projectilBasys.powerMax = powerMax;
                    bulletArray[i].Shoot(GetGunTarget(), showFlyHit, energyValue);

                    //-=Вспышка выстрела=-// проигрывается при включении, потому сперва выключаем, потом включаем
                    muzzleFire.SetActive(false);
                    muzzleFire.SetActive(true);

                    //Проигрывает звук. Меняем значение pitch что бы звуки немного отличались друг от друга
                    audioSorce.pitch = Random.Range(0.9f, 1.1f);
                    audioSorce.PlayOneShot(shootClip);

                    ammo -= 1;//Уменьшам количество патронов в магазине

                    return;
                }
            }

            if (bulletArray.Count < 10)
            {
                Debug.Log("Instance new bullet");
                InstanceBullet();//Заргужает обьект пулю
            }
            else
            {
                Debug.Log("To mach bullet");
            }
        }
    }

    public override bool GetDamage(float damage, float power, bool showFlyHit)
    {
        base.GetDamage(damage, power, showFlyHit);
        return false;
    }

    void InstanceBullet()//Заргужает обьект пулю
    {
        GameObject tmpObj = Instantiate(Resources.Load("Prefabs/Weapon/ShellBullet") as GameObject);
        tmpObj.transform.SetParent(muzzleFire.transform);
        tmpObj.transform.localPosition = Vector3.zero;
        tmpObj.transform.localEulerAngles = Vector3.zero;
        bulletArray.Add(tmpObj.GetComponent<ProjectileController>());
    }

    void CorrectShoot()
    {
        float correctX = transform.parent.localEulerAngles.x + Random.Range(-correct, correct);
        float correctY = transform.parent.localEulerAngles.y + Random.Range(-correct, correct);
        transform.parent.localEulerAngles = new Vector3(correctX, correctY, 0);
    }

    public override string GetSupportMessage()
    {
        if (hitCollision.collider != null)
        {
            return hitCollision.collider.name;
        }
        else
        {
            return "";
        }
    }
}
