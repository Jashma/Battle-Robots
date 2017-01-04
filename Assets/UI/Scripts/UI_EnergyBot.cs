using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_EnergyBot : MonoBehaviour
{
    public Slider energySlider;
    public Image image;
    public Text maxValueText;
    public Text currentValueText;
    private float value;
    private int botID;
    private ModulReactor modulController;

    //Debug
    public Color colorDB;
   

    void OnEnable()
    {
        if (energySlider == null)
        {
            energySlider = GetComponent<Slider>();
            maxValueText = GetComponentInChildren<Text>();

            foreach (Image searchImage in GetComponentsInChildren<Image>())
            {
                if (searchImage.transform.name == "Fill")
                {
                    image = searchImage;
                    currentValueText = searchImage.GetComponentInChildren<Text>();
                    return;
                }
            }
        }
    }

    void LateUpdate()
    {
        if (PlayerController.botController != null)
        {
            if (botID != PlayerController.botController.GetInstanceID())
            {
                botID = PlayerController.botController.GetInstanceID();
                modulController = null;

                foreach (ModulBasys modul in PlayerController.botController.modulController)
                {
                    if (modul.modulType == ModulType.Reactor)
                    {
                        modulController = modul.GetModulReactor();
                        energySlider.maxValue = modulController.EnergyPower;
                    }
                }
            }

            if (modulController != null)
            {
                energySlider.value = modulController.energyPowerCurrent;

                if (currentValueText != null)
                {
                    currentValueText.text = modulController.energyPowerCurrent.ToString("f0");
                }

                maxValueText.text = modulController.EnergyPower.ToString("f0");
                value = modulController.energyPowerCurrent * 0.1f;
                image.color = new Color(1 - value, value, 0);
                colorDB = new Color(1 - value, value, 0);
                
            }
        }
    }
}
