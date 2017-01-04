using UnityEngine;
using System.Collections;

public class PlayerMouseLook : MonoBehaviour 
{
    public float mouseSence;
    public float sniperMouseSence;
    public int invertY;
    public float speedScrollCamera;
    public bool sniperLook = false;
    private float rotationY;
    private float rotationX;
    //ограничния поворота камеры по осям
    //Y
    private float minimumY = -45f;
    private float maximumY = 45f;
    //ограничение скрола для камеры
    private float minimumZ = -55f;
    private float maximumZ = -5f;

    private float posUp = 0.5f;//Высота положения камеры над роботом
    private float tmpPositionY;
    private float tmpPositionZ;
    private float cameraPositionZ;
    private Vector3 tempPositionBot;
    private Vector3 cameraColliderPoint;
    private float cameraSniperZoom = 10;
    public LayerMask BackwardLayerMask;

    void Start()
    {
        cameraPositionZ = -20;
    }

    void LateUpdate()
    {
        if (PlayerController.playerState == PlayerState.Spectator)
        {
            SpectatorMouseRotate();
        }

        if (PlayerController.playerState == PlayerState.Follow)
        {
            FollowBotMouseRotate();
        }

        if (PlayerController.playerState == PlayerState.BotControl)
        {
            if (sniperLook == true)
            {
                SniperMouseRotate();
            }
            else
            {
                ControlBotMouseRotate();
            }
        }

        if (PlayerController.playerState == PlayerState.Deploed)
        {
            DeploedPosition();
        }
        
    }

    void DeploedPosition()
    {
        if (Camera.main.fieldOfView != 60)
        {
            Camera.main.fieldOfView = 60;
        }

        transform.position = PlayerController.baseController.transform.position + Vector3.up * 10;
        transform.LookAt(PlayerController.baseController.deploedShipObj.transform.position);
    }

    void SpectatorMouseRotate()
    {
        if (Camera.main.fieldOfView != 60)
        {
            Camera.main.fieldOfView = 60;
        }

        if (UI_Controller.uiState == UI_State.InGame)
        {
            rotationX = (Input.GetAxis("Mouse X") * mouseSence);
            rotationY = (Input.GetAxis("Mouse Y") * mouseSence);


            transform.eulerAngles += new Vector3(rotationY * invertY, rotationX, 0f);

            float clampY = transform.localEulerAngles.y;
            float clampX = transform.localEulerAngles.x;

            if (transform.localEulerAngles.x < 180)
            {
                clampX = Mathf.Clamp(transform.localEulerAngles.x, 0, maximumY);
            }
            else
            {
                clampX = Mathf.Clamp(transform.localEulerAngles.x, 360 + minimumY, 360);
            }

            transform.localEulerAngles = new Vector3(clampX, clampY, transform.localEulerAngles.z);
        }
    }

    void ControlBotMouseRotate()
    {
        if (Camera.main.fieldOfView != 60)
        {
            Camera.main.fieldOfView = 60;
        }

        if (UI_Controller.uiState == UI_State.InGame)
        {
            if (PlayerController.botController != null)
            {
                tmpPositionY = PlayerController.botController.characterController.height + posUp;

                rotationX = 0;
                rotationY = 0;

                rotationY = (Input.GetAxis("Mouse Y") * mouseSence);
                rotationX = (Input.GetAxis("Mouse X") * mouseSence);
                //Приближаем камеру
                cameraPositionZ = Mathf.Clamp(cameraPositionZ + (Input.GetAxis("Mouse ScrollWheel") * speedScrollCamera), minimumZ, maximumZ);

                //Ограничиваем угол повора камеры по оси Y
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                tempPositionBot = new Vector3(PlayerController.botController.transform.position.x,
                                              PlayerController.botController.transform.position.y + tmpPositionY,
                                              PlayerController.botController.transform.position.z);
                tmpPositionZ = cameraPositionZ * 0.1f;
                cameraColliderPoint = transform.TransformPoint(Vector3.back * 2f);
                //-= проверка на столкновения камеры =-//
                tmpPositionZ = checkCollision(cameraColliderPoint, tempPositionBot, tmpPositionZ, Vector3.Distance(cameraColliderPoint, tempPositionBot));

                transform.eulerAngles += new Vector3(rotationY * invertY, rotationX, 0f);

                float clampY = transform.localEulerAngles.y;
                float clampX = transform.localEulerAngles.x;

                if (transform.localEulerAngles.x < 180)
                {
                    clampX = Mathf.Clamp(transform.localEulerAngles.x, 0, 45);
                }
                else
                {
                    clampX = Mathf.Clamp(transform.localEulerAngles.x, 360 - 45, 360);
                }

                transform.localEulerAngles = new Vector3(clampX, clampY, transform.localEulerAngles.z);

                transform.position = transform.rotation * new Vector3(0f, 0f, tmpPositionZ) + tempPositionBot;
            }
        }
    }

