using UnityEngine;
using System.Collections;

public enum rotateAxis
{
    AxisX, AxisY, AxisXY
}

public class GunRotation : MonoBehaviour
{
    public rotateAxis axis;
    public GunController gunController;
    public BodyRotation bodyRotation;
    public BotController botController;
    //public GameObject pointShoot;
    //public GameObject pointLaser;
    //Точка в пространстве, куда должно быть нацелено оружие
    public Vector3 gunTarget;
    public LayerMask layerMask;
    public float distanceToHit;
    //Точка в пространстве, куда должно быть нацелено оружие
    public Vector3 currentGunTarget;

    private Ray rayCollision;
    public RaycastHit hitCollision;

    private Quaternion directionGun;
    private float tempAngleGunX = 0;
    private float tempAngleGunY = 0;
    private Vector3 pointShoot;
    //-=для дебага=-//
    public string debugMessage;

    void OnEnable()
    {
        botController = GetComponentInParent<BotController>();
        bodyRotation = GetComponentInParent<BodyRotation>();
        gunController = GetComponentInChildren<GunController>();
    }

    void LateUpdate()
    {
        if (gunController.weaponIsOn == true)
        {
            if (botController != null)
            {
                if (botController.botState == SM_BotState.AiControl || botController.botState == SM_BotState.PlayerControl)
                {
                    GetTarget();
                    GetLinePosition();
                    Rotation();
                }
            }
        }
    }

    void GetTarget()
    {
        gunTarget = bodyRotation.bodyTarget;
    }

    void GetLinePosition()
    {
        pointShoot = gunController.muzzleFire.transform.position;
        rayCollision = new Ray(pointShoot, transform.forward * 500f);
        //Debug.DrawRay(pointShoot, transform.forward * 500f, Color.blue);

        if (Physics.Raycast(rayCollision, out hitCollision, 500, layerMask))
        {
            //Debug.DrawRay(pointShoot, hitCollision.point - pointShoot, Color.red);
            distanceToHit = Vector3.Distance(pointShoot, hitCollision.point);
            currentGunTarget = hitCollision.point;

            if (botController.botState == SM_BotState.AiControl)
            {
                AiShoot();
            }
        }
        else
        {
            distanceToHit = Vector3.Distance(pointShoot, transform.forward * 500f);
            currentGunTarget = pointShoot + (Vector3.forward * distanceToHit);
        }

        gunController.currentGunTarget = currentGunTarget;
    }

    void AiShoot()
    {
        if (hitCollision.collider.tag == "Modul")
        {
            ModulScr tmpModulState = hitCollision.collider.GetComponent<ModulScr>();

            if (tmpModulState != null && tmpModulState.botController != null)
            {
                if (tmpModulState.botController.team != botController.team)
                {
                    if (tmpModulState.botController.health > 0)
                    {
                        //-=Стреляем=-//
                        gunController.Shoot();
                    }
                }
            }
            else
            {
                Debug.Log(botController.gameObject.name + " No botState in hitCollision.collider ModuleState");
            }
        }
    }

    void Rotation()
    {
        //вектор, куда нужно вращать башню
        directionGun = Quaternion.LookRotation(gunTarget - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, directionGun, gunController.speedRotateGun * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, directionGun, (gunController.speedRotateGun * 0.1f) * Time.deltaTime);

        tempAngleGunX = 0;
        tempAngleGunY = 0;
        //
        if (axis == rotateAxis.AxisX || axis == rotateAxis.AxisXY)
        {
            tempAngleGunX = transform.localEulerAngles.x;
            if (transform.localEulerAngles.x > 180f)
            {
                //если значение Х больше 180, то используем ограничение между 360 и максимальним ограничением
                tempAngleGunX = Mathf.Clamp(tempAngleGunX, 360f - gunController.maximumAngleGun, 360f);
            }
            else
            {
                if (transform.localEulerAngles.x < 180f)
                {
                    //если значение Х меньше 180, то ограничение используем между 0 и минимальным ограничением
                    tempAngleGunX = Mathf.Clamp(tempAngleGunX, 0f, gunController.minimumAngleGun);
                }
            }
        }
        //
        if (axis == rotateAxis.AxisY || axis == rotateAxis.AxisXY)
        {
            tempAngleGunY = transform.localEulerAngles.y;
            if (transform.localEulerAngles.y > 180f)
            {
                //если значение Y больше 180, то используем ограничение между 360 и максимальним ограничением
                tempAngleGunY = Mathf.Clamp(tempAngleGunY, 360f - gunController.rangeAngleGun, 360f);
            }
            else
            {
                if (transform.localEulerAngles.y < 180f)
                {
                    //если значение Y меньше 180, то ограничение используем между 0 и минимальным ограничением
                    tempAngleGunY = Mathf.Clamp(tempAngleGunY, 0f, gunController.rangeAngleGun);
                }
            }
        }

        //присваиваем орудию новое значение Х и Y
        transform.localEulerAngles = new Vector3(tempAngleGunX, tempAngleGunY, 0);

        //Debug.DrawLine(pointShoot.transform.position, gunTarget, Color.black);
        //Debug.DrawLine(pointShoot.transform.position, currentGunTarget, Color.blue);
        //float tmpDist = Vector3.Distance(transform.position, gunTarget);
        //Debug.DrawLine(transform.position, transform.TransformPoint(Vector3.forward * tmpDist), Color.green);
          
    }
}
