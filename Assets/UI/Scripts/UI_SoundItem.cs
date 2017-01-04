using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_SoundItem : MonoBehaviour 
{
    public AudioClip audioClip;
    public UI_ChangeSoundClip ui_ChangeSoundClip;
    public Slider slider;

    void Start()
    {
        slider.value = 0;
    }

    public void PlaySound()
    {
        ui_ChangeSoundClip.PlayChangeAudio(audioClip);
        slider.maxValue = audioClip.length;
        slider.value = 0;
        StartCoroutine(PlayAudio());
    }

    IEnumerator PlayAudio()
    {
        while(ui_ChangeSoundClip.audioSource.isPlaying == true)
        {
            slider.value = ui_ChangeSoundClip.audioSource.time;

            if (slider.value == audioClip.length)
            {
                slider.value = 0;
            }

            yield return null;
        }
        
    }
}
