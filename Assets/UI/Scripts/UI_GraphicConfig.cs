using UnityEngine;
using System.Collections;

public class UI_GraphicConfig : MonoBehaviour 
{
public RectTransform contentTransform;

    public int contentElement;

    public void OnEnable()
   {
       contentTransform.sizeDelta = Vector2.up * (contentElement * 60);
	}
}
