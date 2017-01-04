using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_AddonModul : MonoBehaviour
{
    private Button enableBtn;
    private Text buttonText;
    public GameObject addonModulObj;

    void Start()
    {
        if (enableBtn == null)
        {
            enableBtn = GetComponent<Button>();
            enableBtn.onClick.AddListener(delegate { EnableAddonModul(); });

            buttonText = GetComponentInChildren<Text>();
        }

        if (addonModulObj.activeSelf == false)
        {
            buttonText.text = "Enable";
        }
        else
        {
            buttonText.text = "Disable";
        }
    }

    public void EnableAddonModul()
    {
        if (addonModulObj.activeSelf == false)
        {
            addonModulObj.SetActive(true);
            buttonText.text = "Disable";
        }
        else
        {
            addonModulObj.SetActive(false);
            buttonText.text = "Enable";
        }
    }

}
