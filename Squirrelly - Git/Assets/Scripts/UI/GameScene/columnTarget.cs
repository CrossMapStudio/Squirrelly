using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class columnTarget : MonoBehaviour
{
    UIPointTowardCamera UIPoint;
    Transform targetTransform;
    // Start is called before the first frame update
    void Awake()
    {
        //Hard Coded Camera Transform for Target
        targetTransform = Camera.main.transform;
        UIPoint = new UIPointTowardCamera(transform, targetTransform);
    }

    // Update is called once per frame
    void Update()
    {
        UIPoint.onUpdate(); 
    }
}
