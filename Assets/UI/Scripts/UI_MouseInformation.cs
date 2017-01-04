using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_MouseInformation : MonoBehaviour
{
    public static UI_MouseInformation Instance { get; private set; }
    public Image fonImage;//Рамка меню
    public bool showMenu = false;//Вкл\Выкл меню
    public Vector2 defaultPanelSize;//Назачаем размер меню по умолчанию

    private Text[] TextMessage;//Массив компонентов, находящихся на дочерних обьектах
    private VerticalLayoutGroup layoutGroup;
    private GameObject layoutGroupObj;
    private RectTransform moveRectTransform;
    private float pivotOtstup;//Отступ от пивота
    private float leftWallDistance;
    private float RightWallDistance;
    private float UpWallDistance;
    private float DownWallDistance;
    private int upPosition;
    private int rightPosition;
    private Vector2 currentPanelSize;
    private float sizeX;
    private float sizeY;

    void Awake()
    {
        Instance = this;
        layoutGroupObj = transform.GetChild(0).gameObject;
        TextMessage = layoutGroupObj.GetComponentsInChildren<Text>();
        moveRectTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponentInChildren<VerticalLayoutGroup>();
        pivotOtstup = 10;

        ClearMessage();
    }

    void Update()
    {
        if (Cursor.visible == true)
        {
            if (showMenu == true)
            {
                ResizeImage();
                ShowMessage();

                CheckCollision();
                CheckPivotPosition();
                ChangeMenuPosition();
            }
        }
        else
        {
            if (showMenu == true)
            {
                ClearMessage();
            }
        }

        fonImage.enabled = showMenu;
    }

    void CheckCollision()
    {
        leftWallDistance = moveRectTransform.anchoredPosition.x;
        showMenu = Check(showMenu, leftWallDistance);

        RightWallDistance = Screen.width - moveRectTransform.anchoredPosition.x;
        showMenu = Check(showMenu, RightWallDistance);

        UpWallDistance = Screen.height - moveRectTransform.anchoredPosition.y;
        showMenu = Check(showMenu, UpWallDistance);

        DownWallDistance = moveRectTransform.anchoredPosition.y;
        showMenu = Check(showMenu, DownWallDistance);
    }

    bool Check(bool controlValue, float value)
    {
        if (controlValue == true)
        {
            if (value < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        } 
        else
        {
            return controlValue;
        } 
    }

    void CheckPivotPosition()
    {
        if (UpWallDistance <= currentPanelSize.y)
        {
            upPosition = 1;
        }
        else
        {
            upPosition = 0;
        }

        if (RightWallDistance <= currentPanelSize.x)
        {
            rightPosition = 1;
        }
        else
        {
            rightPosition = 0;
        }

        moveRectTransform.pivot = new Vector2(rightPosition, upPosition);
    }

    void ChangeMenuPosition()
    {
        moveRectTransform.anchoredPosition = Input.mousePosition + (Vector3.up + Vector3.right) * pivotOtstup;
    }

    public void SetMessage(string[] newMessage)
    {
        showMenu = true;
        
        sizeY = 0;

        for (int i = 0; i < TextMessage.Length; i++)
        {
            if (i < newMessage.Length && newMessage[i] != null)
            {
                TextMessage[i].gameObject.SetActive(true);
                TextMessage[i].text = newMessage[i];
                sizeY += defaultPanelSize.y;
            }
            else
            {
                TextMessage[i].text = null;
                TextMessage[i].gameObject.SetActive(false);
            }
        }
    }

    void ResizeImage()
    {
        sizeX = 0;

        for (int i = 0; i < TextMessage.Length; i++)
        {
            if (TextMessage[i].gameObject.activeSelf == true)
            {
                if (sizeX < TextMessage[i].GetComponent<RectTransform>().sizeDelta.x)
                {
                    sizeX = TextMessage[i].GetComponent<RectTransform>().sizeDelta.x;
                }
            }
        }
    }

    void ShowMessage()
    {
        currentPanelSize = new Vector2(sizeX + (layoutGroup.spacing * 2), sizeY);
        moveRectTransform.sizeDelta = currentPanelSize;
    }

    public void ClearMessage()
    {
        for (int i = 0; i < TextMessage.Length; i++)
        {
            TextMessage[i].text = null;
            TextMessage[i].gameObject.SetActive(false);
        }

        showMenu = false;
    }
}
