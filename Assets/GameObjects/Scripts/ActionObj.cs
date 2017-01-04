using UnityEngine;
using System.Collections;

public class ActionObj : MonoBehaviour 
{
    public SpriteRenderer spriteRender;
    public Vector3 currentPosition;
    private float terrainY;
    private Transform playerTransform;
    private float distance;
    private Vector3 nexPosition;
    public bool inAction;
    private bool placeEmpty;

    void Start()
    {
        inAction = true;
        spriteRender = GetComponentInChildren<SpriteRenderer>();
        playerTransform = GameObject.Find("Player").transform;
        spriteRender.color = Color.green;
        name = "CreatePlace";
        placeEmpty = true;
    }

	void Update () 
    {
        if (UI_Controller.uiState == UI_State.InGame)
        {
            if (inAction == true)
            {
                if (Input.GetAxis("Fire2") != 0)
                {
                    Debug.Log("Input.GetAxis(Fire2)");
                    Destroy(this.gameObject);
                }

                if (Input.GetAxis("Fire1") != 0)
                {
                    if (placeEmpty == true)
                    {
                        Debug.Log("Input.GetAxis(Fire1)");
                        inAction = false;

                        if (GameObject.Find("Satelit") != null)
                        {
                            GameObject.Find("Satelit").transform.position = transform.position;
                        }
                        else
                        {
                            Instantiate(Resources.Load("Prefabs/Satelit"), transform.position, Quaternion.identity);
                        }

                        GameObject.Find("DeploedShip").GetComponent<DeploedShipController>().deploedPosition = transform.position;

                        Destroy(this.gameObject);
                    }
                }

                terrainY = Terrain.activeTerrain.SampleHeight(transform.position);
                distance = Vector3.Distance(playerTransform.position, PlayerController.hitForwardCollision.point);
                distance = Mathf.Clamp(distance, 0, 10);
                nexPosition = playerTransform.TransformPoint(Vector3.forward * distance);
                currentPosition = new Vector3(nexPosition.x, terrainY + 0.15f, nexPosition.z);
                transform.position = currentPosition;
            }
        }
	}

    void OnTriggerStay()
    {
        spriteRender.color = Color.red;
        placeEmpty = false;
    }

    void OnTriggerExit()
    {
        spriteRender.color = Color.green;
        placeEmpty = true;
    }
}
