using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI_HealthBot : MonoBehaviour 
{
    public Slider healthSlider;
    public Image image;
    //Debug
    public Color colorDB;
    public float health;

    void OnEnable()
    {
        //Debug.Log("UI_HealthBot OnEnable");

        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();

            foreach (Image searchImage in GetComponentsInChildren<Image>())
            {
                if (searchImage.transform.name == "Fill")
                {
                    image = searchImage;
                    return;
                }
            }
        }
    }

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
