using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UI_ActionMenu : MonoBehaviour 
{
    public GameObject ActionPrefab;
    public Vector3 createPosition;
    public Button getControllButton;
    public Button getFollowButton;
    public Button getSpectatorButton;
    public Button getDeploedButton;
    private GameObject actionObj;

    private void OnEnable()
    {
        //Create Action Obj
        ActionPrefab = Resources.Load("Prefabs/CreatePlace") as GameObject;

        if (GameObject.Find("CreatePlace") != null)
        {
            Destroy(GameObject.Find("CreatePlace"));
        }

        //Add listen Controll Bot
        getControllButton.onClick.RemoveAllListeners();

        if (GameObject.Find("Player") != null && PlayerController.playerState == PlayerState.Follow)
        {
            getControllButton.GetComponentInChildren<Text>().text = "Controll";
            getControllButton.onClick.AddListener(delegate { GameObject.Find("Player").GetComponent<PlayerController>().ChangePlayerState("BotControl"); });
        }
        else
        {
            getControllButton.GetComponentInChildren<Text>().text = "None";
        }

        //Add listen Follow Bot
        getFollowButton.onClick.RemoveAllListeners();

        if (GameObject.Find("Player") != null && PlayerController.arrayBotController.Count > 0)
        {
            getFollowButton.GetComponentInChildren<Text>().text = "Follow";
            getFollowButton.onClick.AddListener(delegate { GameObject.Find("Player").GetComponent<PlayerController>().ChangePlayerState("Follow"); });
        }
        else
        {
            getFollowButton.GetComponentInChildren<Text>().text = "None";
        }

        //Add listen Spectator
        getSpectatorButton.onClick.RemoveAllListeners();

        if (GameObject.Find("Player") != null)
        {
            getSpectatorButton.GetComponentInChildren<Text>().text = "Spectator";
            getSpectatorButton.onClick.AddListener(delegate { GameObject.Find("Player").GetComponent<PlayerController>().ChangePlayerState("Spectator"); });
        }
        else
        {
            getSpectatorButton.GetComponentInChildren<Text>().text = "None";
        }

        //Add listen Deploed
        getDeploedButton.onClick.RemoveAllListeners();

        if (GameObject.Find("Player") != null)
        {
            getDeploedButton.onClick.AddListener(delegate { GameObject.Find("Player").GetComponent<PlayerController>().ChangePlayerState("Deploed"); });
            getDeploedButton.onClick.AddListener(delegate { GetComponentInParent<UI_Controller>().ChangeState("DeploedMenu"); });
        }
        else
        {
            getSpectatorButton.GetComponentInChildren<Text>().text = "None";
        }
    }

    public void CreateActionObj()
    {
        if (actionObj != null)
        {
            if (actionObj.GetComponent<ActionObj>().inAction == true)
            {
                Destroy(actionObj);
            }
            
            actionObj = null;
        }

        float terrainY = Terrain.activeTerrain.SampleHeight(PlayerController.hitForwardCollision.point);
        createPosition = new Vector3(PlayerController.hitForwardCollision.point.x,terrainY + 0.5f,10);
        actionObj = Instantiate(Resources.Load("Prefabs/CreatePlace"), createPosition, Quaternion.identity) as GameObject;
    }
}
