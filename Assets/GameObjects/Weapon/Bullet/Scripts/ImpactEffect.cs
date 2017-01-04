using UnityEngine;
using System.Collections;

public class ImpactEffect : MonoBehaviour
{
    public ProjectileBasys bulletController;
    private ParticleSystem particleSystem;
    private bool showEffect = false;

    void OnEnable()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }
        else
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, bulletController.rayHit.normal);
        }
    }

    public void ShowEffect()
    {
        showEffect = true;
    }

    void Update()
    {
        if (particleSystem == null)
        {
            return;
        }

        if (particleSystem.isPlaying == false)
        {
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (showEffect == false)
        {
            return;
        }

        SendMessageUpwards("ReturnToParent", 0.1f);
        showEffect = false;
    }

}
