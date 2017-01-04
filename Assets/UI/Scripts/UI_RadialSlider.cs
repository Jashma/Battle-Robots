using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_RadialSlider : MonoBehaviour
{
    public List<ModulBasys> modulList = new List<ModulBasys>();
    public Text freeValueEnergy;
    public GameObject changeEnergyObj;
    public Image[] slider;

    private bool lockHandler;
    const float maxSum = 100;

    private bool isPointerDown = false;
    private GraphicRaycaster raycaster;
    private Vector2 mousePosition;
    private float angle;

    void OnEnable()
    {
        raycaster = GetComponentInParent<GraphicRaycaster>();

        EnableAllSlider();
        FindModul();
        Show_HideSlider();
        ReCalculate(0);
        SetValueText();
    }

    void FindModul()
    {
        modulList.Clear();

        for (int i = 0; i < slider.Length; i++)
        {
            modulList.Add(FindModulType(slider[i]));
        }
    }

    void EnableAllSlider()
    {
        for (int i = 0; i < slider.Length; i++) { slider[i].transform.parent.gameObject.SetActive(true); }
    }

    ModulBasys FindModulType(Image slider)
    {
        ModulType modulType = slider.GetComponentInParent<UI_Modul>().modulType;

        foreach (ModulBasys modul in PlayerController.botController.modulController)
        {
            if (modul.modulType == modulType)
            {
                return modul;
            }
        }

        return null;
    }

    void Show_HideSlider()
    {
        for (int i = 0; i < slider.Length; i++)
        {
            if (modulList.Count > i)
            {
                if (modulList[i] != null && modulList[i].useEnergy == true)
                {
                    slider[i].fillAmount = modulList[i].energyReloadQuoue / 100;
                }
                else
                {
                    slider[i].fillAmount = 0;
                    slider[i].transform.parent.gameObject.SetActive(false);
                }
            }
            else
            {
                slider[i].fillAmount = 0;
                slider[i].gameObject.SetActive(false);
            }
        }
    }

    void SetValueText()
    {
        freeValueEnergy.text = (100 - GetSliderSum()).ToString("f0");

        for (int i = 0; i < slider.Length; i++)
        {
            slider[i].GetComponentInChildren<Text>().text = (slider[i].fillAmount * 100).ToString("f0");

            if (modulList[i] != null)
            {
                modulList[i].energyReloadQuoue = slider[i].fillAmount * 100;
                slider[i].GetComponentInChildren<Text>().text = modulList[i].EnergyPower.ToString("f0");
            }
        }
    }

    void ReCalculate(int index)
    {
        if (lockHandler == false)
        {
            if (GetSliderSum() <= maxSum)
            {
                return;
            }
            else
            {
                float sum = GetSliderSum() - (slider[index].fillAmount * 100);
                float sumToDistribut = maxSum - (slider[index].fillAmount * 100);
                float mLocalHash = sumToDistribut / sum;
                lockHandler = true;

                foreach (Image nextSlider in slider)
                {
                    if (nextSlider == slider[index])
                    {
                        continue;
                    }
                    else
                    {
                        nextSlider.fillAmount *= mLocalHash;
                    }
                }

                lockHandler = false;
            }
        }
    }

    float GetSliderSum()
    {
        float sum = 0;

        for (int i = 0; i < slider.Length; i++)
        {
            if (slider[i].gameObject.activeSelf == true)
            {
                sum += slider[i].fillAmount * 100;
            }
        }

        return sum;
    }

    public void ValueChange(int index)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(slider[index].transform as RectTransform, Input.mousePosition, raycaster.eventCamera, out mousePosition);

        angle = (Mathf.Atan2(-mousePosition.y, mousePosition.x) * 180f / Mathf.PI + 180f) / 360f;

        angle = Mathf.Clamp(angle, 0, modulList[index].energyMaxValue * 0.01f);

        slider[index].fillAmount = angle;

        ReCalculate(index);
        SetValueText();
    }
}
