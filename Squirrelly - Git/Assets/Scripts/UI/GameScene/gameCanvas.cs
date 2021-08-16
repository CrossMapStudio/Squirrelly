using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameCanvas : MonoBehaviour
{
    gameController gameControl;
    public GameObject pauseMenuUI, gameCompleteMenuUI, xpCoin, addedTextContainer;
    private levelCompleteMenu gameCompleteUIScript;

    [SerializeField] private menuButton activeButtonPauseMenu, activeButtonGameCompletionMenu;
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

    //Holds the Animated Stats Elements
    public GameObject statsLayoutGroup;
    [HideInInspector] public List<Text> stats;
    //Used Incase Elements are Missing
    private Dictionary<string, UIStats> animatorDictionary;
    private string statHolderBaseName = "UIStatHolder";
    public string[] UIStatAnimations;

    public enum stat
    {
        timer,
        gameTimer,
        waveCounter,
        scoreCounter,
        unitsSaved,
        unitsLost
    }

    private void Awake()
    {
        animatorDictionary = new Dictionary<string, UIStats>();
        for (int i = 0; i < statsLayoutGroup.transform.childCount; i++)
        {
            GameObject child = statsLayoutGroup.transform.GetChild(i).gameObject;
            var animatorClone = child.GetComponent<Animator>();
            var textClone = child.transform.GetChild(0).GetComponent<Text>();

            UIStats statHolder = new UIStats();
            statHolder.anim = animatorClone;
            statHolder.worldPos = animatorClone.gameObject.transform.position;
            animatorDictionary.Add(animatorClone.gameObject.name, statHolder);
            stats.Add(textClone);
        }
    }

    private void Start()
    {
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        inputController = gameControl.GetComponent<inputHandler>();
        inputController.setIMapControlScheme(1);
        gameController.pauseState = false;
        gameCompleteUIScript = gameCompleteMenuUI.GetComponent<levelCompleteMenu>();
        //For Anim
        acornAnimController = new animationController(acornAnim);
    }

    private void LateUpdate()
    {
        #region In-Game Menu Controller Input Listener
        if (inputHandler.inputListener == 9)
        {
            navigateMenu(0);
            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 10)
        {
            navigateMenu(4);
            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 11 && gameController.pauseState)
        {
            pauseMenu(0);
            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 2 && gameController.pauseState)
        {
            pauseMenu(0);
            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 1)
        {
            checkNullAction();
            inputHandler.setInputActiveListenerValue(0);
        }
        #endregion
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
                activeButtonPauseMenu.setHoveringValue = true;
            inputController.setIMapControlScheme(2);
        }
        else
        {
            inputController.setIMapControlScheme(3);
        }
    }

    public void endGame(interpreterData runData)
    {
        //Enum Later for Ease of Reading - 0 is loss 1 is win
        gameCompleteMenuUI.SetActive(true);
        gameCompleteUIScript.setAllUIElements(runData);
        //Update States by Passing from Game Controller
        if (inputHandler.currentControl == inputHandler.controlSetting.controller)
            activeButtonGameCompletionMenu.setHoveringValue = true;
        inputController.setIMapControlScheme(2);
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
        if (gameController.pauseState)
        {
            activeButtonPauseMenu.setHoveringValue = false;
            activeButtonPauseMenu = activeButtonPauseMenu.getNeighbor(switchValue) ?? activeButtonPauseMenu;
            activeButtonPauseMenu.setHoveringValue = true;
        }
        else
        {
            activeButtonGameCompletionMenu.setHoveringValue = false;
            activeButtonGameCompletionMenu = activeButtonGameCompletionMenu.getNeighbor(switchValue) ?? activeButtonGameCompletionMenu;
            activeButtonGameCompletionMenu.setHoveringValue = true;
        }

        resetInput = false;
        StartCoroutine(inputReset());
    }
    private void checkNullAction()
    {
        if (gameController.pauseState)
        {
            if (activeButtonPauseMenu != null && activeButtonPauseMenu.buttonTypeVariation == menuButton.buttonType.nullAction)
            {
                //Hard Coded --- Yikes
                pauseMenu(0);
            }
            else if (activeButtonPauseMenu != null)
            {
                activeButtonPauseMenu.callButtonAction();
            }
        }
        else
        {
            activeButtonGameCompletionMenu.callButtonAction();
        }
    }

    IEnumerator inputReset()
    {
        yield return new WaitForSeconds(.2f);
        resetInput = true;
    }

    public void playUIStatAnimation(string keyModifier, int animationKey, string addedTextValue = "")
    {
        var clone = animatorDictionary[statHolderBaseName + keyModifier];
        var added = Instantiate(addedTextContainer, clone.anim.gameObject.transform.position + new Vector3(Random.Range(-50,50), 0f, 0f), Quaternion.identity, clone.anim.gameObject.transform);
        added.transform.GetChild(0).GetComponent<Text>().text = addedTextValue;

        Destroy(added, 1.5f);
        clone.anim.playbackTime = 0f;
        clone.anim.Play(UIStatAnimations[animationKey]);
    }
}

struct UIStats
{
    public Vector3 worldPos;
    public Animator anim;
    public Text addedText;
}
