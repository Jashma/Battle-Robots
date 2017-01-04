using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UI_ActionMenu : MonoBehaviour 
{
    public Button getControllButton;
    public Button getSpectatorButton;
    public Button getDeploedButton;
    public Button getActionButton;

    void OnEnable()
    {
        FindButton();

        //Add listen Controll Bot
        AddListenerControllButton();

        //Add listen Spectator
        AddSpectatorButton();

        //Add listen Deploed
        AddDeploedButton();

        //Add listen Action Bot
        AddActionButton();
        
    }

    void FindButton()
    {
        if (getControllButton == null)
        {
            getControllButton = transform.FindChild("ControlButton").GetComponent<Button>();
        }

        if (getSpectatorButton == null)
        {
            getSpectatorButton = transform.FindChild("SpectatorButton").GetComponent<Button>();
        }

        if (getDeploedButton == null)
        {
            getDeploedButton = transform.FindChild("DeploedButton").GetComponent<Button>();
        }

        if (getActionButton == null)
        {
            getActionButton = transform.FindChild("ActionButton").GetComponent<Button>();
        }
    }

    void AddListenerControllButton()//Add listen Controll Bot
    {
        getControllButton.onClick.RemoveAllListeners();

        if (PlayerController.playerState == PlayerState.FollowBot)
        {
            getControllButton.GetComponentInChildren<Text>().text = "Controll";
            getControllButton.onClick.AddListener(delegate { UI_Controller.Instance.ChangeActionMenuState(0); });
            getControllButton.onClick.AddListener(delegate { PlayerController.Instance.ChangePlayerState(3); });//"BotControl"
        }
        else
        {
            if (PlayerController.arrayBotController.Count > 0)
            {
                getControllButton.GetComponentInChildren<Text>().text = "Follow";
                getControllButton.onClick.AddListener(delegate { UI_Controller.Instance.ChangeActionMenuState(0); });
                getControllButton.onClick.AddListener(delegate { PlayerController.Instance.ChangePlayerState(2); });//"Follow"
               
            }
            else
            {
                getControllButton.GetComponentInChildren<Text>().text = "None";
            }
        }

    }

    void AddSpectatorButton()//Add listen Spectator
    {
        getSpectatorButton.onClick.RemoveAllListeners();
        getSpectatorButton.GetComponentInChildren<Text>().text = "Spectator";
        getControllButton.onClick.AddListener(delegate { UI_Controller.Instance.ChangeActionMenuState(0); });
        getSpectatorButton.onClick.AddListener(delegate { PlayerController.Instance.ChangePlayerState(1); });//"Spectator"
        //

    }

    void AddDeploedButton()//Add listen Deploed
    {
        getDeploedButton.onClick.RemoveAllListeners();

        if (PlayerController.playerBase != null)
        {
            getDeploedButton.GetComponentInChildren<Text>().text = "Deploed";
            getControllButton.onClick.AddListener(delegate { UI_Controller.Instance.ChangeActionMenuState(0); });
            getDeploedButton.onClick.AddListener(delegate { PlayerController.Instance.ChangePlayerState(4); });//"Deploed"
            getDeploedButton.onClick.AddListener(delegate { UI_Controller.Instance.ChangeState(13); });//"DeploedMenu"
            //
        }
        else
        {
            getDeploedButton.GetComponentInChildren<Text>().text = "None";
        }
    }

    void AddActionButton()//Add listen Action Bot
    {
        getActionButton.onClick.RemoveAllListeners();
        getActionButton.GetComponentInChildren<Text>().text = "None";

        if (PlayerController.playerState == PlayerState.ControlBot)
        {
            if (PlayerController.botController.supportBotController != null)
            {
                //RepearButton
                if (PlayerController.botController.supportBotController.action == SM_BotAction.Repear)
                {
                    getActionButton.GetComponentInChildren<Text>().text = "Repear";
                    getControllButton.onClick.AddListener(delegate { UI_Controller.Instance.ChangeActionMenuState(0); });
                    getActionButton.onClick.AddListener(delegate { PlayerController.botController.ChangeAction(1); });//"Repear"
                    //
                }

                //ChangeModul
                if (PlayerController.botController.supportBotController.action == SM_BotAction.ChangeModul)
                {
                    getActionButton.GetComponentInChildren<Text>().text = "ChangeModul";
                    getControllButton.onClick.AddListener(delegate { UI_Controller.Instance.ChangeActionMenuState(0); });
                    getActionButton.onClick.AddListener(delegate { PlayerController.botController.ChangeAction(3); });//"ChangeModul"
                    getActionButton.onClick.AddListener(delegate { PlayerController.Instance.ChangePlayerState(5); });//"ChangeModul"
                    getActionButton.onClick.AddListener(delegate { UI_Controller.Instance.ChangeState(17); });//"ChangeModulMenu"
                    //

                }
            }
        }
    }

}
