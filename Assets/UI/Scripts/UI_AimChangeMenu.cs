using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public enum imageFormat
{
    png,
    jpg,
}

public class UI_AimChangeMenu : MonoBehaviour 
{
    public Transform parentTransform;
    public GameObject prefabObj;
    public string pathToFolder = "PlayerAimImage";
    public imageFormat atributFiles;
    public float otstup;
    public string pathToProject;
    public string pathToFile;
    public Vector3 startPosition = new Vector3(80, -35, 0);
    private GameObject[] fileObj;

    private string[] arrayFiles;
    private Texture[] textureClip;

    private DirectoryInfo dir;
    private FileInfo[] info;

    void OnEnable()
    {
        //Debug.Log("Enable");
        otstup = prefabObj.GetComponent<RectTransform>().sizeDelta.y;
        pathToProject = Application.dataPath + "/StreamingAssets/";
        dir = new DirectoryInfo(pathToProject + pathToFolder);
        info = dir.GetFiles("*." + atributFiles.ToString());
        arrayFiles = new string[info.Length];
        textureClip = new Texture[info.Length];
        fileObj = new GameObject[info.Length];
        parentTransform.GetComponent<RectTransform>().sizeDelta = Vector2.up * otstup * info.Length;

        StartCoroutine(LoadTextures(info));
    }

    IEnumerator LoadTextures(FileInfo[] info)
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
                textureClip[index] = www.texture;
                fileObj[index] = Instantiate(prefabObj);
                fileObj[index].transform.SetParent(parentTransform);
                fileObj[index].transform.localPosition = startPosition + Vector3.down * otstup * index;
                fileObj[index].transform.localScale = Vector3.one;
                fileObj[index].GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(f.FullName);
                fileObj[index].GetComponent<UI_AimItem>().aimImage.texture = textureClip[index];
            }
            else
            {
                Debug.Log("WWW Error: " + www.error);
            }

            index++;
        }
    }

    void OnDisable()
    {
        //Debug.Log("Disable");
        for (int i = 0; i<fileObj.Length; i++ )
        {
            DestroyObject(fileObj[i]);
        }
    }
}
