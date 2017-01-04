using UnityEngine;
using System.Collections;

public class UI_QualityVsync : UI_QualityBase
{
    protected override void GraphicsPresetFastest()
    {
        //Debug.Log("GraphicsPresetFastest");
        SetVsync(0);
    }

    protected override void GraphicsPresetFast()
    {
        //Debug.Log("GraphicsPresetFast");
        SetVsync(0);
    }

    protected override void GraphicsPresetSimple()
    {
        slider.value = QualitySettings.vSyncCount;
    }

    protected override void GraphicsPresetGood()
    {
        //Debug.Log("GraphicsPresetGood");
        SetVsync(1);
    }

    protected override void GraphicsPresetBeautiful()
    {
        //Debug.Log("GraphicsPresetBeautifult");
        SetVsync(2);
    }

    protected override void GraphicsPresetFantastic()
    {
        //Debug.Log("GraphicsPresetFantastic");
        SetVsync(2);
    }

    protected override void OnSliderValueChange()
    {
        SetVsync(Mathf.Clamp(Value, 0, displayLabels.Length));
    }

    void SetVsync(int value)
    {
        slider.value = value;
        QualitySettings.vSyncCount = value;
        
    }
}
