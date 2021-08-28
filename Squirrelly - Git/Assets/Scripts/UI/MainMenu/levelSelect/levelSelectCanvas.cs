using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelSelectCanvas : MonoBehaviour
{
    [SerializeField] private menuButton activeButton;
    private inputHandler inputController;
    [SerializeField] private GameObject loadingScreen;
    private InputMap IMap;
    public GameObject levelButton, buttonOrigin;
    public int columnCount = 5, rowCount = 2;
    private gameController gameControl;
    private gameData gData;
    //For UI Bases
    private List<levelButton> levels;
    private menuButton[,] menuGrid;
    private levelButton currentActive;

    //Scrollers
    public menuButton gameMode, difficulty, backButton;
    private scrollerButton gModeScroller, diffScroller;

    private bool resetInput = true;
    public bool isCampaignLevels;
    private bool beginDisable;

    private void Awake()
    {
        levels = new List<levelButton>();
    }

    private void Start()
    {
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        menuGrid = new menuButton[columnCount, rowCount];

        StartCoroutine(waitToPopulate());

        inputController = gameControl.GetComponent<inputHandler>();
        inputController.setIMapControlScheme(0);

        gameController.pauseState = false;
    }

    private void Update()
    {
        if (inputHandler.currentControl == inputHandler.controlSetting.controller && resetInput)
        {
            if (inputController.leftAnalogInputValues.x >= .8f)
            {
                navigateMenu(2);
            }
            else if (inputController.leftAnalogInputValues.x <= -.8f)
            {
                navigateMenu(6);
            }

            if (inputController.leftAnalogInputValues.y >= .8f)
            {
                navigateMenu(0);
            }
            else if (inputController.leftAnalogInputValues.y <= -.8f)
            {
                navigateMenu(4);
            }
        }
    }

    private void LateUpdate()
    {

        //Action X and Action Y
        if (inputHandler.inputListener == 1)
        {
            callAction();
            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 2)
        {
            backButton.callButtonAction();
            inputHandler.setInputActiveListenerValue(0);
        }

        //Back Triggers Etc.
        if (inputHandler.inputListener == 5)
        {
            gModeScroller.scrollOver(-1, 3);
            updateButtons();
            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 6)
        {
            gModeScroller.scrollOver(1, 3);
            updateButtons();
            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 7)
        {
            diffScroller.scrollOver(-1, 4);
            updateButtons();
            inputHandler.setInputActiveListenerValue(0);
        }

        if (inputHandler.inputListener == 8)
        {
            diffScroller.scrollOver(1, 4);
            updateButtons();
            inputHandler.setInputActiveListenerValue(0);
        }
    }

    //This can change based on desired UI Layout
    public void populateLevelSelect()
    {
        int currentColumnIndex = 0, currentRowIndex = 0;
        //First Button to Start the Chain ---

        foreach (storedLevelData element in gameControl.updatedLevels)
        {
            if (isCampaignLevels)
            {
                if (element.campaignLevel)
                {
                    var clone = Instantiate(levelButton, buttonOrigin.transform);
                    levelButton button = clone.GetComponent<levelButton>();
                    //clone.GetComponent<Button>().onClick.AddListener(() => { checkButton(button); });
                    updateStats(ref button, element);
                    levels.Add(button);

                    menuGrid[currentColumnIndex, currentRowIndex] = clone.GetComponent<menuButton>();
                    if (currentColumnIndex == 0)
                    {
                        if (currentRowIndex == 0)
                        {
                            activeButton = clone.GetComponent<menuButton>();
                            activeButton.setHoveringValue = inputHandler.currentControl == inputHandler.controlSetting.controller;
                        }
                    }

                    linkButtons(currentColumnIndex, currentRowIndex);
                    currentColumnIndex++;
                    if (currentColumnIndex == 5)
                    {
                        currentColumnIndex = 0;
                        currentRowIndex++;
                    }

                    if (beginDisable)
                    {
                        button.disableButton();
                    }
                    else
                    {
                        if (element.retrieveStorage().completed == 1)
                        {
                            continue;
                        }
                        else
                        {
                            beginDisable = true;
                        }
                    }
                }
            }
            else
            {
                if (!element.campaignLevel)
                {
                    var clone = Instantiate(levelButton, buttonOrigin.transform);
                    levelButton button = clone.GetComponent<levelButton>();
                    //clone.GetComponent<Button>().onClick.AddListener(() => { checkButton(button); });
                    updateStats(ref button, element);
                    levels.Add(button);

                    menuGrid[currentColumnIndex, currentRowIndex] = clone.GetComponent<menuButton>();
                    if (currentColumnIndex == 0)
                    {
                        if (currentRowIndex == 0)
                        {
                            activeButton = clone.GetComponent<menuButton>();
                            activeButton.setHoveringValue = inputHandler.currentControl == inputHandler.controlSetting.controller;
                        }
                    }

                    linkButtons(currentColumnIndex, currentRowIndex);
                    currentColumnIndex++;
                    if (currentColumnIndex == 5)
                    {
                        currentColumnIndex = 0;
                        currentRowIndex++;
                    }
                }
            }
        }
    }

    private void linkButtons(int column, int row)
    {
        if (column - 1 >= 0)
        {
            menuGrid[column, row].westButton = menuGrid[column - 1, row];
            menuGrid[column - 1, row].eastButton = menuGrid[column, row];
        }
        if (row - 1 >= 0)
        {
            menuGrid[column, row].northButton = menuGrid[column, row - 1];
            menuGrid[column, row - 1].southButton = menuGrid[column, row];
        }
    }

    void updateStats(ref levelButton button, storedLevelData element)
    {
        container current = element.getContainer(element.gameModeTags[gModeScroller.currentSelection]);
        if (diffScroller.currentSelection >= current.dataSize || current.dataSize == 0)
        {
            if (gameControl.compareCurrentLevel(element.id))
            {
                gameControl.currentlySelectedLevel = null;
            }

            button.gameObject.SetActive(false);
        }
        else
        {
            if (button != null)
            {
                button.gameObject.SetActive(true);
                gameInfo local = current.getGameInfo(diffScroller.currentSelection);
                button.setPar(element, local.starsEarned, element.displayName, local.scoreEarned);
                //For Pulling Save Later in Game --- Can Optionally Clear these before Return
                element.difficultyIndex = diffScroller.currentSelection;
                element.containerIndex = gModeScroller.currentSelection;
            }
        }
    }

    public void updateButtons()
    {
        gameControl.currentlySelectedLevel = null;

        for (int i = 0; i < menuGrid.GetLength(0); i++)
        {
            for (int j = 0; j < menuGrid.GetLength(1); j++)
            {
                if (menuGrid[i,j] != null)
                    menuGrid[i, j].setHoveringValue = false;
            }
        }

        for (int i = 0; i < levels.Count; i++)
        {
            levelButton el = levels[i];
            updateStats(ref el, gameControl.updatedLevels[el.levelData.updatedLevelIndex]);
        }
    }

    public void checkButton(levelButton btn)
    {
        if (currentActive != null)
            //currentActive.setStatus(false);
        currentActive = btn;
        //currentActive.setStatus(true);
    }

    public void launchGame()
    {
        gameControl.launchActiveLevel();
    }

    //This is used for the Menu Buttons
    public void callSer(int index)
    {
        if (gameControl != null)
            gameControl.serializationMenu(index);
    }

    private void navigateMenu(int switchValue)
    {
        if (!sceneManager.inLoadState)
        {
            if (activeButton != null)
                activeButton.setHoveringValue = false;
            var clone = activeButton;
            var current = activeButton;
            activeButton = null;
            while (activeButton == null)
            {
                if (current.getNeighbor(switchValue) == null)
                {
                    activeButton = clone;
                    activeButton.setHoveringValue = true;
                    break;
                }
                else
                {
                    if (!current.getNeighbor(switchValue).gameObject.activeSelf)
                    {
                        current = current.getNeighbor(switchValue);
                        continue;
                    }
                    else
                    {
                        var local = new levelButton();
                        if ((local = current.getNeighbor(switchValue).GetComponent<levelButton>()) != null)
                        {
                            if (local.disabled)
                            {
                                activeButton = clone;
                                activeButton.setHoveringValue = true;
                                break;
                            }
                        }

                        activeButton = current.getNeighbor(switchValue);
                        break;
                    }
                }
            }

            activeButton.setHoveringValue = true;
            resetInput = false;
            StartCoroutine(inputReset());
        }
    }

    private void callAction()
    {
        if (activeButton != null && !sceneManager.inLoadState)
            activeButton.callButtonAction();
    }

    public void mouseSelectScrollOptionsGameMode(int direction)
    {
        gModeScroller.scrollOver(direction, 3);
        updateButtons();
    }

    public void mouseSelectScrollOptionsDifficulty(int direction)
    {
        diffScroller.scrollOver(direction, 4);
        updateButtons();
    }

    IEnumerator inputReset()
    {
        yield return new WaitForSeconds(.2f);
        resetInput = true;
    }

    IEnumerator waitToPopulate()
    {
        yield return new WaitForFixedUpdate();
        gModeScroller = gameMode.getScrollButton;
        diffScroller = difficulty.getScrollButton;
        populateLevelSelect();
    }
}
