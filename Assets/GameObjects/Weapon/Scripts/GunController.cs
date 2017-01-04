using UnityEngine;
using System.Collections;

public enum ModulPosition
{
    Left,
    Right,
    Main,
}

public class GunController : MonoBehaviour 
{
    public BotController botController;
    public ModulPosition gunPisition;
    public bool weaponIsOn;
    public float speedRotateGun = 100;//сглаживание(скорость) вращения башни
    public float maximumAngleGun = 25;//ограничение поворота орудия
    public float minimumAngleGun = 25;//ограничение поворота орудия
    public float rangeAngleGun = 25;//ограничение по горизонтали

    public int maxAmmo = 100;//Максимальное количество снарядов в магазине
    public int ammo;//Текущее количество снарядов в магазине
    public float timeReload = 2;//Время перезарядка каждого снаряда
    private float currentTimeReload;//Текущее время перезарядка каждого снаряда
    public float damage = 1;//Сила повреждений
    public float powerMin = 2;//Сила пробития минимальная
    public float powerMax = 3;//Сила пробития максимальная

    public AudioClip shootClip;//Звук выстрела
    public AudioSource audioSorce;//Источние звука (компонент находится на коренном обьекте)
    public GameObject muzzleFire;//Вспышка выстрела
    public GameObject bullet;//Пуля

    public Vector3 currentGunTarget;

    private float correct = 3;

    void OnEnable()
    {
        weaponIsOn = true;
        ammo = maxAmmo;//Полный магазин
        botController = GetComponentInParent<BotController>();
        ListAdd();//Добавляем данный класс в список классов, находящийся в классе  botController

        //Заргужает обьект вспышки выстрела
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
        if (bullet == null)
        {
            bullet = Resources.Load("Prefabs/Weapon/Bullet") as GameObject;
        }

        //Debug
        damage = Random.Range(3, 5);
        

        /*
        foreach (Transform childTransform in GetComponentsInChildren<Transform>())
        {
            if (childTransform.name == "PointShoot")
            {
                pointShoot = childTransform.gameObject;
            }
        }
        */
        //if (muzzleFire != null) { muzzleFire.SetActive(true); }
    }

    void OnDisable()
    {
        ListClear();//Удаляемм данный класс из списка классов, находящийся в классе  botController
    }

    void Update()
    {
        //Debug.DrawLine(transform.position, currentGunTarget, Color.black);
        //Выполняем перезарядку
        if (botController != null)
        {
            if (botController.botState != SM_BotState.Dead)
            {
                if (botController.botState == SM_BotState.AiControl || botController.botState == SM_BotState.PlayerControl)
                {
                    Reload();
                }
            }
        }
    }

    void Reload()//Выполняем перезарядку
    {
        //-=перезарядка=-//
        if (currentTimeReload < timeReload)
        {
            if (ammo > 0)//Если есть ещё патроны в магозине
            {
                currentTimeReload += Time.deltaTime;
            }
        }

        if (currentTimeReload > timeReload)
        {
            currentTimeReload = timeReload;
        }
    }

    public void Shoot()//Выстрел
    {
        if (botController != null)// && currentTimeReload != null)
        {
            if (currentTimeReload == timeReload && ammo > 0)
            {
                currentTimeReload = 0;//Сбрасываем текущее время перезарядки
                ammo -= 1;//Уменьшам количество патронов в магазине

                //Контрольная проверка, что бы значение не было ниже нуля
                if (ammo < 0) 
                { 
                    ammo = 0; 
                }

                CorrectShoot();

                GameObject newBullet = Instantiate(bullet, muzzleFire.transform.position, transform.rotation) as GameObject;//Создаем пулю
                //Удочеряем пулю к вспышке выстрела, что бы пуля могла получит ссылки на нужные её классы
                newBullet.transform.parent = muzzleFire.transform;
                //newBullet.transform.rotation = transform.parent.rotation;
                
                //-=Вспышка выстрела=-// проигрывается при включении, потому сперва выключаем, потом включаем
                muzzleFire.SetActive(false);
                muzzleFire.SetActive(true);
                //Проигрывает звук. Меняем значение pitch что бы звуки немного отличались друг от друга
                audioSorce.pitch = Random.Range(0.9f, 1.1f);
                audioSorce.PlayOneShot(shootClip);
            }
        }
    }

    void ListAdd()//Добавляем данный класс в список классов, находящийся в классе  botController
    {
        for (int i=0; i<botController.gunController.Count; i++ )
        {
            if (botController.gunController[i] == this)
            {
                return;
            }
        }

        botController.gunController.Add(this); 
    }

    void ListClear()//Удаляемм данный класс из списка классов, находящийся в классе  botController
    {
        for (int i = 0; i < botController.gunController.Count; i++)
        {
            if (botController.gunController[i] == this)
            {
                botController.gunController.RemoveAt(i);
            }
        }
    }

    void CorrectShoot()
    {
        float correctX = transform.parent.localEulerAngles.x + Random.Range(-correct, correct);
        float correctY = transform.parent.localEulerAngles.y + Random.Range(-correct, correct);
        transform.parent.localEulerAngles = new Vector3(correctX, correctY, 0);
    }
}
