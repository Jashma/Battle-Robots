using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_ConnectionMenu : MonoBehaviour 
{
    public Button startSingleGame;

    void OnEnable () 
    {
        //startSingleGame.onClick.AddListener(delegate { GameObject.Find("LevelController").GetComponent<LevelController>().StartGame(); });
        //TestConnection();
	}

    void TestConnection()
    {
        //NetworkManager.Instance.InitButton();
        //NetworkManager.Instance.doneTesting = false;
        //NetworkManager.Instance.Connection();
    }
}
