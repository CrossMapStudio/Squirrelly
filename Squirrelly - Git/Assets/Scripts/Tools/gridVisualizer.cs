using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridVisualizer : MonoBehaviour
{
    public float xParam, zParam;
    public Vector3 origin;
    public Color gridColor;

    public void OnDrawGizmos()
    {
        Gizmos.color = gridColor;
        Gizmos.DrawWireCube(origin, new Vector3(xParam, 0f, zParam));
    }
}
