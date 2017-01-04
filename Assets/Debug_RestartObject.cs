using UnityEngine;
using System.Collections;

public class Debug_RestartObject : MonoBehaviour
{
    private Vector3 startPosition;
    public GameObject restartObj;

    void Awake()
    {
        startPosition = transform.position;
    }

    void OnDisable()
    {
        //GameController.Instance.RestartObj(0.1f, restartObj, startPosition);
    }
	
    
}
