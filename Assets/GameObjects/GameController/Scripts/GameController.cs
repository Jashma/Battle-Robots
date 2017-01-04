using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Team
{
    None,
    Friend,
    Enemy,
}

public class GameController : MonoBehaviour
{
    public static List<BotController> arrayBotControllerFriendTeam = new List<BotController>();
    public static List<BotController> arrayBotControllerEnemyTeam = new List<BotController>();
    public static bool showHitInformation = true;
    public static bool showAimInformation = true;
    public static int maxBot = 5;
    public static float aimLerp = 0.1f;
    public static float distToHit = 500;
    public static float soundEffectsVolume = 1;
    public static GameController Instance { get; private set; }
    public static string mainMessage;

    //Debug
    public bool dbShowHitInformation;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UI_Controller.Instance.ChangeState(14);//"ChangeTeam"
    }

    void Update()
    {
        dbShowHitInformation = showHitInformation;

        if (Input.GetKey(KeyCode.Backspace))
        {
            Time.timeScale = 1;
            GameController.mainMessage = "";
        }
    }

    public RectTransform FindRectTransform(GameObject parentObj, string name)
    {
        RectTransform[] tmpTransform = parentObj.GetComponentsInChildren<RectTransform>();

        foreach (RectTransform tmp in tmpTransform)
        {
            if (tmp.name == name)
            {
                return tmp;
            }
        }

        return null;
    }
}
