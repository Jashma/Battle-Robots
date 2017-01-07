using UnityEngine;
using System.Collections;

public abstract class ProjectileBasys : MonoBehaviour
{
    public float damage;
    public float powerMin;
    public float powerMax;
    public float energyCurrent = 0;
    public float speed;
    protected Transform flyTransform;
    public float flyDistance = 100;//Максимальная дальность полёта снаряда
    public float currentFlyDistance;//Текущая дальность полёта снаряд
    public bool showDebug = true;//Показывать или нет вылетающий урон
    public bool showFlyHit;//Флаг, что этой пулей выстрелил робот под управлением игрока

    public RaycastHit rayHit;
    public LayerMask layerMask;

    private ModulBasys modulBasys;

    void OnEnable()
    {
        //Debug.Log("OnEnable() " + gameObject.name);
    }

    virtual public void Fly(Transform flyTransform, float flySpeed)
    {
        flyTransform.position += (flyTransform.forward * flySpeed) * Time.deltaTime;
        currentFlyDistance -= flySpeed * Time.deltaTime;
    }

    virtual public bool CheckHit(Vector3 startPosition, Transform flyTransform, float flySpeed)
    {
        bool hit = false;
        Ray ray = new Ray(startPosition, (flyTransform.forward * flySpeed) * Time.deltaTime);

        if (Physics.Raycast(ray, out rayHit, flySpeed * Time.deltaTime, layerMask))
        {
            hit = true;
            flyTransform.position = rayHit.point;
        }

        return hit;
    }

    virtual public void CheckIsLive()
    {
        Debug.Log("CheckIsLive() " + gameObject.name);
    }

    virtual public void Hit()
    {
        modulBasys = rayHit.collider.GetComponent<ModulBasys>();

        if (modulBasys != null)
        {
            float power = Random.Range(powerMin, powerMax);
            modulBasys.GetDamage(damage, power, showFlyHit);
        }
    }

}
