using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_InGame : MonoBehaviour
{
    public GameObject botInterface;
    public Button showModulMenuButton;
    public GameObject[] inGameMenuObj;
    public Text timeText;
    public Text fpsText;
    private int indexEnableMenu= -1;
    //Debug
    public bool showTextInformation;

    void OnEnable()
    {
        botInterface = transform.FindChild("BotInterface").gameObject;
        timeText = transform.FindChild("Time").GetComponentInChildren<Text>();
        fpsText = transform.FindChild("Fps").GetComponentInChildren<Text>();
        AddChangeSizeButton();

        botInterface.SetActive(EnableBotInterface());
        SetActiveInGameMenu();
    }

    void Update()
    {
        timeText.text = TimerEmult.minedgametime.ToString("f0") + " : " + TimerEmult.timersecond.ToString("f0");
        fpsText.text = FramesPerSecond.fps.ToString("f1");

        botInterface.SetActive(EnableBotInterface());

        if (indexEnableMenu != (int)UI_Controller.uiInGameMenu)
        {
            SetActiveInGameMenu();
            indexEnableMenu = (int)UI_Controller.uiInGameMenu;
        }

    }

    private bool EnableBotInterface()
    {
        if (PlayerController.botController != null)
        {
            return true;
        }
        else
        {
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
        }

        if ((int)UI_Controller.uiInGameMenu != 0)
        {
            inGameMenuObj[(int)UI_Controller.uiInGameMenu - 1].SetActive(true);
            UI_Controller.Instance.ShowCursor(true);
        }
    }

    public void ShowModulEnergyMenu(int value = 2)
    {
        UI_Controller.Instance.ChangeActionMenuState(value);//ChangeReloadEnergyQuoue
        //UI_Controller.Instance.ChangeActionMenuState(3);//BigModulInterface
    }
}
