using UnityEngine;
using System.Collections;

public class TestAimPlace : MonoBehaviour
{
    public ProjectileBasys bulletController;

    void OnEnable()
    {
        if (bulletController.rayHit.collider != null)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, bulletController.rayHit.normal);
            //transform.RotateAround(transform.position, transform.up, 0);
            

            //Quaternion.FromToRotation(Vector3.up, bulletController.rayHit.normal);
            //Debug.Log(bulletController.rayHit.collider.name);
            //transform.up = bulletController.rayHit.normal;
        }
    }

    void OnDisable()
    {
        transform.eulerAngles = Vector3.zero;
    }
}
