using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_LauncherReloadController : UI_BotInterfaceBasys
{
    public ModulType modulType;
    public Text nameWeaponText;
    public Text reloadTimeText;
    public Slider reloadSlider;

    public Color readyColor;

    private ModulLauncher modul;
    private int botID;
    public Image[] rocketArray;

    void OnEnable()
    {
        selfRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (GetBotID() == true)
        {
            if (modul == null)
            {
                currentPosition = MenuPosition.Disable;
            }
            else
            {
                CheckMenuState(modul.modulStatus);
                CheckRocketState();
            }
        }

        CheckMenuPosition();
    }

    private bool GetBotID()
    {
        if (botID != PlayerController.botController.gameObject.GetInstanceID())
        {
            botID = PlayerController.botController.gameObject.GetInstanceID();
            modul = FindModul();
            StartCheckRocket();
            return false;
        }

        return true;
    }

    private ModulLauncher FindModul()
    {
        foreach (ModulBasys modul in PlayerController.botController.modulController)
        {
            if (modul.modulType == modulType)
            {
                //nameInterface = modul.information[0];
                //nameWeaponText.text = nameInterface;
                return modul.GetLauncherController();
            }
        }

        return null;
    }

    void StartCheckRocket()
    {
        for (int i = 0; i < rocketArray.Length; i++)
        {
            if (i <= modul.maxAmmo -1 )
            {
                rocketArray[i].enabled = true;
            }
            else
            {
                rocketArray[i].enabled = false;
            }
        }
    }



    void CheckRocketState()
    {
        for (int i = 0; i < rocketArray.Length; i++)
        {
            if (rocketArray[i] != null)
            {
                if (modul.modulStatus == ModulStatus.On)
                {
                    if (i <= modul.ammo - 1)
                    {
                        rocketArray[i].color = readyColor;
                    }
                    else
                    {
                        rocketArray[i].color = disableColor;
                    }
                }
                else
                {
                    rocketArray[i].color = disableColor;
                }
            }
        }
    }

    public override void CheckMenuState(ModulStatus modulStatus)
    {

        base.CheckMenuState(modulStatus);

        reloadTimeText.color = currentColor;
        nameWeaponText.color = currentColor;
        reloadSlider.maxValue = modul.energyMaxValue;
        reloadSlider.value = modul.energyValue;
        reloadTimeText.text = modul.energyValue.ToString("f0");
    }

    public override void CheckMenuPosition()
    {
        if (modul != null)
        {
            if (modul.modulStatus == ModulStatus.On)
            {
                currentPosition = MenuPosition.Show;
            }
            else
            {
                currentPosition = MenuPosition.Hide;
            }
        }
        else
        {
            currentPosition = MenuPosition.Disable;
        }

        base.CheckMenuPosition();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        modul.AboutThis();
    }
}
