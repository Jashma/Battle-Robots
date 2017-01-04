using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_AbautThis : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string[] information;

    void Start()
    {
        information = new string[1];
        information[0] = name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI_MouseInformation.Instance.SetMessage(information);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_MouseInformation.Instance.ClearMessage();
    }
}
