using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_DebugText : MonoBehaviour 
{
    public RectTransform rectTransform;
    public float speed = 1;
    public Vector3 spacePosition;
    public string debugMessage1;
    public string debugMessage2;
    private Vector3 screenPosition;
    private Camera cameraComponent;
    private float alfa = 1;
    public Text text1;
    public Text text2;
    public int alignment = 1;
    //Debug
    public ModulBasys modulBasys;

    void Start () 
    {
        transform.SetParent(GameObject.Find("DebugTextPanel").transform);
        text1.text = debugMessage1;
        text1.alignment = (TextAnchor)alignment;
        text2.text = debugMessage2;
        text2.alignment = (TextAnchor)alignment;
        rectTransform = GetComponent<RectTransform>();
        cameraComponent = GameObject.Find("Player").GetComponentInChildren<Camera>();
        
	}

    void LateUpdate()
    {
        alfa -= 0.001f;
        spacePosition += (Vector3.up * speed) * Time.deltaTime;
        GetComponent<Text>().color = new Color(1, 1, 1, alfa);
        screenPosition = cameraComponent.WorldToScreenPoint(spacePosition);
        rectTransform.position = screenPosition;
    }
}
