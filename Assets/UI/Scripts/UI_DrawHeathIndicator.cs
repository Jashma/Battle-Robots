using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_DrawHeathIndicator : MonoBehaviour 
{
    public Sprite spriteEnemy;//Сприйт вражеской команды
    public Sprite spriteFriend;//Спрайт дружественной команды
    public Team team;
    public Transform botTransform;
    public float height;//Высота от координат бота
    public float health;
    private float imageSizeMax = 200;//Ширина спрайта
    private float imageSizeMin = 40;//Высота спрайта
    private float imageWidthCurrent;//Ширина спрайта
    private float imageHeightCurrent;//Высота спрайта
    private RectTransform healthImageTransform;
    
    private Vector3 position;
    private Vector2 screenPositionBot;
    private Vector2 screenPositionBotHeight;
    private Slider slider;

    //Debug
    public Vector3 pos1;
    public Vector3 pos2;
    public Vector2 screenPos1;
    public Vector2 screenPos2;

    void OnEnable()
    {
        healthImageTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();

        if (team == Team.Enemy)
        {
            GetComponent<Image>().sprite = spriteEnemy;
        }

        if (team == Team.Friend)
        {
            GetComponent<Image>().sprite = spriteFriend;
        }
    }

    void LateUpdate() 
    {
        //Debug
        height = botTransform.GetComponent<CharacterController>().height;
        //EndDebug
        DrawHelth();
    }

    void GetHelthIndicatorWidth()
    {
        imageWidthCurrent = screenPositionBotHeight.y - screenPositionBot.y;

        imageWidthCurrent = Mathf.Clamp(imageWidthCurrent, imageSizeMin, imageSizeMax);

        imageHeightCurrent = 10;// imageWidthCurrent * 0.1f;

        healthImageTransform.sizeDelta = new Vector2(imageWidthCurrent, imageHeightCurrent);
    }

    void DrawHelth()
    {
        position = botTransform.position;
        screenPositionBot = Camera.main.WorldToScreenPoint(position);

        position = botTransform.position + (Vector3.up * height);
        screenPositionBotHeight = Camera.main.WorldToScreenPoint(position);

        healthImageTransform.position = screenPositionBotHeight;
        slider.value = health;
    }
}
