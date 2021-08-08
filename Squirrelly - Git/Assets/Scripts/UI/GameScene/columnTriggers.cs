using GridHandler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class columnTriggers : MonoBehaviour
{
    private Transform targetTransform;
    public gridController gridControl { get; set; }
    public baseUnit associatedUnit { get; set; }

    public Animator buttonAnimator;
    private animationController animController;

    private UIPointTowardCamera UIPoint;
    private void Awake()
    {
        targetTransform = Camera.main.transform;
        UIPoint = new UIPointTowardCamera(transform, targetTransform);
        animController = new animationController(buttonAnimator);
    }

    private void Update()
    {
        UIPoint.onUpdate();
    }

    public void onHover()
    {
        if (associatedUnit != null && !associatedUnit.unitDead)
            gridControl.checkNodeSelection(associatedUnit, false);

        animController.setBool("hover", true);
    }

    public void onExit()
    {
        gridControl.checkNodeSelection(null, false);
        animController.setBool("hover", false);
    }
}

public class UIPointTowardCamera
{
    Transform transform, targetTransform;
    public UIPointTowardCamera(Transform _transform, Transform _targetTransform)
    {
        transform = _transform;
        targetTransform = _targetTransform;
    }

    public void onUpdate()
    {
        Quaternion direction = Quaternion.LookRotation((targetTransform.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * 4f);
    }
}
