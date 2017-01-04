using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public enum soundFormat
{
    wav,
    ogg,
    all,
}

public class UI_ChangeSoundClip : MonoBehaviour 
{
    public AudioSource audioSource;
    private AudioClip currentAudioClip;

    public Transform parentTransform;
    public GameObject prefabObj;
    public string pathToFolder = "Sound";
    public soundFormat atributFiles;
    public float otstup;
    public string pathToProject;
    public string pathToFile;
    public Vector3 startPosition = new Vector3(90, -35, 0);
    private GameObject[] fileObj = new GameObject[0];

    public string[] arrayFiles;
    public AudioClip[] audioClip;

    private DirectoryInfo dir;
    private FileInfo[] info;

	public void isOnMenu() 
    {
        audioSource = GetComponentInParent<AudioSource>();
        otstup = prefabObj.GetComponent<RectTransform>().sizeDelta.y;

        pathToProject = Application.dataPath + "/StreamingAssets/";
        dir = new DirectoryInfo(pathToProject + pathToFolder);
        info = dir.GetFiles("*." + atributFiles.ToString());
        arrayFiles = new string[info.Length];
        audioClip = new AudioClip[info.Length];
        fileObj = new GameObject[info.Length];

        parentTransform.GetComponent<RectTransform>().sizeDelta = Vector2.up * otstup * info.Length;

        StartCoroutine(LoadAudio(info));
	}

    IEnumerator LoadAudio(FileInfo[] info)
    {
        int index = 0;
        foreach (FileInfo f in info)
        {
            arrayFiles[index] = dir + "/" + Path.GetFileNameWithoutExtension(f.FullName);
            pathToFile = "file://" + arrayFiles[index] + "." + atributFiles.ToString();

            WWW www = new WWW(pathToFile);
            yield return www;

            if (www.error == null)
            {
                audioClip[index] = www.audioClip;
                fileObj[index] = Instantiate(prefabObj);
                fileObj[index].transform.SetParent(parentTransform);
                fileObj[index].transform.localPosition = startPosition + Vector3.down * otstup * index;
                fileObj[index].GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(f.FullName);
                fileObj[index].GetComponentInChildren<UI_SoundItem>().audioClip = audioClip[index];
                fileObj[index].GetComponentInChildren<UI_SoundItem>().ui_ChangeSoundClip = this;
            }
            else
            {
                Debug.Log("WWW Error: " + www.error);
            }

            index++;
        }
    }

    public void PlayChangeAudio(AudioClip audioClip)
    {
        StopChangeAudio();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void StopChangeAudio()
    {
        audioSource.Stop();
    }

    public void ApplyChangeAudio()
    {
        //GlobalBotConfig.stepAudioClip = audioClip;
    }

    void OnDisable()
    {
        for (int i = 0; i < fileObj.Length; i++)
        {
            StopChangeAudio();
            DestroyObject(fileObj[i]);
        }
    }
}
