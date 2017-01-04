using UnityEngine;
using System.Collections;

<<<<<<< HEAD
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
=======
public class WalkSoundController : MonoBehaviour {

    public AudioSource audioSorce;
    public AudioClip audioClip;
    public Transform oneFootTransform;
    public float minDistToGround = 0.5f;
    public bool startPlay;

    public GameObject stepSmoke;

    void OnEnable()
    {
        oneFootTransform = transform;

        if (audioClip == null)
        {
            audioClip = Resources.Load("Sound/RobotFootsteps1") as AudioClip;
        }

        if (audioSorce == null)
        {
            audioSorce = GetComponentInParent<AudioSource>();
        }

        if (transform.FindChild("groundStepSmokes") != null)
        {
            stepSmoke = transform.FindChild("groundStepSmokes").gameObject;
            stepSmoke.SetActive(false);
        } 
>>>>>>> parent of a891378... Global update
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
<<<<<<< HEAD
        {
            if (startPlay == false)
            {
                startPlay = true;
                audioSource.volume = GameController.soundEffectsVolume;
                audioSource.PlayOneShot(audioClip);
            }
        }
=======
            {
                if (startPlay == false)
                {
                    startPlay = true;
                    audioSorce.pitch = Random.Range(0.9f, 1.1f);
                    audioSorce.PlayOneShot(audioClip);

                    if (stepSmoke != null)
                    {
                        if (stepSmoke.activeSelf==true)
                        { 
                            stepSmoke.SetActive(false); 
                        }

                        stepSmoke.SetActive(true);
                    }
                }
            }
>>>>>>> parent of a891378... Global update
        else
        {
            startPlay = false;
        }
<<<<<<< HEAD

=======
         
>>>>>>> parent of a891378... Global update
    }
}
