using UnityEngine;
using System.Collections;

public class CheckVisibleMesh : MonoBehaviour 
{
    public bool camVisible;

    void OnBecameVisible()
    {
        camVisible = true;
    }

    void OnBecameInvisible()
    {
        camVisible = false;
    }
}
