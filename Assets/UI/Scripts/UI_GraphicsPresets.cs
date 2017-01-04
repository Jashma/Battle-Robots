using UnityEngine;
using UnityEngine.Events;

public class UI_GraphicsPresets : MonoBehaviour 
{
    [HideInInspector]
    public UnityEvent FastestPresetEvent;
    [HideInInspector]
    public UnityEvent FastPresetEvent;
    [HideInInspector]
    public UnityEvent SimplePresetEvent;
    [HideInInspector]
    public UnityEvent GoodPresetEvent;
    [HideInInspector]
    public UnityEvent BeautifulPresetEvent;
    [HideInInspector]
    public UnityEvent FantasticPresetEvent;

    void Awake()
    {
        FastestPresetEvent = new UnityEvent();
        FastPresetEvent = new UnityEvent();
        SimplePresetEvent = new UnityEvent();
        GoodPresetEvent = new UnityEvent();
        BeautifulPresetEvent = new UnityEvent();
        FantasticPresetEvent = new UnityEvent();
    }

    void Start()
    {
        QualitySettings.SetQualityLevel(1);
        SetGraphicsPreset((int)QualitySettings.currentLevel);
    }

    public void SetGraphicsPreset(float value)
    {
        switch ((int)value)
        {
            case 0:
                //Debug.Log("FastestPresetEvent");
                FastestPresetEvent.Invoke();
                break;
            case 1:
                //Debug.Log("FastPresetEvent");
                FastPresetEvent.Invoke();
                break;
            case 2:
                //Debug.Log("SimplePresetEvent");
                SimplePresetEvent.Invoke();
                break;
            case 3:
                //Debug.Log("GoodPresetEvent");
                GoodPresetEvent.Invoke();
                break;
            case 4:
                //Debug.Log("BeautifulPresetEvent");
                BeautifulPresetEvent.Invoke();
                break;
            case 5:
                //Debug.Log("FantasticPresetEvent");
                FantasticPresetEvent.Invoke();
                break;
            
            default:
                break;
        }
    }
}
