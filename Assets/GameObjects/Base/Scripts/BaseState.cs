using UnityEngine;
using System.Collections;


public class BaseState : MonoBehaviour 
{
    public GameObject satelitObj;
    public Team team;
    public CaptureState captureState;

    void Update()
    {
        satelitObj.transform.eulerAngles += new Vector3(0, 0.5f, 0) * 30 * Time.deltaTime;
    }

}
