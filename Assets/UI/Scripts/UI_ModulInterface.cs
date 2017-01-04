using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ModulInterfaceHeight
{
    Hide,
    Smoll,
    Big,
}

public class UI_ModulInterface : MonoBehaviour
{
    public ModulInterfaceHeight interfaceHeight = ModulInterfaceHeight.Smoll;
    public Button changeSizeButton; 
    private RectTransform selfRectTransform;
    public Vector2 defaultSize;

    //Debug 

    public Text debugText;
    public Text nameBotText;

    void OnEnable()
    {
        debugText = transform.FindChild("DebugMessage").GetComponentInChildren<Text>();
        nameBotText = transform.FindChild("NameBot").GetComponentInChildren<Text>();
    }

    void Start()
    {
        selfRectTransform = GetComponent<RectTransform>();
    }

    //Debug
    void Update()
    {
        ChangeModulSize((float)interfaceHeight);

        if (PlayerController.botController != null)
        {
            debugText.text = PlayerController.botController.debugMessage;
            nameBotText.text = PlayerController.botController.name;
        }
    }

    void ChangeModulSize(float size)
    {
        selfRectTransform.sizeDelta = defaultSize * size;
    }

    public void ChangeSizeMenu()
    {
        if (interfaceHeight == ModulInterfaceHeight.Smoll)
        {
            interfaceHeight = ModulInterfaceHeight.Big;
        }
        else
        {
            interfaceHeight = ModulInterfaceHeight.Smoll;
        }
    }
    
}
