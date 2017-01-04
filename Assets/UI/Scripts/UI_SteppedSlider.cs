using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_SteppedSlider : MonoBehaviour {

    private Slider slider;

    int Value
    {
        get { return (int)slider.value; }
    }
    
    Rect sliderRect;
    float handleWidth;
    int sliderSteps;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.wholeNumbers = true;


        if (!slider.wholeNumbers)
        {
            Debug.LogError("The stepped slider only works with whole number sliders.");
            return;
        }

        slider.onValueChanged.AddListener(delegate { OnSliderValueChangeSetPosition(); });

        sliderRect = slider.GetComponent<RectTransform>().rect;

        sliderSteps = (int)slider.maxValue - (int)slider.minValue + 1;

        handleWidth = sliderRect.width / sliderSteps;
        slider.handleRect.sizeDelta = new Vector2(handleWidth, slider.handleRect.sizeDelta.y);

        SetHandlePosition(Value);
    }

    void OnSliderValueChangeSetPosition()
    {
        SetHandlePosition(Value);
    }

     void SetHandlePosition(int value)
    {
        float xPosition = sliderRect.x + (handleWidth / 2) + handleWidth * (value - slider.minValue);
        slider.handleRect.localPosition = new Vector3(xPosition, slider.handleRect.localPosition.y, slider.handleRect.localPosition.z);
    }
}
