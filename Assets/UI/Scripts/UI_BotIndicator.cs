using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_BotIndicator : MonoBehaviour  
{
    public bool showHealthIndicator;
    public bool showMarkerIndicator;
    public bool showMessageIndicator;
    private GameObject healthObj;
    private GameObject markerObj;
    private GameObject messageObj;

    void OnEnable()
    {
        healthObj = transform.FindChild("HealthImage").gameObject;
        markerObj = transform.FindChild("MarkImage").gameObject;
        messageObj = transform.FindChild("Message").gameObject;
    }

    void LateUpdate()
    {
        if (transform.parent == null)
        {
            if (GameObject.Find("SceneObject") != null)
            {
                transform.SetParent(GameObject.Find("SceneObject").transform);
            }
        }
        else
        {
            if (healthObj.activeSelf != showHealthIndicator) { healthObj.SetActive(showHealthIndicator); }
            if (markerObj.activeSelf != showMarkerIndicator) { markerObj.SetActive(showMarkerIndicator); }
            if (messageObj.activeSelf != showMessageIndicator) { messageObj.SetActive(showMessageIndicator); }
        }
    }
}
