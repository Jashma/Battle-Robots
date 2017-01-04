using UnityEngine;
using System.Collections;

public class UI_BotOrientationController : MonoBehaviour 
{
    public RectTransform uiPelvis;
    public RectTransform uiBody;
    private Vector3 rotation;
    public Transform botTransform;
    public Transform bodyTransform;
    public Transform playerTransform;

    void OnEnable()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.Find("Player").transform;
        }
    }

    void Update()
    {
        if (botTransform != null && botTransform == PlayerController.botController.thisTransform)
        {
            rotation = new Vector3(0, 0, playerTransform.eulerAngles.y - botTransform.eulerAngles.y);
            uiPelvis.eulerAngles = rotation;

            rotation = new Vector3(0, 0, rotation.z - bodyTransform.localEulerAngles.y);
            uiBody.eulerAngles = rotation;
        }
        else
        {
            botTransform = PlayerController.botController.thisTransform;

            foreach (ModulBasys modul in PlayerController.botController.modulController)
            {
                if (modul.modulType == ModulType.Body)
                {
                    bodyTransform = modul.GetBodyModul().rotateTransform;
                    return;
                }
            }
        }
    }
}
