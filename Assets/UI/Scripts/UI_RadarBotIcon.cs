using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_RadarBotIcon : MonoBehaviour 
{
    public Color color;
    private Image image;
    private float alfa;

    void OnEnable()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        alfa = 0;
        image.color = color;
    }

    void Update()
    {
        if (alfa > 0)
        {
            alfa -= 0.02f;

            if (alfa < 0)
            { alfa = 0; }

            image.color = new Color(color.r, color.g, color.b, alfa);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        alfa = 1;
    }
}
