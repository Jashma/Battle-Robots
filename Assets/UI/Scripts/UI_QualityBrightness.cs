using UnityEngine;
using System.Collections;

public class UI_QualityBrightness : UI_QualityBase
{
    protected override void GraphicsPresetFastest()
    {
        //Debug.Log("GraphicsPresetFastest");
        SetBrightness(40);
    }

    protected override void GraphicsPresetFast()
    {
        //Debug.Log("GraphicsPresetFast");
        SetBrightness(40);
    }

    protected override void GraphicsPresetSimple()
    {
        //Debug.Log("GraphicsPresetFast");
        //slider.value = camera.GetComponent<Brightness>().brightness * 100f;
    }

    protected override void GraphicsPresetGood()
    {
        //Debug.Log("GraphicsPresetGood");
        SetBrightness(40);
    }

    protected override void GraphicsPresetBeautiful()
    {
        //Debug.Log("GraphicsPresetBeautifult");
        SetBrightness(40);
    }

    protected override void GraphicsPresetFantastic()
    {
        //Debug.Log("GraphicsPresetFantastic");
        SetBrightness(40);
    }

     
    protected override void Start()
    {
        base.Start();
        displayValue.text = Value.ToString() + "%";
    }

    protected override void OnSliderValueChange()
    {
        SetBrightness(Value);
    }

    protected override void OnSliderValueChangeSetDisplayText()
    {
        displayValue.text = Value.ToString() + "%";
    }

    void SetBrightness(float value)
    {
        slider.value = value;
        camera.GetComponent<Brightness>().brightness = (slider.value * 3) / 100f;
        //camera.GetComponent<Brightness>().brightness = (value * 3) / 100f;
    }
}
