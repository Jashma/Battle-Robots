using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ModulChangeMenu : MonoBehaviour
{
    private float otstup;
    private Vector3 startPosition;
    private ModulController[] modulControllerArray;
    private GameObject[] modulControllerButtonObj;
    private GameObject[] modulChangeButtonObj;
    private Button returnBtn;

    void OnEnable()
    {
        if (returnBtn == null)
        {
            returnBtn = transform.FindChild("ReturnButton").GetComponent<Button>();
            returnBtn.onClick.RemoveAllListeners();
            returnBtn.onClick.AddListener(delegate { PlayerController.botController.ChangeAction(); });//"None"
            returnBtn.onClick.AddListener(delegate { PlayerController.Instance.ChangePlayerState(3); });//"ChangeModul"
            returnBtn.onClick.AddListener(delegate { UI_Controller.Instance.ChangeState(3); });//"InGame"
        }

        //Находим все классы ModulController и собираем их в массиве
        modulControllerArray = PlayerController.botController.GetComponentsInChildren<ModulController>();
        CreateModulControllerButton(SearchParentTransform("Content", transform.FindChild("ModulControllerList").gameObject));
    }

    Transform SearchParentTransform(string nameParentObj, GameObject StartSearchObj)
    {
        Transform parent = null;

        foreach (Transform searchTransform in StartSearchObj.GetComponentsInChildren<Transform>())
        {
            if (searchTransform.name == nameParentObj)
            {
                parent = searchTransform;
            }
        }

        return parent;
    }

    void DestroyUIObj(GameObject[] modulObj)
    {
        if (modulObj != null)
        {
            foreach (GameObject obj in modulObj)
            {
                Destroy(obj);
            }
        }
    }

    void CreateModulControllerButton(Transform parentTransform)
    {
        otstup = 65;
        startPosition = new Vector3(35, -25, 0);

        RectTransform rectTransform = transform.FindChild("ModulControllerList").GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(otstup * modulControllerArray.Length + 5, rectTransform.sizeDelta.y);

        modulControllerButtonObj = new GameObject[modulControllerArray.Length]; 

        for (int i = 0; i < modulControllerArray.Length; i++)
        {
            GameObject tmpModulObj = Instantiate(Resources.Load("Prefabs/User Interface/UI_Modul")) as GameObject;
            int index = i;
            tmpModulObj.transform.SetParent(parentTransform);
            tmpModulObj.transform.localPosition = startPosition + Vector3.right * otstup * i;
            tmpModulObj.transform.localScale = Vector3.one;
            tmpModulObj.GetComponentInChildren<Text>().text = modulControllerArray[i].transform.name;
            modulControllerButtonObj[i] = tmpModulObj;

            tmpModulObj.GetComponent<Button>().onClick.RemoveAllListeners();
            tmpModulObj.GetComponent<Button>().onClick.AddListener(delegate { EnableChangeModulList(index); });
        }
    }

    public void EnableChangeModulList(int index)
    {
        startPosition = new Vector3(120, -70, 0);
        otstup = 80;

        DestroyUIObj(modulChangeButtonObj);

        modulChangeButtonObj = new GameObject[modulControllerArray[index].modulObj.Length];
        Transform parentTransform = SearchParentTransform("Content", transform.FindChild("ModulChangeList").gameObject);

        for (int i = 0; i < modulControllerArray[index].modulObj.Length; i++)
        {
            GameObject tmpModulObj = Instantiate(Resources.Load("Prefabs/User Interface/UI_ModulChange")) as GameObject;
            //tmpModulObj.transform.parent = parentTransform;
            tmpModulObj.transform.SetParent(parentTransform);
            tmpModulObj.transform.localPosition = startPosition + Vector3.down * otstup * i;
            tmpModulObj.transform.localScale = Vector3.one;
            tmpModulObj.GetComponent<UI_ModulChange>().modulObj = modulControllerArray[index].modulObj[i];
            
            tmpModulObj.GetComponent<UI_ModulChange>().UI_ModulChangeMenu = this;
            tmpModulObj.GetComponent<UI_ModulChange>().index = index;

            tmpModulObj.GetComponent<UI_ModulChange>().EnableThis();

            modulChangeButtonObj[i] = tmpModulObj;
        }

    }

    void OnDisable()
    {
        DestroyUIObj(modulControllerButtonObj);
        DestroyUIObj(modulChangeButtonObj);
    }

}
