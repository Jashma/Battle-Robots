using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_QualityBase : MonoBehaviour 
{
    protected Camera camera;
    protected Text displayValue;
    protected Slider slider;
    public string[] displayLabels;
    public UI_GraphicsPresets graphicsSettings;

    void Awake()
    {
        //Debug.Log("Awake UI_QualityBase");
        camera = Camera.main;
        slider = GetComponent<Slider>();

        graphicsSettings = GetComponentInParent<UI_GraphicsPresets>();
        graphicsSettings.FastestPresetEvent.AddListener(GraphicsPresetFastest);
        graphicsSettings.FastPresetEvent.AddListener(GraphicsPresetFast);
        graphicsSettings.SimplePresetEvent.AddListener(GraphicsPresetSimple);
        graphicsSettings.GoodPresetEvent.AddListener(GraphicsPresetGood);
        graphicsSettings.BeautifulPresetEvent.AddListener(GraphicsPresetBeautiful);
        graphicsSettings.FantasticPresetEvent.AddListener(GraphicsPresetFantastic);
    }

    protected virtual void GraphicsPresetFastest()
    {
    }
    protected virtual void GraphicsPresetFast()
    {
    }
    protected virtual void GraphicsPresetSimple()
    {
    }
    protected virtual void GraphicsPresetGood()
    {
    }
    protected virtual void GraphicsPresetBeautiful()
    {
    }
    protected virtual void GraphicsPresetFantastic()
    {
    }

    protected int Value
    {
        get { return (int)slider.value; }
    }

    protected virtual void Start()
    {
        slider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });
        slider.onValueChanged.AddListener(delegate { OnSliderValueChangeSetDisplayText(); });

        displayValue = transform.Find("Value").GetComponent<Text>();
        displayValue.text = slider.value.ToString();

        if (displayLabels.Length > 0)
        {
            displayValue.text = displayLabels[Value];
        }
    }

    protected virtual void OnSliderValueChange()
    {
    }

    protected virtual void OnSliderValueChangeSetDisplayText()
    {
        if (displayLabels.Length > 0)
        {
            displayValue.text = displayLabels[Value];
        }
        else
        {
            displayValue.text = Value.ToString();
        }
    }
}
