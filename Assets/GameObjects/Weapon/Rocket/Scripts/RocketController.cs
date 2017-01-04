using UnityEngine;
using System.Collections;

public class RocketController : MonoBehaviour
{
    public Transform parentTransform;
    public GameObject explosionObj;
    public GameObject meshStatic;
    public float speed;
    public float lifeTime = 10f;
    public bool playerShoot;
    public LayerMask layerMask;

    private bool hitUnit;
    private float maxFlyDistance;
    
    private RaycastHit hit;
    private Ray ray;
    private float spawnTime;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        meshStatic.SetActive(false);
        explosionObj.SetActive(false);
        maxFlyDistance = 1000;
        hitUnit = false;
        SendMessageUpwards("ClearParent");
        spawnTime = Time.time;
    }

    void Update()
    {
        CheckHits();

        parentTransform.position += (parentTransform.forward * speed) * Time.deltaTime;
        maxFlyDistance -= speed * Time.deltaTime;

        if (Time.time > spawnTime + lifeTime || maxFlyDistance < 0)
        {
            gameObject.SetActive(false);
            CreateExplosion();
        }
    }

    void CheckHits()
    {
        if (hitUnit == false)
        {
            float distance = Vector3.Distance(parentTransform.position, parentTransform.TransformPoint(Vector3.forward * speed * Time.deltaTime));
            ray = new Ray(parentTransform.position, parentTransform.TransformPoint(Vector3.forward * distance) - parentTransform.position);

            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                maxFlyDistance = hit.distance;
                hitUnit = true;
            }
        }
    }

    void CreateExplosion()
    {
        explosionObj.SetActive(true);
    }
}
