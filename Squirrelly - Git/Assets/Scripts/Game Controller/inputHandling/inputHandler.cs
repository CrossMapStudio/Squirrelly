using System.Collections;
using UnityEngine;

public class inputHandler : MonoBehaviour
{

    /// <summary>
    /// Using Primarily Mouse and Keyboard for Right Now This Input Handler will Control the Players ability to interact with the World
    /// </summary>

    private Camera main;
    private dataInterpreter data;
    private bool fireCast = true;

    public enum controlSetting
    {
        mouseKey,
        controller
    }

    public controlSetting currentControl;

    private void Awake()
    {
        main = Camera.main;
        data = GetComponent<dataInterpreter>();
        //This will pull from a setting class in later builds
        currentControl = controlSetting.mouseKey;
    }

    private void Update()
    {
        if (currentControl == controlSetting.mouseKey)
        {
            if (Input.GetMouseButtonDown(0))
            {
                data.Grid.gridControl.startSwitch();
            }

            if (Input.GetMouseButtonDown(1))
            {
                data.unitElimination();
            }

            //Starts the Pause State
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameController.pauseState = !gameController.pauseState;
                data.gameUI.triggerPauseMenuUI();
            }

            fireCast = !gameController.pauseState;
        }
        else if (currentControl == controlSetting.controller)
        {
            //This will be the build ver for Controller --- Plugin ?
        }
    }

    private void FixedUpdate()
    {
        if (currentControl == controlSetting.mouseKey)
        {
            if (fireCast)
            {
                RaycastHit unitHit;
                Ray ray = main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out unitHit, data.unitLayer))
                {
                    data.Grid.gridControl.checkNodeSelection(unitHit.collider.GetComponent<baseUnit>());
                }
                else
                {
                    data.Grid.gridControl.checkNodeSelection(null);
                }
            }
        }
    }
}
