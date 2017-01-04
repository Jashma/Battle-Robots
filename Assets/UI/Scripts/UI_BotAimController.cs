using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_BotAimController : MonoBehaviour 
{
    public ModulPosition gunPosition;//Позиция оружия относительно робота
    public Vector3 screenPosition;//Текущая позиция на экране
    //public GameObject gunCrosshairImage;
    public Image gunCrosshairImage;
    private Transform playerTransform;
    private Camera cameraComponent;
    private RectTransform rectTransform;
    private float aimWidth = 20;

    void OnEnable()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        cameraComponent = GameObject.Find("Player").GetComponentInChildren<Camera>();
        gunCrosshairImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        //rectTransform = gunCrosshairImage.GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        //Если игрок в состоянии сделования или управления ботом
        if (PlayerController.botController != null)
        {
            //Если Угол между взглядом игрока вперед и взглядом башни вперед меньше 60 (нужно, что бы не рисовать прицел, когда камера смотрит в другую сторону от направления оружия)
            if (Vector3.Angle(playerTransform.TransformDirection(Vector3.forward), PlayerController.botController.bodyRotation.transform.TransformDirection(Vector3.forward)) < 60)
            {
                for (int i = 0; i < PlayerController.botController.gunController.Count; i++)
                {
                    if (gunPosition == PlayerController.botController.gunController[i].gunPisition)
                    {
                        if (PlayerController.botController.gunController[i] != null && PlayerController.botController.gunController[i].weaponIsOn == true)
                        {
                            DrawGunCrosshair(PlayerController.botController.gunController[i].currentGunTarget);
                        }
                        else
                        {
                            imageDisable();
                        }
                    }
                }
            }
            else
            {
                imageDisable();
            }
        }
        else
        {
            imageDisable();
        }
    }

    /*
    void ChangeGun(GunController gunController)
    {
        if (gunController != null && gunController.weaponIsOn == true)
        {
            DrawGunCrosshair(gunController.currentGunTarget);
        }
        else
        {
            imageDisable();
        }
    }
    */

    void DrawGunCrosshair(Vector3 gunTarget)
    {
        if (gunCrosshairImage.enabled == true)
        {
            screenPosition = cameraComponent.WorldToScreenPoint(gunTarget);
            rectTransform.anchoredPosition = screenPosition;
        }
        else
        {
            gunCrosshairImage.enabled = true;
        }
    }

    void imageDisable()
    {
        if (gunCrosshairImage.enabled == true)
        {
            gunCrosshairImage.enabled = false; 
        }
    }

}
