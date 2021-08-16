using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailFollow : MonoBehaviour
{
    public Transform target;
    private void LateUpdate()
    {
        if (target != null)
            transform.position = target.position;
    }
}
