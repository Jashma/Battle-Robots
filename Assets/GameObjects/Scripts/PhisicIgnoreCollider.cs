using UnityEngine;
using System.Collections;

public class PhisicIgnoreCollider : MonoBehaviour {

    private Rigidbody rigidBody;

	void Start () 
    {
        rigidBody = GetComponent<Rigidbody>();

	}
}
