using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_BotAimController : MonoBehaviour 
{
    public ModulType modulType;//Позиция оружия относительно робота
    public Vector3 screenPosition;//Текущая позиция на экране
    public Image gunCrosshairImage;
    private Transform playerTransform;
    private Camera cameraComponent;
    private RectTransform rectTransform;
    private float aimWidth = 20;
    private Text aimTargetText;
    private Vector2 oldAimPosition;
    private Vector2 nextAimPosition;
    private ModulBasys modulGun;

    void OnEnable()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        cameraComponent = GameObject.Find("Player").GetComponentInChildren<Camera>();
        gunCrosshairImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        aimTargetText = GetComponentInChildren<Text>();
    }

    void LateUpdate()
    {
        if (modulGun == null)
        {
            gunCrosshairImage.enabled = false;
            modulGun = FindModul();
        }
        else
        {
            DrawGunCrosshair(modulGun.GetGunTarget());
            ShowAimTargetText();

            //Debug.DrawLine(modulGun.thisTransform.position, modulGun.thisTransform.TransformPoint(Vector3.forward * 100));
        }
    }

    private ModulBasys FindModul()
    {
        foreach (ModulBasys modul in PlayerController.botController.modulController)
        {
            if (modulType == modul.modulType)
            {
                return modul;
            }
        }

        return null;
    }

    void DrawGunCrosshair(Vector3 gunTarget)
    {
        if (CheckWeaponAngle() == true)
        {
            gunCrosshairImage.enabled = true;
            AimLerp(gunTarget);
        }
        else
        {
            gunCrosshairImage.enabled = false;
        }
    }

    bool CheckWeaponAngle()
    {
        if (Vector3.Angle(PlayerController.playerTransform.TransformPoint(Vector3.forward * 100), modulGun.thisTransform.TransformPoint(Vector3.forward * 100)) > 35)
        {
            return false;
        }

        return true;
    }

    void ShowAimTargetText()
    {
        if (gunCrosshairImage.enabled == true && GameController.showAimInformation == true)
        {
            aimTargetText.enabled = true;
            aimTargetText.text = modulGun.GetSupportMessage();
        }
        else
        {
            aimTargetText.enabled = false;
        }
    }

    void AimLerp(Vector3 gunTarget)
    {
        oldAimPosition = screenPosition;
        nextAimPosition = cameraComponent.WorldToScreenPoint(gunTarget);
        screenPosition = new Vector2(Mathf.Lerp(oldAimPosition.x, nextAimPosition.x, GameController.aimLerp), Mathf.Lerp(oldAimPosition.y, nextAimPosition.y, GameController.aimLerp));
        rectTransform.position = screenPosition;
    }

}
