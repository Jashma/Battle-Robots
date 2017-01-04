using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_GunReloadController : UI_BotInterfaceBasys
{
    public ModulType modulType;
    public Text nameWeaponText;
    public Text ammoText;
    public Slider ammoSlider;
    public Text reloadTimeText;
    public Slider reloadSlider;

    public ModulGun modul;
    private int botID;

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
                modul = FindModul();
                currentPosition = MenuPosition.Disable;
            }
            else
            {
                CheckMenuState(modul.modulStatus);
            }
        }

        CheckMenuPosition();
    }

    private bool GetBotID()
    {
        if (botID != PlayerController.botController.gameObject.GetInstanceID())
        {
            botID = PlayerController.botController.gameObject.GetInstanceID();
            return false;
        }

        return true;
    }

    private ModulGun FindModul()
    {
        foreach (ModulBasys modul in PlayerController.botController.modulController)
        {
            if (modul.modulType == modulType)
            {
                return modul.GetGunController();
            }
        }

        return null;
    }

    public override void CheckMenuState(ModulStatus modulStatus)
    {
        base.CheckMenuState(modulStatus);

        reloadTimeText.color = currentColor;
        nameWeaponText.color = currentColor;
        nameWeaponText.text = modul.information[0];
        ammoText.color = enableColor;
        reloadSlider.maxValue = modul.energyMaxValue;
        reloadSlider.value = modul.energyValue;
        reloadTimeText.text = modul.energyValue.ToString("f0");
        ammoSlider.maxValue = modul.maxAmmo;
        ammoSlider.value = modul.ammo;
        ammoText.text = modul.ammo.ToString("f0");
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
