using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_MainMenu : MonoBehaviour 
{
    public Button startGame;
    public Button exitGame;

    void OnEnable()
    {
        exitGame.onClick.RemoveAllListeners();
        startGame.onClick.RemoveAllListeners();

        if (PlayerController.inGame == true)
        {
            startGame.GetComponentInChildren<Text>().text = "Return";
            startGame.onClick.AddListener(delegate { ReturnToGame(); });

            exitGame.GetComponentInChildren<Text>().text = "Exit";
            exitGame.onClick.AddListener(delegate { ExitGame(); });
        }
        else
        {
            startGame.GetComponentInChildren<Text>().text = "StartGame";
            startGame.onClick.AddListener(delegate { StartGame(); });

            exitGame.GetComponentInChildren<Text>().text = "Quit";
            exitGame.onClick.AddListener(delegate { Quit(); });
        }
    }

    private void StartGame()
    {
        //Debug.Log("Click button StartGame");
        GetComponentInParent<UI_Controller>().ChangeState(0);//"ConnectionMenu"

    }

    private void Quit()
    {
        //Debug.Log("Click button Quit");
        GetComponentInParent<UI_Controller>().ChangeState(12);//"Quit"

    }

    private void ReturnToGame()
    {
        //Debug.Log("Click button ReturnToGame");
        GetComponentInParent<UI_Controller>().ChangeState(3);//"InGame"

    }

    private void ExitGame()
    {
        //Debug.Log("Click button ExitGame");
        PlayerController.inGame = false;
        GetComponentInParent<UI_Controller>().ChangeState(8);//"MainMenu"

    }
}
