using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class mainMenuCanvas : MonoBehaviour
{
    [SerializeField] private menuButton activeButton;
    private inputHandler inputController;
    [SerializeField] private GameObject loadingScreen;
    private InputMap IMap;
    private bool resetInput = true;

    private void Start()
    {
        inputController = GameObject.FindGameObjectWithTag("GameController").GetComponent<inputHandler>();
        inputController.setIMapControlScheme(0);
        IMap = inputController.IMAP;

        IMap.MenuActions.DPadE.performed += context => navigateMenu(2);
        IMap.MenuActions.DPadW.performed += context => navigateMenu(6);

        IMap.MenuActions.Action1.performed += context => callAction();

        gameController.pauseState = false;
    }

    private void Update()
    {
        if (inputHandler.currentControl == inputHandler.controlSetting.controller && resetInput) {
            if (inputController.leftAnalogInputValues.x >= .8f)
            {
                navigateMenu(2);
            }
            else if (inputController.leftAnalogInputValues.x <= -.8f)
            {
                navigateMenu(6);
            }
        }
    }

    private void navigateMenu(int switchValue)
    {
        activeButton.setHoveringValue = false;
        activeButton = activeButton.getNeighbor(switchValue) ?? activeButton;
        activeButton.setHoveringValue = true;
        resetInput = false;
        StartCoroutine(inputReset());
    }

    private void callAction()
    {
        if (activeButton != null)
            activeButton.callButtonAction();
    }

    IEnumerator inputReset()
    {
        yield return new WaitForSeconds(.2f);
        resetInput = true;
    }
}
