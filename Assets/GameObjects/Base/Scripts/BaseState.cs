using UnityEngine;
using System.Collections;


public class BaseState : MonoBehaviour 
{
    public GameObject deploedShipObj;
    public GameObject satelitObj;
    public Team team;
    public CaptureState captureState;
    public bool enableShip = false;
    public Vector3[] spawnPosition;
    private int spawnIndex;

    void Start()
    {
        if (enableShip == true)
        {
            deploedShipObj.GetComponent<DeploedShipController>().defaultDeploedPosition = transform.position;
            deploedShipObj.GetComponent<DeploedShipController>().team = team;
            deploedShipObj.SetActive(enableShip);
        }
    }

    void Update()
    {
        satelitObj.transform.eulerAngles += new Vector3(0, 0.5f, 0) * 30 * Time.deltaTime;
    }

    public void StartSpawn(GameObject bot)
    {
        StartCoroutine(Spawn(bot, 5, spawnIndex));

        spawnIndex++;
        if (spawnIndex == spawnPosition.Length)
        {
            spawnIndex = 0;
        }
    }

    public IEnumerator Spawn(GameObject bot, float time, int index)
    {
        yield return new WaitForSeconds(time);

        bot.SetActive(true);
        bot.transform.position = spawnPosition[index];
        bot.GetComponent<NavMeshObstacle>().enabled = false;
        bot.GetComponent<BotController>().botState = SM_BotState.Deploed;
        bot.GetComponent<BotController>().Initialise();
    }
}
