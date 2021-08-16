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

    //Need for Control on Camera
    private Camera main;
    private baseCamera cameraController;
    private bool disabled;
    private void Awake()
    {
        main = Camera.main;
        cameraController = main.GetComponent<baseCamera>();
        targetTransform = Camera.main.transform;
        UIPoint = new UIPointTowardCamera(transform, targetTransform);
        animController = new animationController(buttonAnimator);
    }

    private void Update()
    {
        UIPoint.onUpdate();

        if (!disabled && associatedUnit == null || associatedUnit.unitDead)
        {
            disabled = true;
            disableColumnTrigger();
        }
        else if (disabled && associatedUnit != null && !associatedUnit.unitDead)
        {
            disabled = false;
            enableColumnTrigger();
        }
    }

    public void onHover()
    {
        if (associatedUnit != null && !associatedUnit.unitDead)
        {
            gridControl.checkNodeSelection(associatedUnit, false);

            animController.setBool("hover", true);
            //cameraController.zoomToggle(true, 1f);
        }
    }

    public void onExit()
    {
        gridControl.checkNodeSelection(null, false);
        animController.setBool("hover", false);
    }

    public void disableColumnTrigger()
    {
        animController.setBool("disabled", true);
    }

    public void enableColumnTrigger()
    {
        animController.setBool("disabled", false);
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
