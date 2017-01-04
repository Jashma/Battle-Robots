using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_Modul : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector] public UnityEvent changeModulEvent;
    public ModulType modulType;
    public Text modulText;
    private Image image;
    private int botID;
    public ModulBasys modulController;
    public bool showTextInformation;
    private string message;
    //Debug
    public float health;

    void OnEnable()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
            modulText = GetComponentInChildren<Text>();
            changeModulEvent = new UnityEvent();
        }

        if (modulText == null)
        {
            showTextInformation = false;
        }
    }

    void Update()
    {
        if (GetBotID() == true)
        {
            if (modulController == null)
            {
                modulController = FindModul();
                message = "No Modul";
                image.color = UI_Controller.disableColor;
            }
            else
            {
                if (health != modulController.healthModul * 0.01f)
                {
                    health = modulController.healthModul * 0.01f;
                    image.color = new Color(1 - health, health, 0, 0.5f);
                }

                if (health == 0)
                {
                    image.color = new Color(1 - health, health, 0, 0.5f);
                }

                if (modulController.modulType == ModulType.Reactor)
                {
                    message = modulController.modulReactor.EnergyPower.ToString("f0");
                }
                else
                {
                    message = "0";
                }

                SetText(message);
            }
        }
    }

    private ModulBasys FindModul()
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

    private bool GetBotID()
    {
        if (botID != PlayerController.botController.gameObject.GetInstanceID())
        {
            botID = PlayerController.botController.gameObject.GetInstanceID();
            return false;
        }

        return true;
    }

    private bool EnableText()
    {
        if (showTextInformation == true)
        {
            modulText.enabled = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetText(string newText)
    {
        if (EnableText() == true && modulText.text != newText)
        {
            modulText.text = newText;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (modulController != null)
        {
            modulController.AboutThis();
        }
        else
        {
            Debug.Log("No modul controller");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UI_MouseInformation.Instance != null)
        {
            UI_MouseInformation.Instance.ClearMessage();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        changeModulEvent.Invoke();
    }
}
