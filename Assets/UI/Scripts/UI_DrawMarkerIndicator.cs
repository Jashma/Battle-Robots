using UnityEngine;
using System.Collections;

public class UI_DrawMarkerIndicator : MonoBehaviour 
{
    public Transform playerTransform;
    public Transform botTransform;
    private Camera cameraComponent;
    private RectTransform markImageTransform;
    private float imageWidthMark;
    private float imageHeightMark;
    private Vector3 tmpPosition;
    //private float height;
    private Vector2 screenPosition;

    void OnEnable()
    {
        playerTransform = PlayerController.Instance.transform;
        cameraComponent = playerTransform.GetComponentInChildren<Camera>();
        markImageTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        DrawMarker();
    }

    void DrawMarker()
    {
        imageWidthMark = 100 - Vector3.Distance(playerTransform.position, botTransform.position) * 1.5f;
        imageHeightMark = imageWidthMark;// - Vector3.Distance(playerController.transform.position, botController.transform.position)/4;
        markImageTransform.sizeDelta = new Vector2(imageWidthMark, imageHeightMark);
        tmpPosition = new Vector3(botTransform.position.x,
                                  botTransform.position.y,
                                  botTransform.position.z);
        screenPosition = cameraComponent.WorldToScreenPoint(tmpPosition);
        markImageTransform.position = screenPosition;
    }
}
