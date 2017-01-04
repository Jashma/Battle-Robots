using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_RadarController : MonoBehaviour 
{
    public RectTransform searchLineTransform;//Круг расширяющийся на экране радара
    public GameObject[] botIconRed;
    public GameObject[] botIconBlue;
    public Vector3 rotate;
    private RectTransform maskTransform;
    private int nextArrayIndex;
    private Text radarDistanceText;
    private ModulRadar radarController;

	void OnEnable () 
    {
        if (searchLineTransform == null)
        {
            foreach (RectTransform searchTransform in GetComponentsInChildren<RectTransform>())
            {
                if (searchTransform.name == "RadarLine")
                {
                    searchLineTransform = searchTransform;
                    searchLineTransform.gameObject.SetActive(false);
                }

                if (searchTransform.name == "RadarMask")
                {
                    maskTransform = searchTransform;
                }

                if (radarDistanceText == null)
                {
                    radarDistanceText = transform.FindChild("RadarDistanceText").GetComponent<Text>();
                }
            }

            botIconRed = CreateBotIcon(botIconRed, Color.red);
            botIconBlue = CreateBotIcon(botIconBlue, Color.blue);
        }
	}

    //Создаем иконки ботов на радаре
    GameObject[] CreateBotIcon(GameObject[] botIcon, Color color)
    {
        botIcon = new GameObject[GameController.maxBot];

        for (int i=0; i< botIcon.Length; i++)
        {
            botIcon[i] = Instantiate(Resources.Load("Prefabs/User Interface/RadarBotIcon") as GameObject);
            botIcon[i].transform.position = Vector3.zero;
            botIcon[i].transform.SetParent(maskTransform);
            botIcon[i].GetComponent<UI_RadarBotIcon>().color = color;
            botIcon[i].SetActive(false);
            
        }

        return botIcon;
    }


    void Update()
    {
        if (radarController == null)
        {
            foreach (ModulBasys modul in PlayerController.botController.modulController)
            {
                if (modul.modulType == ModulType.Radar)
                {
                    radarController = modul.GetRadarController();
                    return;
                }
            }
        }
        else
        {
            if (radarDistanceText.text != radarController.radarLength.ToString())
            {
                radarDistanceText.text = radarController.radarLength.ToString();
            }

            if (radarController.startCheck == true)
            {
                searchLineTransform.gameObject.SetActive(true);
            }

            maskTransform.eulerAngles = new Vector3(0, 0, PlayerController.botController.transform.eulerAngles.y);
        }

        ControllBotIcon(botIconBlue, GameController.arrayBotControllerFriendTeam);
        ControllBotIcon(botIconRed, GameController.arrayBotControllerEnemyTeam);

    }
    
    void ControllBotIcon(GameObject[] botIcon, List<BotController> botController)
    {
        for (int i = 0; i < botIcon.Length; i++)
        {
            if (i < botController.Count)
            {
                if ("RadarIcon " + botIcon[i].name != botController[i].gameObject.name)
                {
                    botIcon[i].name = "RadarIcon " + botController[i].gameObject.name;
                }

                if (botController[i].team == Team.Friend)
                {
                    botIcon[i].SetActive(true);

                    Vector3 playerPosition = new Vector3(PlayerController.botController.transform.position.x, PlayerController.botController.transform.position.z, 0);
                    Vector3 otherPosition = new Vector3(botController[i].transform.position.x, botController[i].transform.position.z, 0);

                    botIcon[i].transform.localPosition = otherPosition - playerPosition; 
                }
                else
                {
                    if (botController[i].timeRadarFound > Time.time)
                    {
                        botIcon[i].SetActive(true);

                        Vector3 playerPosition = new Vector3(PlayerController.botController.transform.position.x, PlayerController.botController.transform.position.z, 0);
                        Vector3 otherPosition = new Vector3(botController[i].transform.position.x, botController[i].transform.position.z, 0);

                        botIcon[i].transform.localPosition = otherPosition - playerPosition;
                    }
                   else
                    {
                        botIcon[i].SetActive(false);
                    }
                }
            }
            else
            {
                botIcon[i].SetActive(false);
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (radarController != null)
        {
            radarController.AboutThis();
        }
        else
        {
            Debug.Log("No modul controller");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_MouseInformation.Instance.ClearMessage();
    }
}
