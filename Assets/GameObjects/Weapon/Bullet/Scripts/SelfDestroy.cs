using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour 
{

	public float destroyTime = 5;
	void Start () 
    {
        Destroy (gameObject, destroyTime);
	
	}

}
