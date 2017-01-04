using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_BotInterface : MonoBehaviour
{
    public Text debugText;
    public Text nameBotText;
    
    /*
    void OnEnable ()
    {
        debugText = transform.FindChild("DebugMessage").GetComponentInChildren<Text>();
        nameBotText = transform.FindChild("NameBot").GetComponentInChildren<Text>();
    }
	
	void Update ()
    {
        if (PlayerController.botController != null)
        {
            debugText.text = PlayerController.botController.debugMessage;
            nameBotText.text = PlayerController.botController.name;
        }
    }
    */
}
