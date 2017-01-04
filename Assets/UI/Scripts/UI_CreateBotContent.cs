using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_CreateBotContent : MonoBehaviour 
{
    public Text nameBot;
    public Button addBot;
    public int botType;
    public DeploedShipController deploedShipController;
	
	public void OnClickButton () 
    {
        deploedShipController.AddBotQuoue(botType);
	}
}
