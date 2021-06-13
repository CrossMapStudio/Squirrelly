using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridHandler;

public class dataInterpreter : MonoBehaviour
{
    public level activeLevel;
    //Create a Grid Gen and this handle all functionality
    public GridGenerator Grid = new GridGenerator();
    public List<GameObject> unitList;
    public LayerMask unitLayer;

    //For interpreting/Gameplay
    public gameController controller;
    public gameStateController gameStateControl;
    public interpreter gameModeInt;
    public gameCanvas gameUI;

    private void Awake()
    {
        //Used for Intepreter
        gameUI = Camera.main.transform.GetChild(0).GetComponent<gameCanvas>();
    }

    public void initializeOnStart()
    {
        Grid.bakeGrid(activeLevel, activeLevel.originPoint, .5f, unitList);
        gameStateControl = new gameStateController(Grid, this);

        controller = GetComponent<gameController>();
        var gamemodeIndex = controller.activeStorage.containerIndex;
        var difficultyIndex = controller.activeStorage.difficultyIndex;
        //Will Handle Creating the Correct Interpreter
        switch (gamemodeIndex)
        {
            default:
                gameModeInt = new timeModeInt(difficultyIndex, this, gameStateControl, gameUI);
                break;
        }

        //Generate the Level Stage Element --- Can Also Hold Load until this is finished + If we want to do baked lighting
        var stage = activeLevel.stageElement == null ? Instantiate(activeLevel.defaultStageElement, transform.position, Quaternion.identity) : 
            Instantiate(activeLevel.stageElement, transform.position + activeLevel.levelOriginAdjustment, Quaternion.identity);
    }

    private void Update()
    {
        if (gameStateControl != null)
            gameStateControl.update();

        if (gameModeInt != null)
            gameModeInt.onUpdate();
    }

    public void unitElimination()
    {
        if (Grid.gridControl.deleteUnit())
            gameStateControl.adjustWinNumber();
    }
}
/// <summary>
/// This class will handle the random placemeent of Units -> and Connection of Nodes
/// </summary>

public class gameStateController
{
    private List<baseUnit> activeUnits;
    private int winNum;
    private GridGenerator grid;
    private dataInterpreter data;
    public static int compareNum;
    private enum state
    {
        start,
        playing,
        cleared,
        won,
        lost
    }
    private state gameState = state.start;
    public gameStateController(GridGenerator _grid, dataInterpreter _data, int totalNum = 0)
    {
        winNum = totalNum;
        grid = _grid;
        data = _data;
        waveTrigger();
    }

    public void update()
    {
        gameState = compareNum >= winNum ? state.cleared : gameState;
        if (gameState == state.cleared)
        {
            //units animate and make em do dance or something ->
            waveTrigger();
        }
    }

    public void waveTrigger()
    {
        //Increment Wave Counter - Give Points - Etc.aw
        if (gameState != state.start)
            data.gameModeInt.onWaveCompletion();

        winNum = grid.startWave().Count;
        gameState = state.playing;
    }

    public void adjustWinNumber()
    {
        winNum--;
    }

    public void setState(int index)
    {
        switch (index)
        {
            case 0:
                if (gameState != state.lost)
                {
                    gameState = state.lost;
                    data.Grid.gridControl.clearGrid();
                    gameController.pauseState = true;
                    data.gameUI.endGame();
                }
                break;
            case 1:
                break;
            default:
                break;
        }
    }
}

public interface interpreter
{
    void onUpdate();
    void onWaveCompletion();
}

public class timeModeInt : interpreter
{
    gameStateController controller;
    dataInterpreter data;
    gameCanvas gameUI;

    timeGamemode levelData;
    float currentTime, targetTime;

    public enum difficulty
    {
        easy = 0,
        moderate = 1,
        hard = 2,
        challenging = 3
    }
    public difficulty currentDifficulty;
    public timeModeInt(int diff, dataInterpreter _data, gameStateController _controller, gameCanvas _gameUI)
    {
        currentDifficulty = (difficulty)diff;
        controller = _controller;
        data = _data;
        gameUI = _gameUI;
        //Based on difficulty set all Parameters to the Passed Values Correspondence
        levelData = data.controller.currentlySelectedLevel.getTimeMode;

        //Give Value based on Difficulty
        currentTime = levelData.gamemodeDifficulties[(int)currentDifficulty].startTime;
    }

    public void onUpdate()
    {
        if (currentTime <= 0f)
        {
            if (!gameController.pauseState)
                controller.setState(0);
        }
        else
        {
            if (!gameController.pauseState)
                currentTime -= Time.deltaTime;
            gameUI.timer.text = currentTime.ToString("F2");
        }
    }

    public void onWaveCompletion()
    {
        currentTime += levelData.gamemodeDifficulties[(int)currentDifficulty].addedTime;
        Debug.Log("Time Added! -> " + currentTime);
    }
}