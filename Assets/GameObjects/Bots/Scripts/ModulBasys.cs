using UnityEngine;
using System.Collections;

public enum ModulType
{
    None,
    Radar,
    Optic,
    Reactor,
    Body,
    LLauncherController,
    RLauncherController,
    Pelvic,
    LSustav,
    LBedro,
    LGolen,
    RSustav,
    RBedro,
    RGolen,
    FreeObj,
    Armore,
    ArrayLeight,
    LGunController,
    RGunController,
    LGun,
    RGun,
    LLauncher,
    RLauncher,
}


public enum ModulStatus
{
    None,
    On,
    Off,
}

public enum rotateAxis
{
    AxisX, AxisY, AxisXY, None
}

public abstract class ModulBasys : MonoBehaviour
{
    public ModulType modulType;
    public ModulStatus modulStatus = ModulStatus.None;
    public float healthModul;
    public float armoreModul;
    public string[] information;
    public int coast = 0;
    public float energyMaxValue = 100;//Максимальное количество энергии
    public float energyMinToAction = 10;//Минимальное количество энергии, необходимое для действия 
    public float energyReloadQuoue = 10;//Процент потребления энергии
    
    public GameObject smokeObj;
    public GameObject sparkleObj;
    public GameObject[] modulAddOn = new GameObject[0];
    public Transform thisTransform;
    public ModulReactor modulReactor;

    private float energyPower;//Текущее количество энергии
    public float EnergyPower
    {
        get { return energyPower; }

        set
        {
            energyPower =  Mathf.Clamp(value, 0, energyMaxValue);
        }
    }

    public void OnEnable()
    {
        healthModul = 100;
        modulStatus = ModulStatus.Off;
        thisTransform = GetComponent<Transform>();
        if (information.Length == 0)
        {
            information = new string[10];
            information[0] = "Name modul " + gameObject.name;
            information[1] = "About modul ";
            information[2] = "State modul " + modulStatus.ToString();
            information[3] = "Health modul " + healthModul.ToString();
            information[4] = "Armore modul " + armoreModul.ToString();
        }
    }

    virtual public bool GetDamage(float damage, float power, bool showFlyHit)
    {
        if (showFlyHit == true)
        {
            ShowDebugText(transform.position + Vector3.up * 2, "", " / " + damage.ToString("f1") + " / " + power.ToString("f1") + " / " + healthModul.ToString("f1"), 1);
        }

        if (power > armoreModul)
        {
            healthModul -= damage;
        }

        if (healthModul < 50 && smokeObj == null)
        {
            smokeObj = Instantiate(Resources.Load("Prefabs/Effects/SmokeLight") as GameObject);
            smokeObj.transform.parent = this.transform;
            smokeObj.transform.localPosition = Vector3.zero;

            sparkleObj = Instantiate(Resources.Load("Prefabs/Effects/Sparkle") as GameObject);
            sparkleObj.transform.parent = this.transform;
            sparkleObj.transform.localPosition = Vector3.zero;
        }


        if (healthModul < 0) { healthModul = 0; modulStatus = ModulStatus.Off; }
        return false;
    }

    virtual public ModulRadar GetRadarController()
    {
        return null;
    }

    virtual public void startReactor(ModulBasys[] modulController)
    {
    }

    virtual public Modul GetModulController()
    {
        return null;
    }

    virtual public ModulReactor GetModulReactor()
    {
        return null;
    }

    virtual public ModulPelvic GetModulPelvic()
    {
        return null;
    }

    virtual public ModulBody GetBodyModul()
    {
        return null;
    }

    virtual public ModulGun GetGunController()
    {
        return null;
    }

    virtual public ModulLauncher GetLauncherController()
    {
        return null;
    }

    virtual public Vector3 GetGunTarget()
    {
        return Vector3.zero;
    }

    virtual public ModulReactor GetReactorModul()
    {
        return null;
    }

    virtual public void SetLookTarget(Vector3 pointTarget)
    {
    }

    virtual public void Shoot(bool showFlyHit)
    {
    }

    virtual public void AboutThis()
    {
        if (UI_MouseInformation.Instance != null)
        {
            UI_MouseInformation.Instance.SetMessage(information);
        }
        else
        {
            //Debug.Log("UI_MouseInformation is disable");
        }
    }

    virtual public int GetNumberModul()
    {
        return 0;
    }

    virtual public void ActionModul(float clearEnergy)
    {
        EnergyPower -= clearEnergy;
    }

    virtual public float ReloadEnergy(float reloadEnergy)
    {
        return 0;
    }

    virtual public void ReloadWeapon()
    {

    }

    virtual public void ShowUIInformation()
    {

    }

    void OnDisable()
    {
        modulStatus = ModulStatus.None;
    }

    virtual public string GetSupportMessage()
    {
        return "";
    }

    virtual public void ShowDebugText(Vector3 createPosition, string message1, string message2, int alignment = 1)
    {
        GameObject debugText = Instantiate(Resources.Load("Prefabs/User Interface/DebugText"), createPosition, Quaternion.identity) as GameObject;
        debugText.GetComponent<UI_DebugText>().spacePosition = createPosition;
        debugText.GetComponent<UI_DebugText>().alignment = alignment;
        debugText.GetComponent<UI_DebugText>().debugMessage1 = message1;
        debugText.GetComponent<UI_DebugText>().debugMessage2 = message2;
    }
}
