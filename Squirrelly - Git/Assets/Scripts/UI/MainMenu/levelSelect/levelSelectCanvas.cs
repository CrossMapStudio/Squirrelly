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
        IMap = inputController.IMAP;

        IMap.MenuActions.DPadE.performed += cnx => navigateMenu(2);
        IMap.MenuActions.DPadW.performed += cnx => navigateMenu(6);

        IMap.MenuActions.DPadN.performed += cnx => navigateMenu(0);
        IMap.MenuActions.DPadS.performed += cnx => navigateMenu(4);

        IMap.MenuActions.Action1.performed += context => callAction();
        IMap.MenuActions.Action2.started += context => backButton.callButtonAction();

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

    //This can change based on desired UI Layout
    public void populateLevelSelect()
    {
        int currentColumnIndex = 0, currentRowIndex = 0;
        //First Button to Start the Chain ---

        foreach (storedLevelData element in gameControl.updatedLevels)
        {
            var clone = Instantiate(levelButton, buttonOrigin.transform);
            levelButton button = clone.GetComponent<levelButton>();
            //clone.GetComponent<Button>().onClick.AddListener(() => { checkButton(button); });
            updateStats(ref button, element);
            levels.Add(button);
            
            menuGrid[currentColumnIndex, currentRowIndex] = clone.GetComponent<menuButton>();
            if (currentColumnIndex == 0) {
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
                button.setPar(element, local.starsEarned, element.displayName);
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
            updateStats(ref el, gameControl.updatedLevels[i]);
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
                    activeButton = current.getNeighbor(switchValue);
                    break;
                }
            }
        }

        resetInput = false;
        StartCoroutine(inputReset());
    }

    private void callAction()
    {
        if (activeButton != null)
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
        IMap.MenuActions.fBumperRight.performed += context => { gModeScroller.scrollOver(-1, 3); updateButtons(); };
        IMap.MenuActions.fBumperLeft.performed += context => { gModeScroller.scrollOver(1, 3); updateButtons(); };

        IMap.MenuActions.bBumperRight.started += context => { diffScroller.scrollOver(-1, 4); updateButtons(); };
        IMap.MenuActions.bBumperLeft.started += context => { diffScroller.scrollOver(1, 4); updateButtons(); };
        populateLevelSelect();
    }
}
