using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_DrawHeathIndicator : MonoBehaviour 
{
    public Color redColor;
    public Color blueColor;
    public Sprite helthSpriteRed;
    public Sprite helthSpriteBlue;
    public Image helthImage;
    private PlayerController playerController;
    private BotController botController;
    private Camera cameraComponent;
    private float imageWidthHealth;
    private float imageHeightHealth;
    private RectTransform healthImageTransform;
    private float height;
    private Vector3 tmpPosition;
    private Vector3 screenPosition;
    private Slider slider;

    void OnEnable()
    {
        cameraComponent = GameObject.Find("Player").GetComponentInChildren<Camera>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        botController = GetComponentInParent<UI_BotIndicator>().botController;
        healthImageTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();

        if (botController.team == Team.Red)
        {
            //helthImage.color = redColor;
            helthImage.sprite= helthSpriteRed;
        }

        if (botController.team == Team.Blue)
        {
            //helthImage.color = blueColor;
            helthImage.sprite = helthSpriteBlue;
        }
    }

    void LateUpdate()
    {
        DrawHelth();
    }

    void DrawHelth()
    {
        imageWidthHealth = 100 - Vector3.Distance(playerController.transform.position, botController.transform.position) * 1f;
        imageHeightHealth = 10 - Vector3.Distance(playerController.transform.position, botController.transform.position) * 0.1f;
        healthImageTransform.sizeDelta = new Vector2(imageWidthHealth, imageHeightHealth);
        height = 1 + botController.characterController.height / 1.5f;
        tmpPosition = new Vector3(botController.transform.position.x,
                                  botController.transform.position.y + height,
                                  botController.transform.position.z);
        screenPosition = cameraComponent.WorldToScreenPoint(tmpPosition);
        healthImageTransform.position = screenPosition;
        slider.value = botController.health;
    }
}
