using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI_TeamPanelController : MonoBehaviour 
{
    public GameObject[] unitButtonObj;//Массив панелей имен ботов 
    public Text[] unitButtonText;//Массив Текста в панеле имен ботов
    public Image[] unitButtonImage;//Массив image в панеле имен ботов
    public Vector3 createPosition;//Позиция где создается панель бота
    public float otstup;//Отступ между панелями ботов
    public Color enableColor;
    public Color disableColor;

    private GameObject unitPrefabObj;//
    private RectTransform rectTransform;
    private Text nameMenuText;
    

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
    }

    void Update()
    {
        if (name == "TeamFriendPanel")
        {
            if (nameMenuText.text != PlayerController.playerTeam.ToString())
            {
                nameMenuText.text = PlayerController.playerTeam.ToString();
            }

            if (PlayerController.playerTeam == Team.Red)
            {
                DrawButtonText(LevelController.arrayBotControllerRed, true);
            }

            if (PlayerController.playerTeam == Team.Blue)
            {
                DrawButtonText(LevelController.arrayBotControllerBlue, true);
            }
        }

        if (name == "TeamEnemyPanel")
        {
            if (nameMenuText.text != PlayerController.enemyTeam.ToString())
            {
                nameMenuText.text = PlayerController.enemyTeam.ToString();
            }

            if (PlayerController.playerTeam == Team.Red)
            {
                DrawButtonText(LevelController.arrayBotControllerBlue);
            }

            if (PlayerController.playerTeam == Team.Blue)
            {
                DrawButtonText(LevelController.arrayBotControllerRed);
            }
        }
    }

    void CreatePrefabPanel()
    {
        unitButtonObj = new GameObject[LevelController.maxBot];//Размер массива равен максимальному кличеству ботов одной команды
        unitButtonText = new Text[LevelController.maxBot];
        unitButtonImage = new Image[LevelController.maxBot];

        //Проходим по массиву обьектов и создаем панельки ботов, делаем их дочерними к этому обьекту
        for (int i = 0; i < unitButtonObj.Length; i++)
        {
            unitButtonObj[i] = Instantiate(unitPrefabObj) as GameObject;
            unitButtonObj[i].transform.SetParent(transform);
            unitButtonObj[i].transform.localPosition = createPosition;
            createPosition = unitButtonObj[i].transform.localPosition + Vector3.down*otstup;
            unitButtonObj[i].SetActive(false);

            unitButtonText[i] = unitButtonObj[i].GetComponentInChildren<Text>();
            unitButtonImage[i] = unitButtonObj[i].GetComponent<Image>();
        }
    }

    void DrawButtonText(List<BotController> arrayBotController, bool check = false)
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

                    if (arrayBotController[i].botState == SM_BotState.Dead)//Если мертвый бот
                    { 
                        unitButtonText[i].color = disableColor;//Назначаем цвет имени бота "Выключен"
                    }
                    else
                    {
                        unitButtonText[i].color = enableColor;//Назначаем цвет имени бота "Включен"
                    }

                    //Здесь рисуем рамочку вокруг того бота, который управляется игроком
                    //Если эта панел команды игрока, то check будет true
                    if (check == true)
                    {
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
