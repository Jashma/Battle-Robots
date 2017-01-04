using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_AimItem : MonoBehaviour 
{
    public RawImage aimImage;
    
    public Button redButton;
    public Button blueButton;
    public Button greenButton;
    public Button yellowButton;
    public Button whiteButton;

    public Button applyButton;
    public Color currentColor;


    void Start()
    {
        currentColor = Color.white;
        aimImage.color = currentColor;
    }

    public void ChangeColor(Image buttonImage)
    {
        aimImage.color = buttonImage.color;
        currentColor = buttonImage.color;
    }

    public void applyCrossHeir(RawImage image)
    {
        //GlobalPlayerConfig.playerCrossheirTexture = image.texture;
        //GlobalPlayerConfig.playerCrossheirColor = image.color;
    }
}
