﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, ISubmitHandler, ISelectHandler
{
    
    public bool activeThisObj = true;
    public Color normalColor;
    public Color highlightColorInitial;
    public Color highlightColorFadeTo;
    public float fadeSpeed = 0.75f;
    private float t;
    private bool fadeDown = true;
    private bool changeObj = false;

    private Button button;
    private Slider slider;
    private Scrollbar scrollbar;

    public Image[] arrayImages = new Image[0];
    private Vector3 lastPointerPositon;

    void Start()
    {
        //Если ссылки не назначены в инспекторе, назначаем компоненты на всех дочерних обьектах
        if (arrayImages.Length == 0)
        {
            arrayImages = GetComponentsInChildren<Image>();
        }

        button = GetComponent<Button>();
        slider = GetComponent<Slider>();
        scrollbar = GetComponent<Scrollbar>();

        if (slider != null)
        {
            slider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });
        }

        if (scrollbar != null)
        {
            scrollbar.onValueChanged.AddListener(delegate { OnSliderValueChange(); });
        }
    }

   void Update()
    {
        if (activeThisObj == true)
        {
            if (UI_Controller.instance.currentMouseOverGameObject == gameObject && Input.mousePosition != lastPointerPositon)
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }

            arrayImageActive();
            lastPointerPositon = Input.mousePosition;
        }
    }
     
     public void OnPointerEnter(PointerEventData eventData)
    {
        if (activeThisObj == true)
        {
            UI_Controller.instance.currentMouseOverGameObject = gameObject;
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
      
    public void OnPointerExit(PointerEventData eventData)
    {
        if (activeThisObj == true)
        {
            UI_Controller.instance.currentMouseOverGameObject = null;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (activeThisObj == true)
        {
            UI_AudioManager.instance.PlayHoverSound();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (activeThisObj == true)
        {
            if (button == null)
            {
                return;
            }

            UI_Controller.instance.currentButton = button;
            UI_AudioManager.instance.PlayClickSound();
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (activeThisObj == true)
        {
            if (button == null)
            {
                return;
            }

            UI_Controller.instance.currentButton = button;
            UI_AudioManager.instance.PlayClickSound();
        }
    }

    public void OnSliderValueChange()
    {
        if (activeThisObj == true)
        {
            UI_AudioManager.instance.PlaySliderSound();
        }
    }

    private void arrayImageActive()
    {
        if (arrayImages.Length > 0)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                changeObj = true;

                for (int i = 0; i < arrayImages.Length; i++)
                {
                    if (t <= 1f)
                    {
                        t += Time.deltaTime / fadeSpeed;

                        if (fadeDown)
                        {
                            arrayImages[i].color = Color.Lerp(highlightColorInitial, highlightColorFadeTo, t);

                        }
                        else
                        {
                            arrayImages[i].color = Color.Lerp(highlightColorFadeTo, highlightColorInitial, t);
                        }


                    }
                    else
                    {
                        t = 0;
                        fadeDown = !fadeDown;
                    }
                }
            }
            else
            {
                if (changeObj == true)
                {
                    changeObj = false;
                    for (int i = 0; i < arrayImages.Length; i++)
                    {
                        arrayImages[i].color = normalColor;
                    }
                    
                }
            }
        }
    }
}