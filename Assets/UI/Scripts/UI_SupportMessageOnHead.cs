using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_SupportMessageOnHead : MonoBehaviour
{
    public RectTransform thisRectTransform;
    public Text messageText;
    public Slider slider;
    public Text valueText;
    public Text sliderValueText;
    private Transform targetTransform;
    private float heightOnHead;

    void OnEnable ()
    {
        messageText = transform.FindChild("MessageText").GetComponent<Text>();
        valueText = transform.FindChild("ValueText").GetComponent<Text>();
        slider = transform.FindChild("Slider").GetComponent<Slider>();
        thisRectTransform = GetComponent<RectTransform>();
        transform.SetParent(UI_Controller.Instance.sceneObjTransform);
        SliderOnValueChange();
    }

    void LateUpdate()
    {
        SetScreenPosition();
    }

    void SetScreenPosition()
    {
        thisRectTransform.position = Camera.main.WorldToScreenPoint(targetTransform.position + (Vector3.up * heightOnHead));
    }

    public void SetTargetTransform(Transform newTransform)
    {
        targetTransform = newTransform;
    }

    public void SetHeightOnHead(float value)
    {
        heightOnHead = value;
    }

    public void EnableUIElement(bool value, GameObject obj)
    {
        if (obj.activeSelf != value) { obj.SetActive(value); }
    }

    public void SetTextValue(float value)
    {
        valueText.text = value.ToString("f0");
    }

    public void SetTextMessage(string message)
    {
        messageText.text = message;
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }

    public void SliderOnValueChange()
    {
        sliderValueText.text = slider.value.ToString("f0");
    }
}
