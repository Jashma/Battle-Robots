using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_BotIndicator : MonoBehaviour  
{
    public BotController botController;
    private GameObject healthObj;
    private GameObject markerObj;


    void OnEnable()
    {
        healthObj = transform.FindChild("HealthImage").gameObject;
        markerObj = transform.FindChild("MarkImage").gameObject;

        healthObj.SetActive(false);
        markerObj.SetActive(false);
    }

    void LateUpdate()
    {
        if (botController != null)
        {
            if (botController.checkVisibleMesh.camVisible == true)
            {
                if (PlayerController.botController != botController)
                {
                    //Рисуем Полоску ХП
                    if (botController.team == PlayerController.playerTeam)
                    {
                        if (healthObj.activeSelf == false) { healthObj.SetActive(true); }
                    }
                    else
                    {
                        if (botController.timeVisualFound >= Time.time)
                        {
                            if (healthObj.activeSelf == false) { healthObj.SetActive(true); }
                        }
                        else
                        {
                            if (healthObj.activeSelf == true) { healthObj.SetActive(false); }
                        }
                    }

                    //Рисуем Маркер
                    if (botController.team != PlayerController.playerTeam && botController.timeRadarFound >= Time.time)
                    {
                        if (markerObj.activeSelf == false) { markerObj.SetActive(true); }
                    }
                    else
                    {
                        if (markerObj.activeSelf == true) { markerObj.SetActive(false); }
                    }
                }
                else
                {
                    if (healthObj.activeSelf == true) { healthObj.SetActive(false); }
                    if (markerObj.activeSelf == true) { markerObj.SetActive(false); }
                }
            }
            else
            {
                if (healthObj.activeSelf == true) { healthObj.SetActive(false); }
                if (markerObj.activeSelf == true) { markerObj.SetActive(false); }
            }
        }
    }
}
