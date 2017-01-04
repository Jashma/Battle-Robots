using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    public BotController botController;
    public GunController gunController;
    public float speed;
    private ModulScr moduleState;
    public GameObject impactWoodPrefab;
    public GameObject impactPrefab;
    public GameObject impactMetalPrefab;
    public GameObject impactMetalRicochetPrefab;
    public bool playerShoot;
    private bool hitUnit;
    public float maxFlyDistance;
    public float damage;
    public float powerMin;
    public float powerMax;
    private float power;
    private int intDamage;
    public float lifeTime = 0.5f;
    private float spawnTime;
    public Team team;
    public LayerMask layerMask;
    private RaycastHit hit;
    private Ray ray;

    //Для дебага//
    public string debugText;
    public bool showDebug = true;

    void Start()
    {
        gunController = GetComponentInParent<GunController>();
        botController = GetComponentInParent<BotController>();

        if (PlayerController.botController == botController)
        {
            playerShoot = true;
        }

        transform.parent = null;

        damage += gunController.damage;
        powerMin = gunController.powerMin;
        powerMax = gunController.powerMax;

        spawnTime = Time.time;
        team = botController.team;

        CheckHits();
    }

    void Update()
    {
        CheckHits();

        transform.position += (transform.forward * speed) * Time.deltaTime;
        maxFlyDistance -= speed * Time.deltaTime;

        if (Time.time > spawnTime + lifeTime || maxFlyDistance < 0)
        {
            Destroy(gameObject);
        }
    }

    void CheckHits()
    {
        if (hitUnit == false)
        {
            float distance = Vector3.Distance(transform.position, transform.TransformPoint(Vector3.forward * speed * Time.deltaTime));
            ray = new Ray(transform.position, transform.TransformPoint(Vector3.forward * distance) - transform.position);
            
            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                maxFlyDistance = hit.distance;
                hitUnit = true;

                if (hit.collider.tag == "Modul")
                {
                    moduleState = hit.collider.GetComponent<ModulScr>();

                    if (moduleState != null)
                    {
                        power = Random.Range(powerMin, powerMax);
                        moduleState.getDamage(damage, power, playerShoot, false);
                    }

                    if (hit.transform.GetComponent<Rigidbody>())
                    {
                        hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * 100, hit.point);
                    }
                }

                CreateImpact();
                ShowDebugText(hit.collider, hit.collider.gameObject.name, distance);

                if (hit.collider.tag == "ArmorBot")
                {
                    if (hit.collider.name == "Armor_Body_back")
                    {
                        //Instantiate(Resources.Load("Prefabs/DebugSphere"), hit.point, Quaternion.identity);
                        Instantiate(Resources.Load("Prefabs/DebugSphere"), transform.position, Quaternion.identity);
                        Debug.DrawLine(hit.point, transform.position);
                    }
                }
            }
        }
    }

    void CreateImpact()
    {
        if (hit.collider.material.name == "Ground (Instance)")
        {
            Instantiate(impactPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        }
        else
            if (hit.collider.material.name == "Metal (Instance)")
            {
                Instantiate(impactMetalRicochetPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            }
            else
                if (hit.collider.material.name == "Wood (Instance)")
                {
                    Instantiate(impactWoodPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                }
                else
                {
                    Instantiate(impactMetalPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                }
    }

    void ShowDebugText(Collider hitCollider, string message, float distance)
    {
        if (showDebug == true)
        {
            if (playerShoot == true)
            {
                Debug.DrawRay(transform.position, transform.TransformPoint(Vector3.forward * distance) - transform.position, Color.red, 1);
                GameObject debugText = Instantiate(Resources.Load("Prefabs/User Interface/DebugText"), hitCollider.transform.position, Quaternion.identity) as GameObject;
                debugText.GetComponent<UI_DebugText>().spacePosition = hitCollider.transform.position;
                debugText.GetComponent<UI_DebugText>().debugMessage = message;
            }
        }
    }
}
