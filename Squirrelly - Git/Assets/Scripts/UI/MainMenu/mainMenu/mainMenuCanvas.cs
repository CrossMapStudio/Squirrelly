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

    public GameObject settingsPanel;


    private void Start()
    {
        inputController = GameObject.FindGameObjectWithTag("GameController").GetComponent<inputHandler>();
        inputController.setIMapControlScheme(0);
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

    private void LateUpdate()
    {
        if (inputHandler.inputListener == 1)
        {
            checkNullAction();
            inputHandler.setInputActiveListenerValue(0);
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

    private void checkNullAction()
    {
        if (activeButton.buttonTypeVariation == menuButton.buttonType.nullAction)
        {
            openSettings();
        }
        else
        {
            callAction();
        }
    }

    public void openSettings()
    {
        //Hard Coded for Settings
        settingsPanel.SetActive(true);
    }

    IEnumerator inputReset()
    {
        yield return new WaitForSeconds(.2f);
        resetInput = true;
    }
}
