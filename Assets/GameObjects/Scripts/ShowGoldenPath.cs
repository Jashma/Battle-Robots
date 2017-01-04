using UnityEngine;
using System.Collections;

public class ShowGoldenPath : MonoBehaviour
{
    public NavMeshPath path;
    private float elapsed = 0.0f;
    public Vector3[] wayPoints;
    public Color color;

    void Start()
    {
        path = new NavMeshPath();
        elapsed = 0.0f;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
        }
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
    }
}
