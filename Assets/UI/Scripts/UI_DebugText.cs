using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_DebugText : MonoBehaviour 
{
    public RectTransform rectTransform;
    public Vector3 spacePosition;
    public string debugMessage;
    private Vector3 screenPosition;
    private Camera cameraComponent;
    private float alfa = 1;

    void Start () 
    {
        transform.SetParent(GameObject.Find("UI").transform);
        GetComponent<Text>().text = debugMessage;
        rectTransform = GetComponent<RectTransform>();
        cameraComponent = GameObject.Find("Player").GetComponentInChildren<Camera>();
        
	}

    void LateUpdate()
    {
        alfa -= 0.01f;
        spacePosition += (Vector3.up * 0.05f);
        GetComponent<Text>().color = new Color(1, 1, 1, alfa);
        screenPosition = cameraComponent.WorldToScreenPoint(spacePosition);
        rectTransform.position = screenPosition;
    }
}
