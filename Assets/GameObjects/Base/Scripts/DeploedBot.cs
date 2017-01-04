using UnityEngine;
using System.Collections;

public class DeploedBot : MonoBehaviour 
{

	void Update()
	{
		transform.parent.eulerAngles += new Vector3 (0, 2, 0);
	}
}
