using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameCanvas : MonoBehaviour
{
    gameController gameControl;
    public Text timer, gameTimer, waveCounter, scoreCounter;
    public GameObject pauseMenuUI, gameCompleteMenuUI;
    private levelCompleteMenu gameCompleteUIScript;

    [SerializeField] private menuButton activeButton;
    private inputHandler inputController;
    [SerializeField] private GameObject loadingScreen;
    private InputMap IMap;
    private bool resetInput = true;
    //Use this for the Acorns Triggers Etc.
    public Color acornEarnedColor;
    public Image[] acorns;
    public Animator acornAnim;
    [HideInInspector]
    public animationController acornAnimController;

    private void Start()
    {
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        inputController = gameControl.GetComponent<inputHandler>();
        inputController.setIMapControlScheme(1);

        IMap = inputController.IMAP;

        IMap.InGamePause.DPadN.performed += context => navigateMenu(0);
        IMap.InGamePause.DPadS.performed += context => navigateMenu(4);
        IMap.InGamePause.Action1.performed += context => checkNullAction();
        IMap.InGamePause.Action2.started += context => pauseMenu(0);
        IMap.InGamePause.Options.started += context => pauseMenu(0);

        gameController.pauseState = false;

        gameCompleteUIScript = gameCompleteMenuUI.GetComponent<levelCompleteMenu>();

        //For Anim
        acornAnimController = new animationController(acornAnim);
    }

    public void waveReset()
    {
        gameControl.gameObject.GetComponent<dataInterpreter>().gameStateControl.waveTrigger();
    }

    public void quitScene() 
    {
        Debug.Log("Saving");
        gameControl.modifyLevelData();
        gameControl.serializationMenu(0);
        gameControl.sceneControl.loadScene(0);
    }

    public void triggerPauseMenuUI()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(gameController.pauseState);
        if (gameController.pauseState)
        {
            if (inputHandler.currentControl == inputHandler.controlSetting.controller)
                activeButton.setHoveringValue = true;
            IMap.InGame.Disable();
            IMap.InGamePause.Enable();
        }
        else
        {
            IMap.InGame.Enable();
            IMap.InGamePause.Disable();
        }
    }

    public void endGame(interpreterData runData)
    {
        //Enum Later for Ease of Reading - 0 is loss 1 is win
        gameCompleteMenuUI.SetActive(true);
        gameCompleteUIScript.setAllUIElements(runData);
        //Update States by Passing from Game Controller
        if (inputHandler.currentControl == inputHandler.controlSetting.controller)
            activeButton.setHoveringValue = true;
        IMap.InGame.Disable();
        IMap.InGamePause.Enable();
        }

    public void pauseMenu(int index)
    {
        switch (index)
        {
            case 0:
                gameController.pauseState = false;
                triggerPauseMenuUI();
                break;
            case 1:
                gameController.pauseState = false;
                gameControl.sceneControl.restart(1);
                break;
            case 2:
                gameController.pauseState = false;
                gameControl.sceneControl.loadScene(0);
                break;
        }
    }
    private void navigateMenu(int switchValue)
    {
        activeButton.setHoveringValue = false;
        activeButton = activeButton.getNeighbor(switchValue) ?? activeButton;
        activeButton.setHoveringValue = true;
        resetInput = false;
            StartCoroutine(inputReset(this));
    }
    private void checkNullAction()
    {
        if (activeButton != null && activeButton.buttonTypeVariation == menuButton.buttonType.nullAction)
        {
            //Hard Coded --- Yikes
            pauseMenu(0);
        }
        else if (activeButton != null)
        {
            activeButton.callButtonAction();
        }
    }

    IEnumerator inputReset(gameCanvas local)
    {
        yield return new WaitForSeconds(.2f);
        if (local != null)
            resetInput = true;
    }
}
