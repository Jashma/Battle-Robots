using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI_Modul : MonoBehaviour 
{
    public ModulScr modulController;
    public Text modulText;
    public Color enableColor;
    public Color disableColor;
    private float healthModul;
    private Image image;
    private int botID;
    //Debug
    public float health;

    void OnEnable()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
            modulText = GetComponentInChildren<Text>();
        }

        modulController = null;

        if (PlayerController.botController != null)
        {
            botID = PlayerController.botController.GetInstanceID();

            foreach (ModulScr modul in PlayerController.botController.modulScr)
            {
                if (modul.transform.parent.tag == tag)
                {
                    modulController = modul;

                    if (GetComponentInParent<UI_InGame>().showNameModul == true)
                    {
                        modulText.text = modulController.nameModul;
                    }
                    else
                    {
                        modulText.text = "";
                    }

<<<<<<< HEAD
                if (modulController.modulType == ModulType.Reactor)
                {
                    message = modulController.modulReactor.EnergyPower.ToString("f0");
=======
                    GetComponent<Image>().color = enableColor;
                    return;
>>>>>>> parent of a891378... Global update
                }
            }

            image.color = disableColor;
        }
    }

    void Update()
    {
        if (botID != PlayerController.botController.GetInstanceID())
        {
            OnEnable();
        }

        if (modulController != null)
        {
            if (health != modulController.health * 0.01f)
            {
                healthModul = modulController.health;
                health = modulController.health * 0.01f;
                image.color = new Color(1 - health, health, 0, 0.5f);
            }

            if (health == 0)
            {
                image.color = new Color(1 - health, health, 0, 0.5f);
            }
        }
    }
}
