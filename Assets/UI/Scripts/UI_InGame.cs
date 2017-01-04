using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_InGame : MonoBehaviour 
{
    public GameObject botInterface;
<<<<<<< HEAD
    public Button showModulMenuButton;
    public GameObject[] inGameMenuObj;
    public Text timeText;
    public Text fpsText;
    private int indexEnableMenu= -1;
=======
    public GameObject radar;
    public Text DebugText;
    //private float lifeTime = 1;
    
>>>>>>> parent of a891378... Global update
    //Debug
    public bool showNameModul;

	void OnEnable () 
    {
        if (botInterface == null)
        {
            botInterface = transform.FindChild("BotInterface").gameObject;
            radar = transform.FindChild("RadarInterface").gameObject;
        }

        botInterface.SetActive(false);
        radar.SetActive(false);
	}

    void Update()
    {
        if (botInterface.activeSelf == false)
        {
            if (PlayerController.botController != null)
            {
                botInterface.SetActive(true);
                radar.SetActive(true);
            }
        }
        else
        {
<<<<<<< HEAD
            return false;
        }
    }

    void AddChangeSizeButton()
    {
        showModulMenuButton.onClick.RemoveAllListeners();
        showModulMenuButton.onClick.AddListener(delegate { ShowModulEnergyMenu(2); });
    }

    void SetActiveInGameMenu()
    {
        foreach (GameObject obj in inGameMenuObj)
        {
            obj.SetActive(false);
            UI_Controller.Instance.ShowCursor(false);
=======
            if (PlayerController.botController == null)
            {
                DebugText.text = "";
                botInterface.SetActive(false);
                radar.SetActive(false);
            }
            else
            {
                if (DebugText == null)
                {
                    if (GameObject.Find("DebugMessageText") != null)
                    {
                        DebugText = GameObject.Find("DebugMessageText").GetComponent<Text>();
                    }
                }
                else
                {
                    DebugText.text = PlayerController.botController.aiMessage;
                }
            }
>>>>>>> parent of a891378... Global update
        }
    }
}
