using GridHandler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private enum inputMapController
    {
        WB,
        SB,
        NB,
        EB,
        L1,
        L2,
        R1,
        R2
    }

    private enum inputMapKeyMouse
    {
        A,
        W,
        S,
        D,
        LA,
        UP,
        DA,
        RA
    }
    public TextMesh inputText;
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

    public void setInput(int controllerType, int input)
    {
        switch (controllerType)
        {
            case 0:
                inputMapController ps = (inputMapController)input;
                inputText.text = ps.ToString();
                break;
            case 1:
                inputMapKeyMouse keyMouse = (inputMapKeyMouse)input;
                inputText.text = keyMouse.ToString();
                break;
        }
    }

    public void onHover()
    {
        if (associatedUnit != null && !associatedUnit.unitDead)
        {
            gridControl.checkNodeSelection(associatedUnit, false);
            animController.setBool("hover", true);
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
        Vector3 rotDir = targetTransform.position - transform.position;
        rotDir.z = 0f;
        Quaternion direction = Quaternion.LookRotation((rotDir).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * 4f);
    }
}
