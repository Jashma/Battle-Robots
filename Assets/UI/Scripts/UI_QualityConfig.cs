using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class UI_QualityConfig : MonoBehaviour 
{
    public Text currentQualityText;
    public Dropdown resDropdown; // выпадающая менюшка
    UI_GraphicsPresets graphicsSettings;
    public GameObject customQuality;

    public void OnEnable()
    {
        graphicsSettings = GetComponentInParent<UI_GraphicsPresets>();
        SetDropdownMenu();
	}

    void SetDropdownMenu()
    {
        resDropdown.options = new System.Collections.Generic.List<Dropdown.OptionData>();
        
        foreach (QualityLevel level in Enum.GetValues(typeof(QualityLevel)))
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            resDropdown.options.Add(option);
            option.text = level.ToString();
        }

        currentQualityText.text = QualitySettings.currentLevel.ToString();
        resDropdown.onValueChanged.AddListener(delegate { ApplyQualityLevel(); });
        ApplyQualityLevel();
    }

    void ApplyQualityLevel()
    {
        QualitySettings.SetQualityLevel(resDropdown.value);
        graphicsSettings.SetGraphicsPreset(resDropdown.value);

        if (QualitySettings.currentLevel == QualityLevel.Simple)
        {
            customQuality.SetActive(true);
        }
        else
        {
            customQuality.SetActive(false);
        }
    }
}
