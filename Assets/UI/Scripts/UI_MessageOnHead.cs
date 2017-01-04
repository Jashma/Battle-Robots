using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_MessageOnHead : MonoBehaviour
{
    public string message;
    public Transform botTransform;
    public float heightOnHead;
    private Text messageText;
    private Vector3 position;
    
    private Vector2 screenPositionBotHeight;
    private RectTransform rectTransform;

    void OnEnable()
    {
        messageText = GetComponentInChildren<Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        DrawMessage();
    }

    void DrawMessage()
    {
        position = botTransform.position + (Vector3.up * heightOnHead);
        screenPositionBotHeight = Camera.main.WorldToScreenPoint(position);

        rectTransform.position = screenPositionBotHeight;

        if (messageText.text != message)
        {
            messageText.text = message;
        }
    }
}
