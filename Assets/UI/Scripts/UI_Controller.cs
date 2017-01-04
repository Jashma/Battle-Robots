using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum UI_State
{
    ConnectionMenu,
    ConfigClient,
    ConfigServer,
    InGame,
    ConfigMenu,
    ConfigAudioMenu,
    ConfigAimMenu,
    ConfigGraphicMenu,
    MainMenu,
    Exit,
    WaitConnect,
    PauseMenu,
    ActionMenu,
    Quit,
    DeploedMenu,
    ChangeTeam,
    ChangeLevel,
}

public class UI_Controller : MonoBehaviour 
{
    public static UI_Controller instance;
    
    public Transform[] uiMenuTransform;
    public static Transform currentMenuTransform;
    [HideInInspector]
    public Button currentButton;
    //[HideInInspector]
    public GameObject currentMouseOverGameObject;

    private bool isAxisInUse = false;
    public static UI_State uiState;
    public static float speedMoveMenu = 25;
    public static float speedScaleMenu = 0.01f;

    public static UI_Controller Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (UI_Controller)FindObjectOfType(typeof(UI_Controller));
            }
            return instance;
        }
    }

    //Debug
    public  UI_State currentUiState;
    public Transform currentMenu;

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

        name = "UI";
    }

	void Start() 
    {
        for (int i = 0; i < uiMenuTransform.Length; i++)
        {
            if (uiMenuTransform[i] != null)
            {
                uiMenuTransform[i].gameObject.SetActive(false);
            }
        }

        ChangeState("MainMenu");
	}

    public void ChangeState(string state = "")
    {
        if (state != "")
        {
            uiState = (UI_State)Enum.Parse(typeof(UI_State), state);

            if (currentMenuTransform != null)
            {
                currentMenuTransform.gameObject.SetActive(false);
            }

            for (int i = 0; i < uiMenuTransform.Length; i++)
            {
                if (uiMenuTransform[i].name == uiState.ToString())
                {
                    currentMenuTransform = uiMenuTransform[i];
                    currentMenuTransform.gameObject.SetActive(true);
                }
            }
        }


        if (UI_Controller.uiState == UI_State.Quit)
        {
            QuitApplication();
        }

        if (UI_Controller.uiState == UI_State.InGame)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        currentUiState = uiState;
        currentMenu = currentMenuTransform;
    }

    void Update()
    {
        if (Input.GetAxis("Cancel") != 0)
        {
            if (isAxisInUse == false)
            {
                if (uiState == UI_State.InGame)
                {
                    ChangeState("MainMenu");
                }

                if (uiState == UI_State.WaitConnect)
                {
                    ChangeState("ConnectionMenu");
                }
            }

            isAxisInUse = true;
            StartCoroutine(DisableAxis(50));
        }

        if (Input.GetAxis("Action") != 0)
        {
            if (isAxisInUse == false)
            {
                if (uiState == UI_State.InGame)
                {
                    ChangeState("ActionMenu");
                }
            }

            isAxisInUse = true;
            StartCoroutine(DisableAxis(50));
        }
        else
        {
            if (uiState == UI_State.ActionMenu)
            {
                ChangeState("InGame");
            }
        }
    }

    private IEnumerator DisableAxis(float time)
    {
        yield return new WaitForSeconds(time * Time.deltaTime);
        isAxisInUse = false;
    }

    private void QuitApplication()
    {
        //If we are running in a standalone build of the game
#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
