using UnityEngine;
using System.Collections;

public class UI_QualityShadow : UI_QualityBase
{
    protected override void GraphicsPresetFastest()
    {
        //Debug.Log("GraphicsPresetFastest");
        SetShadowDistance(10);
    }

    protected override void GraphicsPresetFast()
    {
        //Debug.Log("GraphicsPresetFast");
        SetShadowDistance(20);
    }

    protected override void GraphicsPresetSimple()
    {
        //Debug.Log("GraphicsPresetFast");
        slider.value = QualitySettings.shadowDistance;
    }

    protected override void GraphicsPresetGood()
    {
        //Debug.Log("GraphicsPresetGood");
        SetShadowDistance(60);
    }

    protected override void GraphicsPresetBeautiful()
    {
        //Debug.Log("GraphicsPresetBeautifult");
        SetShadowDistance(80);
    }

    protected override void GraphicsPresetFantastic()
    {
        //Debug.Log("GraphicsPresetFantastic");
        SetShadowDistance(100);
    }

    protected override void Start()
    {
        base.Start();
        displayValue.text = Value.ToString() + "%";
    }

    protected override void OnSliderValueChange()
    {
        SetShadowDistance(Value);
    }

    void SetShadowDistance(float value)
    {
        slider.value = value;
        QualitySettings.shadowDistance = value;
        //slider.value = value;
    }
}
