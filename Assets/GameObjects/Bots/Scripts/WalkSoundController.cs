using UnityEngine;
using System.Collections;

public class WalkSoundController : MonoBehaviour
{
    public AudioClip audioClip;
    public Transform oneFootTransform;
    //public Transform[] footTransform;
    public float minDistToGround = 0.5f;
    public bool startPlay;

    private AudioSource audioSource;

    void Start()
    {
        oneFootTransform = transform;
        audioSource = GetComponentInParent<AudioSource>();
        //stepAudioClip = Resources.Load("DefaultSound/Step/Robot Footsteps 1") as AudioClip;

        if (audioSource != null)
        {
            audioSource.volume = GameController.soundEffectsVolume;
        }
    }

    void Update()
    {
        voiceToGround();
    }

    public void voiceToGround()
    {
        Vector3 groundPosition = new Vector3(oneFootTransform.position.x, Terrain.activeTerrain.SampleHeight(oneFootTransform.position), oneFootTransform.position.z);
        Debug.DrawLine(oneFootTransform.position, groundPosition, Color.red);

        if (Vector3.Distance(oneFootTransform.position, groundPosition) < minDistToGround)
        {
            if (startPlay == false)
            {
                startPlay = true;
                audioSource.volume = GameController.soundEffectsVolume;
                audioSource.PlayOneShot(audioClip);
            }
        }
        else
        {
            startPlay = false;
        }

    }
}
