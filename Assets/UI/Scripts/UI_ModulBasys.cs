using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class UI_ModulBasys : MonoBehaviour
{
    private int botID;
    public ModulType modulType;
    public string message;
    public Text modulText;
    //public bool showTextInformation;
    public Image image;
    public ModulBasys modulController;

    virtual public bool GetBotID()
    {
        if (botID != PlayerController.botController.gameObject.GetInstanceID())
        {
            botID = PlayerController.botController.gameObject.GetInstanceID();
            return false;
        }

        return true;
    }

    virtual public ModulBasys FindModul()
    {
        foreach (ModulBasys modul in PlayerController.botController.modulController)
        {
            if (modul.modulType == modulType)
            {
                return modul;
            }
        }
        return null;
    }

    virtual public void SetText(string newText)
    {
        if (modulText.text != newText)
        {
            modulText.text = newText;
        }
    }

    virtual public bool CheckModulState(ModulBasys modul)
    {
        if (modul.modulStatus == ModulStatus.On)
        {
            return true;
        }

        return false;
    }

    virtual public Color ColorByValue(float value)
    {
        value = value * 0.01f;
        return new Color(1 - value, value, 0, 0.5f);
    }

    virtual public float AlfaByValue(float value)
    {
        return value * 0.01f;
    }
}
