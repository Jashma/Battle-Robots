using UnityEngine;
using System.Collections;

public class UI_DrawMarkerIndicator : MonoBehaviour 
{
    private PlayerController playerController;
    public BotController botController;
    private Camera cameraComponent;
    private RectTransform markImageTransform;
    private float imageWidthMark;
    private float imageHeightMark;
    private Vector3 tmpPosition;
    private float height;
    private Vector3 screenPosition;

    void OnEnable()
    {
        cameraComponent = GameObject.Find("Player").GetComponentInChildren<Camera>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        botController = GetComponentInParent<UI_BotIndicator>().botController;
        markImageTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        DrawMarker();
    }

    void DrawMarker()
    {
        imageWidthMark = 100 - Vector3.Distance(playerController.transform.position, botController.transform.position) * 1.5f;
        imageHeightMark = imageWidthMark;// - Vector3.Distance(playerController.transform.position, botController.transform.position)/4;
        markImageTransform.sizeDelta = new Vector2(imageWidthMark, imageHeightMark);
        height = botController.characterController.height / 2.5f;
        tmpPosition = new Vector3(botController.transform.position.x,
                                  botController.transform.position.y + height,
                                  botController.transform.position.z);
        screenPosition = cameraComponent.WorldToScreenPoint(tmpPosition);
        markImageTransform.position = screenPosition;
    }
}
