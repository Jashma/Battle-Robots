using UnityEngine;
using System.Collections;

public enum MouseState
{
    None,
    SniperLook,
    ChangeModulLook,
    FreeLook,
    FollowLook,
    ControlLook,
    DeploedLook,
}

public class PlayerMouseLook : MonoBehaviour 
{
    public MouseState mouseState;
    public float sniperMouseSence;
    private int invertY = -1;
    public float speedScrollCamera;
    private float rotationY;
    private float rotationX;
    //ограничния поворота камеры по осям
    //Y
    private float minimumY = -45f;
    private float maximumY = 45f;
    //ограничение скрола для камеры
    private float minimumZ = -55f;
    private float maximumZ = -5f;
    private Transform bodyTransform;
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
        mouseState = MouseState.FreeLook;
        cameraPositionZ = -20;
    }

    void LateUpdate()
    {
        rotationX = 0;
        rotationY = 0;

        if (mouseState == MouseState.FreeLook)
        {
            SpectatorMouseRotate();
        }

        if (mouseState == MouseState.FollowLook)
        {
            FollowBotMouseRotate();
        }

        if (mouseState == MouseState.ControlLook)
        {
            ControlBotMouseRotate();
        }

        if (mouseState == MouseState.SniperLook)
        {
            SniperMouseRotate();
        }

        if (mouseState == MouseState.ChangeModulLook)
        {
            ChangeModulMouseRotate();
        }

        if (mouseState == MouseState.DeploedLook)
        {
            DeploedPosition();
        }
        
    }

    public void ChangeLookState(int enumValue)
    {
        if (enumValue == 0)
        {
            if (PlayerController.playerState == PlayerState.ControlBot)
            {
                enumValue = 5;
            }

            if (PlayerController.playerState == PlayerState.ChangeModul)
            {
                enumValue = 2;
            }

            if (PlayerController.playerState == PlayerState.Deploed)
            {
                enumValue = 6;
            }

            if (PlayerController.playerState == PlayerState.FollowBot)
            {
                enumValue = 4;
            }

            if (PlayerController.playerState == PlayerState.Spectator)
            {
                enumValue = 3;
            }
        }

        mouseState = (MouseState)enumValue;
        
    }

    void DeploedPosition()
    {
        if (Camera.main.fieldOfView != 60)
        {
            Camera.main.fieldOfView = 60;
        }
    }

    void SpectatorMouseRotate()
    {
        if (Camera.main.fieldOfView != 60)
        {
            Camera.main.fieldOfView = 60;
        }

        if (UI_Controller.uiState == UI_State.InGame)
        {
            GetMouseAxis(PlayerController.mouseSence);

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
                tmpPositionY = PlayerController.botController.botHeight + posUp;

                GetMouseAxis(PlayerController.mouseSence);

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

            tmpPositionY = PlayerController.botController.botHeight + posUp;

            if (UI_Controller.uiState == UI_State.InGame)
            {
                GetMouseAxis(PlayerController.mouseSence);

                //Приближаем камеру
                //cameraPositionZ = Mathf.Clamp(cameraPositionZ + (Input.GetAxis("Mouse ScrollWheel") * speedScrollCamera), minimumZ, maximumZ);
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
        else
        {
            Debug.Log("No Bot to follow");
            PlayerController.Instance.ChangePlayerState(1);//"ChangeModul"
        }
    }

    void SniperMouseRotate()
    {
        if (UI_Controller.uiState == UI_State.InGame)
        {
            if (PlayerController.botController == null)
            {
                ChangeLookState(0);
            }
            else
            {
                if (bodyTransform == null)
                {
                    foreach (ModulBasys modul in PlayerController.botController.modulController)
                    {
                        if (modul.modulType == ModulType.Body)
                        {
                            bodyTransform = modul.GetBodyModul().rotateTransform;
                            return;
                        }
                    }
                }
                else
                {
                    float sence = (sniperMouseSence * 0.1f) * (Camera.main.fieldOfView * 0.1f);

                    GetMouseAxis(sence);

                    transform.position = PlayerController.botController.pointSniper.position; //bodyTransform.TransformPoint(PlayerController.botController.pointSniper);

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
            }
        }
        else
        {
            ChangeLookState(0);
        }
    }

    void ChangeModulMouseRotate()
    {
        if (Camera.main.fieldOfView != 60)
        {
            Camera.main.fieldOfView = 60;
        }

        if (UI_Controller.uiState == UI_State.ChangeModulMenu)
        {
            if (PlayerController.botController != null)
            {
                rotationY = 0;
                rotationX = 0;
                cameraPositionZ = minimumZ;
                tmpPositionY = PlayerController.botController.botHeight / 2;
                tempPositionBot = new Vector3(PlayerController.botController.transform.position.x,
                                               PlayerController.botController.transform.position.y + tmpPositionY,
                                               PlayerController.botController.transform.position.z);
                tmpPositionZ = cameraPositionZ * 0.1f;
                transform.position = tempPositionBot + new Vector3(0, 0, tmpPositionZ);
                transform.LookAt(tempPositionBot);
            }
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

    void GetMouseAxis(float sence)
    {
        if (UI_Controller.uiInGameMenu == UI_InGameMenu.None)
        {
            rotationY = (Input.GetAxis("Mouse Y") * sence);
            rotationX = (Input.GetAxis("Mouse X") * sence);
            //Приближаем камеру
            cameraPositionZ = Mathf.Clamp(cameraPositionZ + (Input.GetAxis("Mouse ScrollWheel") * speedScrollCamera), minimumZ, maximumZ);
        }
    }
}
