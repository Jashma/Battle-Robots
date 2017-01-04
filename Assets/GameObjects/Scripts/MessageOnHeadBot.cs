using UnityEngine;
using System.Collections;

public class MessageOnHeadBot : MonoBehaviour
{
    public bool showMessageOnHead;

    public string message;
    public float value;
    public float valueText;
    public float heightOnHead;

    public bool showSlider = false;
    public bool showValue = false;
    public bool showMessage = false;

    private bool camVisible;
    public Transform thisTransform;
    private GameObject ui_MessageObj;
    private UI_SupportMessageOnHead ui_MessageOnHead;

    void Start ()
    {
        if (ui_MessageObj == null)
        {
            thisTransform = GetComponent<Transform>();

            if (UI_Controller.Instance.transform != null)
            {
                ui_MessageObj = Instantiate(Resources.Load("Prefabs/User Interface/MessageOnHead")) as GameObject;
                ui_MessageObj.transform.SetParent(UI_Controller.Instance.transform);
                ui_MessageOnHead = ui_MessageObj.GetComponent<UI_SupportMessageOnHead>();
                ui_MessageOnHead.SetTargetTransform(thisTransform);

            }
        }
    }

    void Update()
    {
        ui_MessageOnHead.SetHeightOnHead(heightOnHead);

        ui_MessageOnHead.SetTextValue(valueText);
        ui_MessageOnHead.SetTextMessage(message);
        ui_MessageOnHead.SetSliderValue(value);

        ui_MessageOnHead.EnableUIElement(showMessage, ui_MessageOnHead.messageText.gameObject);
        ui_MessageOnHead.EnableUIElement(showValue, ui_MessageOnHead.valueText.gameObject);
        ui_MessageOnHead.EnableUIElement(showSlider, ui_MessageOnHead.slider.gameObject);

        if (camVisible == true && showMessageOnHead == true)
        {
            if (ui_MessageObj.activeSelf == false) { ui_MessageObj.SetActive(true); }
        }
        else
        {
            if (ui_MessageObj.activeSelf == true) { ui_MessageObj.SetActive(false); }
        }

        //Debug
        valueText = value;
        //EndDebug
    }

    void OnDestroy()
    {
        Destroy(ui_MessageObj);
    }

    void OnBecameVisible()
    {
        camVisible = true;
    }

    void OnBecameInvisible()
    {
        camVisible = false;
    }
}
