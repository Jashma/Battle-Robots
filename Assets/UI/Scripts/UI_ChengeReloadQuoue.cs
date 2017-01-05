using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UI_ChengeReloadQuoue : MonoBehaviour
{
    public Slider[] modulSlider;
    public List<ModulBasys> modulList = new List<ModulBasys>();

    public float otstup = 35f;

    private Text energyOstatokText;

    private RectTransform selfRectTransform;
    private int activeSlider;
    private Vector2 sliderDefaultSize = new Vector2(30,60);

    //Debug
    public float[] oldValue;
    private bool lockHandler;
    const float maxSum = 100;

    void OnEnable()
    {
        if (selfRectTransform == null)
        {
            selfRectTransform = GetComponent<RectTransform>();
            modulSlider = GetComponentsInChildren<Slider>();
            energyOstatokText = transform.FindChild("EnergyOstatok").GetComponent<Text>();
        }

        FindModul();//Находим все нужные модули
        HideSlider();//Отключаем ненужные слайдеры и назначаем метод нужным слайдерам
        ValueChange(0);//Пересчитываем значения слайдера
        SetValueText();
        SetMenuSize();

    }

    void FindModul()
    {
        modulList.Clear();

        foreach (ModulBasys modul in PlayerController.botController.modulController)
        {
            modulList.Add(modul);
        }
    }

    void HideSlider()
    {
        activeSlider = 0;
        for (int i = 0; i< modulSlider.Length; i++ )
        {
            if (modulList.Count > i)
            {
                modulSlider[i].gameObject.SetActive(true);
                activeSlider++;
                modulSlider[i].onValueChanged.RemoveAllListeners();
                //modulSlider[i].value = modulList[i].energyReloadQuoue;

                int index = modulSlider[i].GetComponent<RectTransform>().GetSiblingIndex();

                modulSlider[i].onValueChanged.AddListener(delegate { ValueChange(index); });
                modulSlider[i].onValueChanged.AddListener(delegate { SetValueText(); });
                modulSlider[i].transform.FindChild("NameModul").GetComponent<Text>().text = modulList[i].information[0];

            }
            else
            {
                modulSlider[i].value = 0;
                modulSlider[i].onValueChanged.RemoveAllListeners();
                modulSlider[i].transform.FindChild("NameModul").GetComponent<Text>().text = "";
                modulSlider[i].transform.FindChild("LostEnergy").GetComponent<Text>().text = "";
                modulSlider[i].transform.FindChild("ValueEnergy").GetComponent<Text>().text = "";
                modulSlider[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetMenuSize()
    {
        selfRectTransform.sizeDelta = (Vector2.up * 350) + (Vector2.right * (otstup * (modulList.Count)));
    }

    public void SetValueText()
    {
        for (int i = 0; i < modulSlider.Length; i++)
        {
            if (modulSlider[i].gameObject.activeSelf == true)
            {
                modulSlider[i].transform.FindChild("LostEnergy").GetComponent<Text>().text = modulSlider[i].maxValue.ToString("f0");
                modulSlider[i].transform.FindChild("ValueEnergy").GetComponent<Text>().text = modulSlider[i].value.ToString("f0");
            }
        } 
    }

    public void ValueChange(int index)
    {
        if (lockHandler == true)
        {
            return;
        }
        else
        {
            if (GetSliderSum() <= maxSum)
            {
                return;
            }
            else
            {
                float sum = GetSliderSum() - modulSlider[index].value;
                float sumToDistribut = modulSlider[index].maxValue - modulSlider[index].value;
                float mLocalHash = sumToDistribut / sum;
                lockHandler = true;

                foreach (Slider slider in modulSlider)
                {
                    if (slider == modulSlider[index])
                    {
                        continue;
                    }
                    else
                    {
                        slider.value *= mLocalHash;
                    }
                }

                lockHandler = false;
            }
        }
    }

    float GetSliderSum()
    {
        float sum = 0;

        for (int i = 0; i < modulSlider.Length; i++)
        {
            if (modulSlider[i].gameObject.activeSelf == true)
            {
                sum += modulSlider[i].value;
            }
        }

        return sum;
    }

}
