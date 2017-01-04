using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum Team
{
    None,
    Red,
    Blue,
}

public class LevelController : MonoBehaviour 
{
    static LevelController instance = null;
    public static List<BotController> arrayBotControllerRed = new List<BotController>();
    public static List<BotController> arrayBotControllerBlue = new List<BotController>();
    public static List<BaseState> arrayBaseState = new List<BaseState>();
    public static List<FactoryState> arrayFactoryState = new List<FactoryState>();
    public static Terrain terrain;
    public static int maxBot = 5;
    //public Terrain currentTerrain;

    //Debug
    public int DB_maxBot;
    public static LevelController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (LevelController)FindObjectOfType(typeof(LevelController));
            }
            return instance;
        }
    }

    private GameObject ui;
    public int[] leavel;

    //Debug
    public List<BotController> botControllerRed;
    public List<BotController> botControllerBlue;

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        Instantiate(Resources.Load("Prefabs/User Interface/UI"));

        //Debug
        botControllerRed = arrayBotControllerRed;
        botControllerBlue = arrayBotControllerBlue;
        terrain = Terrain.activeTerrain;
        
    }

    void Update()
    {
        //Debug
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            if (Time.timeScale < 1.0f)
            {
                Time.timeScale = 1.0f;
            }

            Debug.Log("Time.timeScale= " + Time.timeScale);
        }

        if (Input.GetKeyUp(KeyCode.PageUp))
        {
            Time.timeScale += 0.01f;

            if (Time.timeScale > 1.0f)
            {
                Time.timeScale = 1.0f;
                
            }

            Debug.Log("Time.timeScale= " + Time.timeScale);
        }

        if (Input.GetKeyUp(KeyCode.PageDown))
        {
            Time.timeScale -= 0.01f;

            if (Time.timeScale < 0.01f)
            {
                Time.timeScale = 0.01f;

            }

            Debug.Log("Time.timeScale= " + Time.timeScale);
        }


        DB_maxBot = maxBot;
    }

    public void LoadLeavel(int Level)
    {
        GameObject.Find("UI").GetComponent<UI_Controller>().ChangeState("WaitConnect");
        StartCoroutine(LoadNextLevel(50, Level));
        Application.LoadLevel(1);
    }

    private IEnumerator LoadNextLevel(float time, int level)
    {
        yield return new WaitForSeconds(time * Time.deltaTime);
        Application.LoadLevel(level);

        if (level == 0)
        {
            GameObject.Find("UI").GetComponent<UI_Controller>().ChangeState("MainMenu");
        }

        if (level == 1)
        {
            GameObject.Find("UI").GetComponent<UI_Controller>().ChangeState("WaitConnect");
        }

        //Debug.Log("LoadLevel " + level);
    }

}
