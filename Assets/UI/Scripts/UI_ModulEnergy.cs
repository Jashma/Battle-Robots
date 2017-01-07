using UnityEngine;
//using System.Collections;
//using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ModulEnergy : UI_ModulBasys, IPointerClickHandler
{
    public GameObject radialImage;
    public bool startCrossfade;
    private float fadeSpeed = 0.5f;
    private float t;
    private bool fadeDown = true;
    private Color normalColor = Color.white;
    private Color highlightColorInitial = Color.white;
    private Color highlightColorFadeTo = Color.white;

    void OnEnable()
    {
        if (image == null)
        {
            highlightColorInitial.a = 0.5f;

            image = GetComponent<Image>();
            modulText = GetComponentInChildren<Text>();
            radialImage = transform.FindChild("RadialSliderImage").gameObject;
            radialImage.SetActive(false);
             
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
            }
            else
            {
                message = modulController.energyReloadQuoue.ToString("f0") + " %";
                //

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
            Crossfade();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SendMessageUpwards("SelectChangeModul", this);
        startCrossfade = true;
    }

    
    private void Crossfade()
    {
        if (startCrossfade == true)
        {
            if (t <= 1f)
            {
                t += Time.deltaTime / fadeSpeed;

                if (fadeDown)
                {
                    image.color = Color.Lerp(highlightColorInitial, highlightColorFadeTo, t);

                }
                else
                {
                    image.color = Color.Lerp(highlightColorFadeTo, highlightColorInitial, t);
                }
            }
            else
            {
                t = 0;
                fadeDown = !fadeDown;
            }
        }
    }
}
