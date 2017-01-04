using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModulScr : MonoBehaviour 
{
    private BoxCollider boxCollider;
    private Rigidbody rigidBody;
    public GameObject smokePrefab;
    public GameObject smokeObj;
    public BotController botController;
    public float health;//здоровье модуля
    public float armore;//броня
    public bool crite;
    public int costModul;
    public string nameModul;
    public string aboutModul;

    private bool init = false;

    void Awake()
    {
        smokePrefab = Resources.Load("Weapons/ModernGunEffects/Effects/Misc/smokeLight") as GameObject;
    }

    public void EnableModule()
    {
        nameModul = transform.parent.tag;
        name = transform.parent.tag;
        tag = "Modul";
        gameObject.layer = 10;
        health = 100;
        armore = Random.Range(1, 3);

        for (int i = 0; i < botController.modulScr.Count; i++)
        {
            if (botController.modulScr[i] == null || botController.modulScr[i].transform.parent.tag == transform.parent.tag)
            {
                botController.modulScr[i] = this;
                init = true;
                return;
            }
        }

        if (init == false)
        {
            botController.modulScr.Add(this);
        } 
    }

    public bool getDamage(float damage, float power, bool playerShoot = false, bool leftPositionGun = false)
    {
        bool isBreak = false;
        if (power > armore)
        {
            health -= damage;
            botController.health -= damage;

            //Смерть бота
            if (botController.health <= 0)
            {
                botController.DestroyBot();
                botController.health = 0;
            }

            //Создаём дым
            if (health < 50 && crite == false)
            {
                //smokeObj = Instantiate(smokePrefab, transform.position, transform.rotation) as GameObject;
                //smokeObj.transform.parent = transform;
                //smokeObj.transform.eulerAngles = new Vector3(-90, 0, 0);
               // smokeObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                //crite = true;
            }

            //GameObject debugText = Instantiate(Resources.Load("Prefabs/DebugText"), transform.position, Quaternion.identity) as GameObject;
            //debugText.GetComponent<UI_DebugText>().spacePosition = transform.position;
            //debugText.GetComponent<UI_DebugText>().debugMessage = damage.ToString();
        }
        else
        {
            //GameObject debugText = Instantiate(Resources.Load("Prefabs/DebugText"), transform.position, Quaternion.identity) as GameObject;
            //debugText.GetComponent<UI_DebugText>().spacePosition = transform.position;
            //debugText.GetComponent<UI_DebugText>().debugMessage = "Не пробил";
        }

        return isBreak;
    }
}
