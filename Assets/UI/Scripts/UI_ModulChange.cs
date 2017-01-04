using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ModulChange : MonoBehaviour, IPointerClickHandler //ISelectHandler, //IPointerEnterHandler, IPointerExitHandler, ISubmitHandler//
{
    public GameObject modulObj;
    private ModulController modulController;
    private ModulBasys modulBasys;
    private Button buyModul;

    private Text nameModul;
    private Text coasModul;
    private Text coasValue;
    public UI_ModulChangeMenu UI_ModulChangeMenu;
    private UI_ModulInformation UI_ModulInformation;
    public int index;


    public void EnableThis()
    {
        modulController = modulObj.GetComponentInParent<ModulController>();
        modulBasys = modulObj.GetComponent<ModulBasys>();
        UI_ModulInformation = UI_ModulChangeMenu.transform.FindChild("ModulInformation").GetComponent<UI_ModulInformation>();

        if (modulObj.activeSelf == true)
        {
            UI_ModulInformation.modulBasys = modulBasys;
            UI_ModulInformation.ShowInformation();
        }

        nameModul = transform.FindChild("Name").GetComponent<Text>();
        coasModul = transform.FindChild("Coast").GetComponent<Text>();
        coasValue = transform.FindChild("Coast value").GetComponent<Text>();

        buyModul = transform.FindChild("BuyButton").GetComponent<Button>();
        buyModul.onClick.AddListener(delegate { ChangeModul(); });
        
        
        nameModul.text = modulBasys.information[0];
        coasValue.text = modulBasys.coast.ToString();

        if (modulBasys.modulStatus == ModulStatus.None)
        {
            buyModul.gameObject.SetActive(true);
            coasModul.text = "Coast";
            coasValue.gameObject.SetActive(true);

        }
        else
        {
            buyModul.gameObject.SetActive(false);
            coasModul.text = "Enter";
            coasValue.gameObject.SetActive(false);
            
        }
    }

    void ChangeModul()
    {
        foreach (GameObject modul in modulController.modulObj)
        {
            if (modul == modulObj)
            {
                modul.SetActive(true);
            }
            else
            {
                modul.SetActive(false);
            }
        }

        modulObj.GetComponentInParent<BotController>().ActivateModul();
        UI_ModulChangeMenu.EnableChangeModulList(index);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UI_ModulInformation.modulBasys = modulBasys;
        UI_ModulInformation.ShowInformation();
    }

    /*
    public void OnSubmit(BaseEventData eventData)
    {
        UI_ModulInformation.modulBasys = modulBasys;
        UI_ModulInformation.ShowInformation();
    }

    public void OnSelect(BaseEventData eventData)
    {
        UI_ModulInformation.modulBasys = modulBasys;
        UI_ModulInformation.ShowInformation();
    }

    
 public void OnPointerEnter(PointerEventData eventData)
 {
     UI_ModulInformation.modulBasys = modulBasys;
     UI_ModulInformation.ShowInformation();
 }

 public void OnPointerExit(PointerEventData eventData)
 {
     UI_ModulInformation.modulBasys = null;
     UI_ModulInformation.ShowInformation();
 }
 */
}
