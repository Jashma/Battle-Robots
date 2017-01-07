using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_EnergyConfig : MonoBehaviour
{
    public ModulBasys modulReactor;
    public GameObject changeEnergyObj;
    public Text freeEnergyText;
    public Text energyText;
    public Slider energySlider;

    private UI_ModulEnergy currentUIModul;
    private Slider changeEnergySlider;
    private Text changeEnergyMaxSliderText;
    private Text changeEnergyValueSliderText;

    void OnEnable ()
    {
        freeEnergyText = transform.FindChild("FreeEnergy").GetComponent<Text>();
        energySlider = transform.FindChild("EnergySlider").GetComponent<Slider>();
        energyText = energySlider.transform.FindChild("EnergyValue").GetComponent<Text>();
        modulReactor = PlayerController.botController.GetModulByStatus(ModulType.Reactor);
        changeEnergyObj = transform.FindChild("ChangeEnergySlider").gameObject;
        changeEnergySlider = changeEnergyObj.GetComponentInChildren<Slider>();
        changeEnergyMaxSliderText = changeEnergySlider.GetComponentInChildren<Text>();
        changeEnergyValueSliderText = GameController.Instance.FindTransform(changeEnergyObj, "CurrentValueEnergy").GetComponent<Text>();
        EnableChangeEnergy(false);
    }

    void Update()
    {
        energySlider.maxValue = modulReactor.energyMaxValue;
        energySlider.value = modulReactor.EnergyPower;

        SetText(energyText, modulReactor.EnergyPower.ToString());
    }

    void SetText(Text text, string newText)
    {
        if (text.text != newText)
        {
            text.text = newText;
        }
    }

    public void SelectChangeModul(UI_ModulEnergy ui_modul)
    {
        if (currentUIModul != null) { currentUIModul.startCrossfade = false; }

        currentUIModul = ui_modul;
        EnableChangeEnergy(true);

        changeEnergySlider.maxValue = currentUIModul.modulController.energyMaxValue;
        changeEnergySlider.value = currentUIModul.modulController.energyReloadQuoue;
        changeEnergyMaxSliderText.text = changeEnergySlider.maxValue.ToString("f0");
    }

    public void SliderValueChange()
    {

    }


    public void EnableChangeEnergy(bool value)
    {
        changeEnergyObj.SetActive(value);
        
    }

    
}
