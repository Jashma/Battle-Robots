using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ModulInformation : MonoBehaviour
{
    public GameObject informationObj;
    public Text nameModul;
    public Text armoreValue;
    public Text coastValue;
    public Text aboutModul;
    public ModulBasys modulBasys;

    private Transform parentTransform;
    private Transform modulAddonListTransform;
    private GameObject[] modulAddonButtonObj;
    private float otstup;
    private Vector3 startPosition;

    void OnEnable()
    {
        informationObj = transform.FindChild("Information").gameObject;
        nameModul = informationObj.transform.FindChild("NameModul").GetComponent<Text>();
        armoreValue = informationObj.transform.FindChild("ArmoreModulValue").GetComponent<Text>();
        coastValue = informationObj.transform.FindChild("CoastModulValue").GetComponent<Text>();
        aboutModul = informationObj.transform.FindChild("AboutModul").GetComponent<Text>();
        modulAddonListTransform = transform.FindChild("ModulAddonList");
        informationObj.SetActive(false);
    }

    public void ShowInformation()
    {
        if (modulBasys == null)
        {
            nameModul.text = "";
            armoreValue.text = "";
            coastValue.text = "";
            aboutModul.text = "";
            DestroyUIObj(modulAddonButtonObj);
            informationObj.SetActive(false);
        }
        else
        {
            informationObj.SetActive(true);
            nameModul.text = modulBasys.information[0];
            armoreValue.text = modulBasys.armoreModul.ToString();
            coastValue.text = modulBasys.coast.ToString();
            aboutModul.text = modulBasys.information[0];
            CreateModulAddonButton();
            
        }
    }

    void CreateModulAddonButton()
    {
        startPosition = new Vector3(30, -15, 0);
        otstup = 70;

        DestroyUIObj(modulAddonButtonObj);

        modulAddonButtonObj = new GameObject[modulBasys.modulAddOn.Length];//Длину массива определяем по количеству доп обьектов на модуле
        Transform parentTransform = SearchParentTransform("Content", transform.FindChild("Information").gameObject);

        for (int i = 0; i < modulAddonButtonObj.Length; i++)
        {
            GameObject tmpModulObj = Instantiate(Resources.Load("Prefabs/User Interface/UI_EnableAddOnModul")) as GameObject;
            tmpModulObj.transform.parent = parentTransform;
            tmpModulObj.transform.localPosition = startPosition + Vector3.right * otstup * i;
            tmpModulObj.transform.localScale = Vector3.one;
            tmpModulObj.GetComponent<UI_AddonModul>().addonModulObj = modulBasys.modulAddOn[i];
            modulAddonButtonObj[i] = tmpModulObj;
        }

    }

    Transform SearchParentTransform(string nameParentObj, GameObject StartSearchObj)
    {
        //Debug.Log(StartSearchObj);

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

    void OnDisable()
    {
        DestroyUIObj(modulAddonButtonObj);
    }
}
