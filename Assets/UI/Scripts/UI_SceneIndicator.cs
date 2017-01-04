using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_SceneIndicator : MonoBehaviour
{
    public bool show = false;
    public bool showMessage = false;
    public bool showSlider = false;
    public bool showSliderValue = false;
    public Vector3 targetPosition;
    public float heightOnHead;
    public string message;
    public float value;
    private Text messageText;
    private Slider slider;
    private RectTransform oldValueSlider;
    private RectTransform thisRectTransform;

    void OnEnable()
    {
        slider = GetComponentInChildren<Slider>();
        messageText = GetComponentInChildren<Text>();
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(delegate { ChangeSliderValue(); });
        oldValueSlider = GameController.Instance.FindRectTransform(this.gameObject, "OldValue");
    }

    public void ChangeSliderValue()
    {
        if (show == true)
        {
            GetComponentInChildren<Text>().text = slider.value.ToString("f0");
        }
        else
        {
            GetComponentInChildren<Text>().text = "";
        }
    }

    void Update()
    {
        Show();
        SetScreenPosition();
    }

    void Show()
    {
        if (show == true)
        {
            EnableUIElement(showMessage, messageText.gameObject);
            EnableUIElement(showSlider, slider.gameObject);
        }
        else
        {
            EnableUIElement(false, messageText.gameObject);
            EnableUIElement(false, slider.gameObject);
        }
    }

    void SetSliderValue()
    {
        slider.value = value;
    }

    void SetTextMessage()
    {
        messageText.text = message;
    }

    void SetScreenPosition()
    {
        thisRectTransform.position = Camera.main.WorldToScreenPoint(targetPosition + (Vector3.up * heightOnHead));
    }

    void EnableUIElement(bool value, GameObject obj)
    {
        if (obj.activeSelf != value)
        {
            obj.SetActive(value);
        }
    }
}
