using UnityEngine;

public class inputHandler : MonoBehaviour
{

    /// <summary>
    /// Using Primarily Mouse and Keyboard for Right Now This Input Handler will Control the Players ability to interact with the World
    /// </summary>

    private Camera main;
    private dataInterpreter data;
    private bool fireCast = true;
    private InputMap IMap;
    private Vector2 leftAnalogCurrentValue;

    public enum controlSetting
    {
        mouseKey,
        controller
    }

    public static controlSetting currentControl;

    private enum inputAction
    {
        westButton,
        southButton,
        northButton,
        eastButton,
        leftFront,
        leftBack,
        rightFront,
        rightBack
    }

    private inputAction activeInput;

    private KeyCode[] keys = { 
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E,
        KeyCode.R,
        KeyCode.T,
        KeyCode.Y,
        KeyCode.U,
        KeyCode.I
    };

    private void Awake()
    {
        IMap = new InputMap();
        //Analog Sticks
        IMap.MenuActions.LeftStick.performed += context => leftAnalogCurrentValue = context.ReadValue<Vector2>();
        IMap.MenuActions.LeftStick.canceled += context => leftAnalogCurrentValue = Vector2.zero;
        main = Camera.main;
        //Game Actions
        IMap.InGame.WestAction.started += context => { activeInput = inputAction.westButton; gridInputCheck(); };
        IMap.InGame.SouthAction.started += context => { activeInput = inputAction.southButton; gridInputCheck(); };
        IMap.InGame.NorthAction.started += context => { activeInput = inputAction.northButton; gridInputCheck(); };
        IMap.InGame.EastAction.started += context => { activeInput = inputAction.eastButton; gridInputCheck(); };
        IMap.InGame.LeftFrontBumper.started += context => { activeInput = inputAction.leftFront; gridInputCheck(); };
        IMap.InGame.LeftBackBumper.started += context => { activeInput = inputAction.leftBack; gridInputCheck(); };
        IMap.InGame.RightFrontBumper.started += context => { activeInput = inputAction.rightFront; gridInputCheck(); };
        IMap.InGame.RightBackBumper.started += context => { activeInput = inputAction.rightBack; gridInputCheck(); };
        IMap.InGame.Options.started += context =>
        {
            gameController.pauseState = !gameController.pauseState;
            data.gameUI.triggerPauseMenuUI();
        };

        //Turn off in-game, enable menu
        setIMapControlScheme(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentControl = controlSetting.mouseKey;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            currentControl = controlSetting.controller;

        if (currentControl == controlSetting.controller)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (currentControl == controlSetting.mouseKey)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (!gameController.pauseState && currentControl == controlSetting.mouseKey)
        {
            if (Input.GetMouseButtonDown(0))
                if (data != null)
                    data.Grid.gridControl.startSwitch();

            for (int i = 0; i < keys.Length; i++)
            {
                if (Input.GetKeyDown(keys[i]))
                {
                    activeInput = (inputAction)i;
                    gridInputCheck();
                }
            }
        }
        
        if (currentControl == controlSetting.mouseKey)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                if (data != null)
                    gameController.pauseState = !gameController.pauseState;
                if (data != null && data.gameUI != null)
                    data.gameUI.triggerPauseMenuUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            errorHandler.printMessage("File Clearing Attempt -");
            serializationHandler.clearFile(serializationHandler.fileTag);
        }
    }

    private void FixedUpdate()
    {
        if (currentControl == controlSetting.mouseKey && data)
        {
            if (fireCast && !gameController.pauseState)
            {
                RaycastHit unitHit;
                if (!main)
                {
                    main = Camera.main;
                    return;
                }

                Ray ray = main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out unitHit, data.unitLayer))
                {
                    data.Grid.gridControl.checkNodeSelection(unitHit.collider.GetComponent<baseUnit>(), false);
                }
                else
                {
                    data.Grid.gridControl.checkNodeSelection(null, false);
                }
            }
        }
    }

    private void gridInputCheck()
    {
        if ((int)activeInput < data.gameStateControl.activeUnits.Count)
            data.Grid.gridControl.checkNodeSelection(data.gameStateControl.activeUnits[(int)activeInput]);
    }

    public void setIMapControlScheme(int controlSchemeIndex)
    {
        switch (controlSchemeIndex)
        {
            case 0:
                IMap.MenuActions.Enable();
                IMap.InGame.Disable();
                IMap.InGamePause.Disable();
                data = null;
                break;
            case 1:
                IMap.InGame.Enable();
                IMap.MenuActions.Disable();
                data = GetComponent<dataInterpreter>();
                break;
        }
    }

    public Vector2 leftAnalogInputValues { get { return leftAnalogCurrentValue; } }
    public InputMap IMAP { get { return IMap; } }
}
