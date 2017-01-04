using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UI_RadarController : MonoBehaviour 
{
    
    public RectTransform searchLineTransform;
    public GameObject[] botIconRed;
    public GameObject[] botIconBlue;
    public Vector3 rotate;
    private RectTransform maskTransform;
    private int nextArrayIndex;
    private Text radarDistanceText;
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
        botIcon = new GameObject[LevelController.maxBot];

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
        if (PlayerController.botController != null)
        {
            if (radarDistanceText.text != PlayerController.botController.radarController.radarLength.ToString())
            {
                radarDistanceText.text = PlayerController.botController.radarController.radarLength.ToString();
            }

            if (PlayerController.botController.radarController.startCheck == true)
            {
                searchLineTransform.gameObject.SetActive(true);
            }

            maskTransform.eulerAngles = new Vector3(0, 0, PlayerController.botController.transform.eulerAngles.y);

            if (PlayerController.botController.botState == SM_BotState.AiControl && PlayerController.botController.botState != SM_BotState.PlayerControl)
            {
                ControllBotIcon(botIconBlue, LevelController.arrayBotControllerBlue);
                ControllBotIcon(botIconRed, LevelController.arrayBotControllerRed);
            }
        }
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

                if (PlayerController.playerTeam == botController[i].team)
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
      
}
