using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_EnergyConfig : MonoBehaviour
{
    public UI_Modul[] ui_modul; 
    public ModulBasys currentModulList;
    public Slider changeEnergySlider;

    void Start ()
    {
        FindAllModul();
        
    }

    void FindAllModul()
    {
        ui_modul = GetComponentsInChildren< UI_Modul >();

        foreach (UI_Modul modul in ui_modul)
        {
            modul.changeModulEvent.AddListener(changeModulEvent);
        }
    }

    void changeModulEvent()
    {
        Debug.Log("message");
    }
}
