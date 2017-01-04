using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI_Health : MonoBehaviour 
{
    public Slider healthSlider;
    public Image image;
    public float health;

    //Debug
    public Color colorDB;
    

    void Update()
    {
        if (PlayerController.botController != null)
        {
            if (health != PlayerController.botController.health * 0.01f)
            {
                healthSlider.value = PlayerController.botController.health;
                health = PlayerController.botController.health * 0.01f;
                image.color = new Color(1 - health, health, 0);
                colorDB = new Color(1 - health, health, 0);
            }
        }
    }
}
