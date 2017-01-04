using UnityEngine;
using System.Collections;

public class UI_RadarLine : MonoBehaviour 
{
    public RectTransform searchLineTransform;
    public Vector2 defaultSize;

    private CircleCollider2D collider;

    void OnEnable()
    {
        if (searchLineTransform == null)
        {
            searchLineTransform = GetComponent<RectTransform>();
            collider = GetComponent<CircleCollider2D>();
        }

        searchLineTransform.sizeDelta = defaultSize;
        collider.radius = searchLineTransform.sizeDelta.x / 2;
    }

    void Update()
    {
        if (searchLineTransform.sizeDelta.x > 200)
        {
            gameObject.SetActive(false);
        }
        else
        {
            searchLineTransform.sizeDelta += Vector2.one * 500 * Time.deltaTime;
            collider.radius = searchLineTransform.sizeDelta.x / 2;
        }
    }
}
