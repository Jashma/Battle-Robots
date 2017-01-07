using UnityEngine;
using UnityEngine.UI;

public class UI_Modul : UI_ModulBasys
{
    public Image healthImage;

    void OnEnable()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
            healthImage = transform.FindChild("HealthImage").GetComponent<Image>();
            modulText = GetComponentInChildren<Text>();
        }
    }

    void Update()
    {
        if (GetBotID() == true)
        {
            if (modulController == null)
            {
                modulController = FindModul();
                message = "";
                image.color = UI_Controller.Instance.hideColor;
                healthImage.color = UI_Controller.Instance.hideColor;
            }
            else
            {
                message = "";

                healthImage.color = ColorByValue(modulController.healthModul);
                

                if (CheckModulState(modulController) == true)
                {
                    image.color = UI_Controller.Instance.enableColor;
                }
                else
                {
                    image.color = UI_Controller.Instance.disableColor;
                }
            }

            SetText(message);
        }
    }
}
