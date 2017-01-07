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
    PreferenceMenu,
    ConfigAudioMenu,
    ConfigAimMenu,
    ConfigGraphicMenu,
    MainMenu,
    Exit,
    WaitConnect,
    PauseMenu,
    Quit,
    DeploedMenu,
    ChangeTeam,
    ChangeLevel,
    GameConfig,
    ChangeModulMenu,
}

public enum UI_InGameMenu
{
    None,
    ActionMenu,
    ChangeReloadEnergyQuoue,
    BigModulInterface,
    Disable,
}

public class UI_Controller : MonoBehaviour 
{
    public static Transform currentMenuTransform;
    public Color enableColor;
    public Color disableColor;
    public Color hideColor;
    public Transform sceneObjTransform;
    public Transform[] uiMenuTransform;
    [HideInInspector]
    public Button currentButton;
    //[HideInInspector]
    public GameObject currentMouseOverGameObject;
    public Color elementMenuColor;
    private bool useAxisEsc = false;
    private bool useAxisAction = false;

    public static UI_State uiState;
    public static UI_InGameMenu uiInGameMenu;
    public static float speedMoveMenu = 25;
    public static float speedScaleMenu = 0.01f;

    //Debug
    public UI_State currentUiState;
    public UI_InGameMenu currentUiInGameMenu;
    public Transform currentMenu;
    public static UI_Controller Instance { get; private set; }


    void Awake()
    {
        name = "UI";
        DontDestroyOnLoad(gameObject);
        Instance = this;
        DisableAllMenu();//DisableAllMenu
        ChangeState(8);//MainMenu
    }

    void DisableAllMenu()
    {
        for (int i = 0; i < uiMenuTransform.Length; i++)
        {
            {
                uiMenuTransform[i].gameObject.SetActive(false);
            }
        }
    }

    public void ChangeState(int nextState)
    {
        uiState = (UI_State)nextState;

        for (int i = 0; i < uiMenuTransform.Length; i++)
        {
            if (uiMenuTransform[i].name == uiState.ToString())
            {
                currentMenuTransform = uiMenuTransform[i];
                currentMenuTransform.gameObject.SetActive(true);
            }
            else
            {
                uiMenuTransform[i].gameObject.SetActive(false);
            }
        }
        
        if (UI_Controller.uiState == UI_State.Quit)
        {
            QuitApplication();
        }

        currentMenu = currentMenuTransform;
    }

    public void ChangeActionMenuState(int nextState)
    {
        uiInGameMenu = (UI_InGameMenu)nextState;
    }

    public void ShowCursor(bool show)
    {
        Cursor.visible = show;

        if (show == true)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        UseAxisEsc();
        UseAxisAction();

        //Debug
        currentUiInGameMenu = uiInGameMenu;
        currentUiState = uiState;
    }

    void UseAxisEsc()
    {
        if (Input.GetAxis("Cancel") > 0)
        {
            if (useAxisEsc == false)
            {
                useAxisEsc = true;

                if (uiState == UI_State.InGame)
                {
                    ChangeState(8);//"MainMenu"
                    return;
                }

                if (uiState == UI_State.MainMenu)
                {
                    ChangeState(3);//"MainMenu"
                    return;
                }

                if (uiState == UI_State.WaitConnect)
                {
                    ChangeState(0);//"ConnectionMenu"
                    return;
                }
            }
        }
        else
        {
            useAxisEsc = false;
        }
    }

    void UseAxisAction()
    {
        if (Input.GetAxis("Action") != 0)
        {
            if (useAxisAction == false)
            {
                useAxisAction = true;

                if (uiState == UI_State.InGame)
                {
                    if (uiInGameMenu == UI_InGameMenu.None)
                    {
                        ChangeActionMenuState(1);//ActionMenu
                    }
                }
            }
            else
            {
                useAxisAction = false;
            }
        }
        else
        {
            //if (uiInGameMenu == UI_InGameMenu.ActionMenu)
            {
                //ChangeActionMenuState(0);//None
            }

            ChangeActionMenuState(0);//None
        }
    }

    public Transform GetTransform(string nameMenu = "")
    {
        Transform returnTransform = null;

        foreach (Transform transform in uiMenuTransform)
        {
            if (transform.name == nameMenu)
            {
                returnTransform = transform;
            }
        }

        return returnTransform;
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
