using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_ConsolMessageController : MonoBehaviour
{
    public static UI_ConsolMessageController Instance { get; private set; }
    public string message;
    public bool showConsol = false;
    public Text[] textElement = new Text[10];

    private Vector2 showPosition = new Vector2(6, 275);
    private Vector2 hidePosition = new Vector2(-300, 275);
    private Text messageText;

    //Debug 
    public string oldMessage;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        ClearAllMessage();
        ChangePosition();
    }

    void ClearAllMessage()
    {
        foreach (Text newText in textElement)
        {
            newText.text = "";
        }
    }

    public void SetNewMessage(string newMessage)
    {
        if (newMessage != message)
        {
            oldMessage = message;
            message = newMessage;
            ChangeMessage(message);
        }
    }

    public void ChangeValue()
    {
        if (showConsol == false)
        {
            showConsol = true;
        }
        else
        {
            showConsol = false;
        }

        ChangePosition();
    }

    void ChangePosition()
    {
        if (showConsol == true)
        {
            GetComponent<RectTransform>().position = showPosition;
        }
        else
        {
            GetComponent<RectTransform>().position = hidePosition;
        }
    }

    void ChangeMessage(string message)
    {
        for (int i = textElement.Length - 1; i > -1; i--)
        {
            if (i == 0)
            {
                textElement[i].text = message;
            }
            else
            {
                textElement[i].text = textElement[i - 1].text;
            }
        }
    }

}
