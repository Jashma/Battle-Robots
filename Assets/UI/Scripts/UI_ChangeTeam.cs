using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ChangeTeam : MonoBehaviour 
{

	public void ChangeTeamRed () 
    {
        PlayerController.Instance.PlayerStart();
        UI_Controller.Instance.ChangeState(3);//"InGame"
    }

    public void ChangeTeamBlue()
    {
        PlayerController.Instance.PlayerStart();
        UI_Controller.Instance.ChangeState(3);//"InGame"
    }
}
