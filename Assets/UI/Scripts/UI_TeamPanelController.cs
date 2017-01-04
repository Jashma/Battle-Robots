using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI_TeamPanelController : MonoBehaviour 
{
    public GameObject[] unitButtonObj;//Массив панелей имен ботов 
    public Text[] unitButtonText;//Массив Текста в панеле имен ботов
    public Image[] unitButtonImage;//Массив image в панеле имен ботов
    public Vector3 createBotPosition = new Vector3(80, -40, 0);//Позиция где создается панель бота
    public float otstup = 30;//Отступ между панелями ботов
    public Team team;

    //private float panelWidth;
    private GameObject unitPrefabObj;//
    private RectTransform rectTransform;
    private Text nameMenuText;
    private List<BotController> botList;

    void OnEnable()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (unitPrefabObj == null)
        {
            unitPrefabObj = Resources.Load("Prefabs/User Interface/BotTeamHud") as GameObject;
        }

        if (unitButtonObj.Length == 0)
        {
            CreatePrefabPanel();
        }

        if (nameMenuText == null)
        {
            nameMenuText = transform.FindChild("NameTeamText").GetComponent<Text>();
        }

        if (team == Team.Friend)
        {
            botList = GameController.arrayBotControllerFriendTeam;
            nameMenuText.text = PlayerController.playerTeam.ToString();
        }

        if (team == Team.Enemy)
        {
            botList = GameController.arrayBotControllerEnemyTeam;
            nameMenuText.text = PlayerController.enemyTeam.ToString();
        }
    }

    void Update()
    {
        DrawButtonText(botList);
    }

    void CreatePrefabPanel()
    {
        unitButtonObj = new GameObject[GameController.maxBot];//Размер массива равен максимальному кличеству ботов одной команды
        unitButtonText = new Text[GameController.maxBot];
        unitButtonImage = new Image[GameController.maxBot];

        //Проходим по массиву обьектов и создаем панельки ботов, делаем их дочерними к этому обьекту
        for (int i = 0; i < unitButtonObj.Length; i++)
        {
            unitButtonObj[i] = Instantiate(unitPrefabObj) as GameObject;
            unitButtonObj[i].transform.SetParent(transform);
            unitButtonObj[i].transform.localPosition = createBotPosition;
            createBotPosition = unitButtonObj[i].transform.localPosition + Vector3.down*otstup;
            unitButtonObj[i].SetActive(false);

            unitButtonText[i] = unitButtonObj[i].GetComponentInChildren<Text>();
            unitButtonImage[i] = unitButtonObj[i].GetComponent<Image>();
        }

        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, otstup + otstup * unitButtonObj.Length);
    }

    void DrawButtonText(List<BotController> arrayBotController)
    {
        for (int i = 0; i < unitButtonObj.Length; i++)//Проходим по массиву обьектов
        {
            if (unitButtonObj[i] != null)//Если в массив назначена панель имени бота
            {
                if (i < arrayBotController.Count)//Если боты в массиве arrayBotController ещё не заончились
                {
                    if (unitButtonObj[i].activeSelf == false)
                    {
                        unitButtonObj[i].SetActive(true);//Включаем панель имени бота
                    }

                    unitButtonText[i].text = arrayBotController[i].name + "  " + i;//Назначаем имя

                    if (arrayBotController[i].botState == SM_BotState.Destroy)//Если мертвый бот
                    {
                        unitButtonText[i].color = UI_Controller.disableColor;//Назначаем цвет имени бота "Выключен"
                    }
                    else
                    {
                        unitButtonText[i].color = UI_Controller.enableColor;//Назначаем цвет имени бота "Включен"
                    }

                    //Здесь рисуем рамочку вокруг того бота, который управляется игроком
                    //Если эта панел команды игрока, то check будет true
                    if (PlayerController.botController != null)
                    {
                        if (PlayerController.botController == arrayBotController[i])
                        {
                            unitButtonImage[i].enabled = true;
                        }
                        else
                        {
                            unitButtonImage[i].enabled = false;
                        }
                    }
                    else
                    {
                        if (unitButtonImage[i].enabled == true)
                        {
                            unitButtonImage[i].enabled = false;
                        }
                    }
                }
                else
                {
                    //Если боты в массиве arrayBotController закончились, то отключаем все оставшиеся панели имен ботов
                    unitButtonText[i].text = "none";
                    unitButtonObj[i].SetActive(false);
                }
            }
        }
    }
}
