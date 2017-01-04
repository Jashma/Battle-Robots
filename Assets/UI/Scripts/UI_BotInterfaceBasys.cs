using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum MenuPosition
{
    Show,
    Hide,
    Disable,
}

public abstract class UI_BotInterfaceBasys : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform selfRectTransform;
    public Vector3 showPosition;
    public Vector3 hidePosition;
    public Vector3 disablePosition;
    public string nameInterface;
    public Color enableColor;
    public Color disableColor;
    public Color currentColor;
    public MenuPosition currentPosition;
    public ModulStatus modulStatus;

    virtual public void CheckMenuPosition()
    {
        if (currentPosition == MenuPosition.Show)
        {
            if (selfRectTransform.anchoredPosition3D != showPosition)
            {
                selfRectTransform.anchoredPosition3D = showPosition;
            }
        }

        if (currentPosition == MenuPosition.Hide)
        {
            if (selfRectTransform.anchoredPosition3D != hidePosition)
            {
                selfRectTransform.anchoredPosition3D = hidePosition;
            }
        }

        if (currentPosition == MenuPosition.Disable)
        {
            if (selfRectTransform.anchoredPosition3D != disablePosition)
            {
                selfRectTransform.anchoredPosition3D = disablePosition;
            }
        }
    }

    virtual public void CheckMenuState(ModulStatus modulStatus)
    {
        if (modulStatus == ModulStatus.On)
        {
            currentColor = enableColor;
        }

        if (modulStatus == ModulStatus.Off)
        {
            currentColor = disableColor;
        }
    }

    virtual public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UI_MouseInformation.Instance != null)
        {
            UI_MouseInformation.Instance.ClearMessage();
        }
    }
}
