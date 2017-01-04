using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Smaa;

public class UI_QualityAntiAliasing : UI_QualityBase
{
    protected override void GraphicsPresetFastest()
    {
        //Debug.Log("GraphicsPresetFastest");
        SetAntiAliasing(0);
    }

    protected override void GraphicsPresetFast()
    {
        //Debug.Log("GraphicsPresetFast");
        SetAntiAliasing(0);
    }

    protected override void GraphicsPresetSimple()
    {
        //Debug.Log("GraphicsPresetFast");
    }

    protected override void GraphicsPresetGood()
    {
        //Debug.Log("GraphicsPresetGood");
        SetAntiAliasing(1);
    }

    protected override void GraphicsPresetBeautiful()
    {
        //Debug.Log("GraphicsPresetBeautifult");
        SetAntiAliasing(2);
    }

    protected override void GraphicsPresetFantastic()
    {
        //Debug.Log("GraphicsPresetFantastic");
        SetAntiAliasing(3);
    }

    protected override void OnSliderValueChange()
    {
        SetAntiAliasing(Value);
    }

    protected override void Start()
    {
        base.Start();
        displayValue.text = Value.ToString() + "%";
    }

    void SetAntiAliasing(float value)
    {
        /*
        if (value == 2)
        {
            //camera.GetComponent<Antialiasing>().enabled = false;
            //camera.GetComponent<SMAA>().enabled = true;
        }
        else if (value == 1)
        {
            //camera.GetComponent<Antialiasing>().enabled = true;
            //camera.GetComponent<SMAA>().enabled = false;
        }
        else
        {
            //camera.GetComponent<Antialiasing>().enabled = false;
            //camera.GetComponent<SMAA>().enabled = false;
        }
        */
        if (value == 0)
        { QualitySettings.antiAliasing = 0; }

        if (value == 1)
        { QualitySettings.antiAliasing = 2; }

        if (value == 2)
        { QualitySettings.antiAliasing = 4; }

        if (value == 3)
        { QualitySettings.antiAliasing = 8; }

        slider.value = value;
    }
}