    void FollowBotMouseRotate()
    {
        if (Camera.main.fieldOfView != 60)
        {
            Camera.main.fieldOfView = 60;
        }

        if (PlayerController.botController != null)
        {
            if (transform.parent != null) { transform.parent = null; }

            rotationX = 0;
            rotationY = 0;

            tmpPositionY = PlayerController.botController.characterController.height + posUp;

            if (UI_Controller.uiState == UI_State.InGame)
            {
                rotationY = (Input.GetAxis("Mouse Y") * mouseSence);
                rotationX = (Input.GetAxis("Mouse X") * mouseSence);
                //Приближаем камеру
                cameraPositionZ = Mathf.Clamp(cameraPositionZ + (Input.GetAxis("Mouse ScrollWheel") * speedScrollCamera), minimumZ, maximumZ);
            }

            tempPositionBot = new Vector3(PlayerController.botController.transform.position.x,
                                           PlayerController.botController.transform.position.y + tmpPositionY,
                                           PlayerController.botController.transform.position.z);
            tmpPositionZ = cameraPositionZ * 0.1f;
            cameraColliderPoint = transform.TransformPoint(Vector3.back * 2f);
            //-= проверка на столкновения камеры =-//
            tmpPositionZ = checkCollision(cameraColliderPoint, tempPositionBot, tmpPositionZ, Vector3.Distance(cameraColliderPoint, tempPositionBot));

            transform.eulerAngles += new Vector3(rotationY * invertY, rotationX, 0f);

            float clampY = transform.localEulerAngles.y;
            float clampX = transform.localEulerAngles.x;

            if (transform.localEulerAngles.x < 180)
            {
                clampX = Mathf.Clamp(transform.localEulerAngles.x, 0, 45);
            }
            else
            {
                clampX = Mathf.Clamp(transform.localEulerAngles.x, 360 - 45, 360);
            }

            transform.localEulerAngles = new Vector3(clampX, clampY, transform.localEulerAngles.z);

            transform.position = transform.rotation * new Vector3(0f, 0f, tmpPositionZ) + tempPositionBot;
        }
    }

    void SniperMouseRotate()
    {
        if (UI_Controller.uiState == UI_State.InGame)
        {
            if (transform.parent == null) { transform.parent = PlayerController.botController.transform; }

            rotationX = 0;
            rotationY = 0;

            float sence = (sniperMouseSence*0.1f) * (Camera.main.fieldOfView * 0.1f);

            rotationY = (Input.GetAxis("Mouse Y") * sence);
            rotationX = (Input.GetAxis("Mouse X") * sence);

            transform.position = PlayerController.botController.bodyController.transform.TransformPoint(PlayerController.botController.bodyController.pointSniper);

            transform.localEulerAngles += new Vector3(rotationY * invertY, rotationX, 0f);

            float clampY = transform.localEulerAngles.y;
            float clampX = transform.localEulerAngles.x;

            if (transform.localEulerAngles.x < 180)
            {
                clampX = Mathf.Clamp(transform.localEulerAngles.x, 0, 45);
            }
            else
            {
                clampX = Mathf.Clamp(transform.localEulerAngles.x, 360 - 45, 360);
            }

            transform.localEulerAngles = new Vector3(clampX, clampY, transform.localEulerAngles.z);

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                cameraSniperZoom += 15;
                if (cameraSniperZoom > 55)
                {
                    cameraSniperZoom = 55;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                cameraSniperZoom -= 15;
                if (cameraSniperZoom < 10)
                {
                    cameraSniperZoom = 10;
                }
            }

            Camera.main.fieldOfView = cameraSniperZoom;

        }
        else
        {
            transform.parent = null;
            sniperLook = false;
        }
    }

    float checkCollision(Vector3 myPosition, Vector3 targetPosition, float currentDistance, float collisionDistance)
    {
        Ray rayCollision = new Ray(targetPosition, myPosition - targetPosition);
        RaycastHit hitCollision;
        if (Physics.Raycast(rayCollision, out hitCollision, collisionDistance, BackwardLayerMask))
        {//сохраняем z камеры равный расстоянию до обьекта столкновения
            if (hitCollision.distance < currentDistance * -1f)
            {
                currentDistance = hitCollision.distance;
                currentDistance = currentDistance * -1f;
            }
        }
        return currentDistance;
    }

}
