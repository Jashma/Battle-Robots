using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_DeploedMenu : MonoBehaviour 
{
    
    public Transform parentTransform;
    public Vector3 startPosition = new Vector3(90, -35, 0);
    private float otstup = 50;
    public Toggle toggle;
    public Text queueBot;
    private int queueValue;
    private DeploedShipController deploedShipController;
    private  GameObject botPanelObj;
    private GameObject botPanelPrefab;
    

	void OnEnable () 
    {
        if (botPanelPrefab == null)
        {
            botPanelPrefab = Resources.Load("Prefabs/User Interface/BotPanel") as GameObject;
        }

        if (toggle == null)
        { toggle = transform.FindChild("AutoToggle").GetComponent<Toggle>(); }

        if (queueBot == null)
        { queueBot = transform.FindChild("QuoueText").GetComponent<Text>(); }
        
        if (deploedShipController != null)
        {
            if (botPanelObj == null)
            {
                LoadBotPanel();
            }

            toggle.isOn = deploedShipController.autoCreateBot;
        }
	}

    void OnDisable()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().ChangePlayerState(2);//"Follow"
    }

    void Update()
    {
        queueValue = 0;

        for (int i = 0; i < deploedShipController.queueBot.Length; i++)
        {
            if (deploedShipController.queueBot[i] > 0)
            {
                queueValue++;
            }
        }

        queueBot.text = queueValue.ToString();
    }

    void LoadBotPanel()
    {
        for (int i=0; i<deploedShipController.spawnObjPrefab.Length; i++)
        {
            botPanelObj = Instantiate(botPanelPrefab);
            botPanelObj.transform.SetParent(parentTransform);
            botPanelObj.transform.localScale = Vector3.one;
            botPanelObj.transform.localPosition = startPosition + Vector3.down * otstup * i;
            botPanelObj.GetComponent<UI_CreateBotContent>().nameBot.text = deploedShipController.spawnObjPrefab[i].name;
            botPanelObj.GetComponent<UI_CreateBotContent>().botType = i;
            botPanelObj.GetComponent<UI_CreateBotContent>().deploedShipController = deploedShipController;
        }
    }

    public void AutoCreateToggle()
    {
        deploedShipController.autoCreateBot = toggle.isOn;
    }
}
