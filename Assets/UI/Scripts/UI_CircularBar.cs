using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_CircularBar : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public ModulType modulType;
    public ModulBasys modulController;
    private int botID;

    private Image sliderImage;
    private bool isPointerDown = false;
    private Text valueText;
    private Vector2 mousePosition;
    private float angle;
    private GraphicRaycaster raycaster;

    void OnEnable()
    {
        /*
        sliderImage = transform.FindChild("RadialSliderImage").GetComponent<Image>();
        valueText = GetComponentInChildren<Text>();
        raycaster = GetComponentInParent<GraphicRaycaster>();

        botID = PlayerController.botController.gameObject.GetInstanceID();

        modulController = FindModul();

        if (modulController != null)
        {
            angle = modulController.energyReloadQuoue / 100;
            SetFillAmount(angle);
            SetColor(angle);
            SetValue(angle);
            SetValue(angle * 100f);
        }
        */
    }

    private ModulBasys FindModul()
    {
        foreach (ModulBasys modul in PlayerController.botController.modulController)
        {
            if (modul.modulType == modulType)
            {
                return modul;
            }
        }
        return null;
    }

    void Update()
    {
        /*
        if (isPointerDown && modulController != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, raycaster.eventCamera, out mousePosition);

            angle = (Mathf.Atan2(-mousePosition.y, mousePosition.x) * 180f / Mathf.PI + 180f) / 360f;

            SetFillAmount(angle);
            SetColor(angle);
            SetValue(angle);
            SetValue(angle * 100f);
            ValueAdd();
        }
        */
    }

    private void ValueAdd()
    {
        //modulController.energyReloadQuoue = sliderImage.fillAmount * 100;
    }

    private void SetFillAmount(float value)
    {
        sliderImage.fillAmount = value;
    }

    private void SetValue(float value)
    {
        valueText.text = value.ToString("f0");
    }

    private void SetColor(float value)
    {
        sliderImage.color = Color.Lerp(Color.red, Color.green, value);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }

}
