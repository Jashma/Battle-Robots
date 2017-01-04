using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UI_GameConfig : MonoBehaviour
{
    private Slider mouseSenceSlider;
    private Text mouseSenceValue;

    private Slider distToHitSlider;
    private Text distToHitValue;

    private Slider aimLerpSlider;
    private Text aimLerpValue;

    private Toggle hitInformation;
    private Toggle aimInformation;

    void OnEnable()
    {
        if (mouseSenceSlider == null)
        {
            foreach (Transform trans in GetComponentsInChildren<Transform>())
            {
                if (trans.name == "AimInfoToggle")
                {
                    aimInformation = trans.GetComponent<Toggle>();
                    GameController.showAimInformation = hitInformation.isOn;
                }

                if (trans.name == "HitInfoToggle")
                {
                    hitInformation = trans.GetComponent<Toggle>();
                    GameController.showHitInformation = hitInformation.isOn;
                }

                if (trans.name == "MouseSenceValue")
                {
                    mouseSenceValue = trans.GetComponent<Text>();
                    mouseSenceValue.text = mouseSenceSlider.value.ToString();
                }

                if (trans.name == "MouseSenceSlider")
                {
                    mouseSenceSlider = trans.GetComponent<Slider>();
                    mouseSenceSlider.value = PlayerController.mouseSence;
                }

                if (trans.name == "DistanceToHitValue")
                {
                    distToHitValue = trans.GetComponent<Text>();
                    distToHitValue.text = distToHitSlider.value.ToString();
                }

                if (trans.name == "DistanceToHitSlider")
                {
                    distToHitSlider = trans.GetComponent<Slider>();
                    distToHitSlider.value = GameController.distToHit;
                }

                if (trans.name == "AimLerpSlider")
                {
                    aimLerpSlider = trans.GetComponent<Slider>();
                    aimLerpSlider.value = GameController.aimLerp;
                }

                if (trans.name == "AimLerpValueText")
                {
                    aimLerpValue = trans.GetComponent<Text>();
                    aimLerpValue.text = aimLerpSlider.value.ToString();
                }
            }

            mouseSenceSlider.onValueChanged.AddListener(delegate { ChangeMouseSence(); });
            distToHitSlider.onValueChanged.AddListener(delegate { ChangeDistToHit(); });
            aimLerpSlider.onValueChanged.AddListener(delegate { ChangeAimLerp(); });

            hitInformation.onValueChanged.AddListener(delegate { ChangeHitInformation(); });
            aimInformation.onValueChanged.AddListener(delegate { ChangeAimInformation(); });
        }
    }

    public void ChangeAimLerp()
    {
        GameController.aimLerp = aimLerpSlider.value;
        aimLerpValue.text = aimLerpSlider.value.ToString("f1");
    }

    public void ChangeHitInformation()
    {
        GameController.showHitInformation = hitInformation.isOn;
    }

    public void ChangeAimInformation()
    {
        GameController.showAimInformation = aimInformation.isOn;
    }

    public void ChangeMouseSence ()
    {
        PlayerController.mouseSence = mouseSenceSlider.value;
        mouseSenceValue.text = mouseSenceSlider.value.ToString("f1");
    }

    public void ChangeDistToHit()
    {
        GameController.distToHit = distToHitSlider.value;
        distToHitValue.text = distToHitSlider.value.ToString("f1");
    }
}
