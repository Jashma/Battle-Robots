using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UI_SoundConfigMenu : MonoBehaviour 
{
    public UI_ChangeSoundClip ui_ChangeSoundClip;
    public Text buttonText;

    public void isOnMenu()
    {
        
        ui_ChangeSoundClip.gameObject.SetActive(true);
        ShowChangeSoundPanel();
    }

    public void ShowChangeSoundPanel()
    {
        if (ui_ChangeSoundClip.gameObject.activeSelf == true)
        {
            ui_ChangeSoundClip.gameObject.SetActive(false);
            buttonText.text = "Change Audio Clip";
        }
        else
        {
            ui_ChangeSoundClip.gameObject.SetActive(true);
            ui_ChangeSoundClip.isOnMenu();
            buttonText.text = "Hide";
        }
    }
}
