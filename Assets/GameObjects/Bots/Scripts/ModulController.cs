using UnityEngine;
using System.Collections;

public class ModulController : MonoBehaviour
{
    public int changeModulIndex;
    public GameObject[] modulObj;
    public bool enableModul = false;

	void OnEnable()
    {
        changeModulIndex = Random.Range(0, modulObj.Length);

        ChangeModul(changeModulIndex);

    }

    public void ChangeModul(int index)
    {
        for (int i = 0; i < modulObj.Length; i++)
        {
            if (i == index)
            {
                modulObj[i].SetActive(enableModul);
            }
            else
            {
                modulObj[i].SetActive(false);
            }
        }
    }
}
