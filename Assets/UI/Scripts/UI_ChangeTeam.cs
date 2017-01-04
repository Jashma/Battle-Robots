using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ChangeTeam : MonoBehaviour 
{

	public void ChangeTeamRed () 
    {
        GameObject.Find("Player").GetComponent<PlayerController>().PlayerStart("Red");
	}

    public void ChangeTeamBlue()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().PlayerStart("Blue");
    }
}
