using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    public bool shoot = false;
    public bool readyToShoot = false;
    public bool readyToReload = false;
    
    public Transform parentTransform;//обьект к которому удочеряется пуля изначально и после выстрела
    public Vector3 startPosition;//Стартовая позиция пули. НУжна что бы после отключения пуля вернулась в началльную позицию
    public Quaternion startRotation;//Стартовая позиция пули. НУжна что бы после отключения пуля вернулась в началльную позицию
    public ProjectileBasys projectilBasys;
    public GameObject rocketObj;//Мешь ракеты или пули
    public GameObject effectObj;//Эфект после поподания снаряда
    //Debug
    public GameObject aimObj;
    public bool startReload = false;
    private Transform thisTransform;

    private void Awake()
    {
        readyToReload = true;
        thisTransform = transform;
        projectilBasys.gameObject.SetActive(false); 
    }

    void Start()
    {
        parentTransform = thisTransform.parent;
        startPosition = thisTransform.localPosition;
        startRotation = thisTransform.localRotation;

        if (rocketObj != null) { rocketObj.SetActive(false); }
        if (aimObj != null) { aimObj.SetActive(false); }
        if (effectObj != null) { effectObj.SetActive(false); }

        Reload();
    }

    private void ReStart()
    {
        thisTransform.localPosition = startPosition;
        thisTransform.localRotation = startRotation;
        readyToReload = true;
    }

    //Debug
    private void Update()
    {
        //Debug
        if (shoot == true)
        {
            Shoot(Vector3.zero, false, 100);
            shoot = false;
        }

        if (startReload == true)
        {
            Reload();
            startReload = false;
        }
    }
    //End Debug

    public void Reload()
    {
        readyToReload = false;
        readyToShoot = true;
        if (rocketObj != null) { rocketObj.SetActive(true); }
    }

    public void ReturnToParent(float time = 0.1f)
    {
        if (readyToReload == false)
        {
            StartCoroutine(ToParent(time));
        }
    }

    public void PostEffect()
    {
        if (rocketObj != null) { rocketObj.SetActive(false); }
        if (aimObj != null) { aimObj.SetActive(true); }
        projectilBasys.gameObject.SetActive(false);
        effectObj.SetActive(true);
        effectObj.SendMessage("ShowEffect");
    }

    private IEnumerator ToParent(float time)
    {
        yield return new WaitForSeconds(time * Time.deltaTime);
        thisTransform.SetParent(parentTransform);
        ReStart();
    }

    public void Shoot(Vector3 directionLook, bool showFlyHit, float energy = -1)
    {
        projectilBasys.energyCurrent = energy;
        projectilBasys.showFlyHit = showFlyHit;
        thisTransform.LookAt(directionLook);
        readyToShoot = false;
        readyToReload = false;
        
        thisTransform.parent = null;
        projectilBasys.gameObject.SetActive(true);
    }

    
}
