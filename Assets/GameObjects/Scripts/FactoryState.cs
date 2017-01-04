using UnityEngine;
using System.Collections;

public class FactoryState : MonoBehaviour 
{
    private GameObject ui_IndicatorObj;
    public CaptureState captureState;
    public BaseState baseState;
    public bool camVisible;
    private Transform cilinderTransform;

    void Start()
    {
        captureState = GetComponent<CaptureState>();
        LevelController.arrayFactoryState.Add(this);
        /*
        cilinderTransform = transform.FindChild("Cylinder");
        cilinderTransform.gameObject.SetActive(false);
        ui_IndicatorObj = Instantiate(Resources.Load("UI/Prefab/Factory_ui"), Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
        ui_IndicatorObj.transform.SetParent(GameObject.Find("SceneObjInterface").transform);
        ui_IndicatorObj.GetComponent<UI_FactoryIndicator>().factoryState = this;
        ui_IndicatorObj.GetComponent<UI_SceneIndicator>().parentTransform = gameObject.transform;
        GameObject.Find("SceneObjInterface").GetComponent<UI_SceneObjController>().uiObjList.Add(ui_IndicatorObj.GetComponent<UI_SceneIndicator>());
         */ 
        camVisible = false;
    }

    void Update()
    {
        if (captureState.team != Team.None)
        {
            if (baseState == null)
            {
                if (captureState.team == Team.Red)
                {
                    baseState = GameObject.FindGameObjectWithTag("BaseRed").GetComponent<BaseState>();
                }

                if (captureState.team == Team.Blue)
                {
                    baseState = GameObject.FindGameObjectWithTag("BaseBlue").GetComponent<BaseState>();
                }
            }
        }
        else
        {
            baseState = null;
        }
    }

    void OnBecameVisible()
    {
        camVisible = true;
    }

    void OnBecameInvisible()
    {
        camVisible = false;
    }

}
