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
    //Grid Texture
    public GameObject gridTexture;

    public float nodeRadius = .5f;
    public Vector2 unitSpacing;

    public void initializeOnStart()
    {
        gameUI = Camera.main.transform.GetChild(0).GetComponent<gameCanvas>();
        Grid.bakeGrid(activeLevel, activeLevel.originPoint, nodeRadius, unitSpacing, unitList, gridTexture);
        gameStateControl = new gameStateController(Grid, this);

        gameStateControl.activeUnits = Grid.gridControl.getUnitList;

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
        if (gameStateControl != null && gameStateControl.gameState != gameStateController.state.none)
        {
            gameStateControl.update();
            if (gameModeInt != null)
                gameModeInt.onUpdate();
        }
    }

    public void clear()
    {
        gameUI = null;
        gameStateControl = null;
        gameModeInt = null;
    }
}
/// <summary>
/// This class will handle the random placemeent of Units -> and Connection of Nodes
/// </summary>

public class gameStateController
{
    public List<baseUnit> activeUnits;
    public GridGenerator grid;
    private dataInterpreter data;
    public enum state
    {
        start,
        playing,
        cleared,
        won,
        lost,
        none
    }
    public state gameState = state.start;
    public gameStateController(GridGenerator _grid, dataInterpreter _data, int totalNum = 0)
    {
        grid = _grid;
        data = _data;
        waveTrigger();
    }

    public void update()
    {
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

        grid.startWave();
        gameState = state.playing;
    }

    public void setState(int index)
    {
        switch (index)
        {
            //End Game
            case 0:
                data.gameModeInt.updateAll();
                serializationHandler.saveGame(serializationHandler.fileTag, data.controller.getGameDataForSave);
                data.Grid.gridControl.clearGrid();
                gameController.pauseState = true;
                data.gameUI.endGame(data.gameModeInt.runData);
                gameState = state.none;
                break;
            case 1:
                gameState = state.cleared;
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
    void updateAll();
    void onUnitDeath();
    void setRunData(string gameStatus);
    void checkNodeData();
    interpreterData runData { get; set; }
}

public class timeModeInt : interpreter
{
    gameStateController controller;
    dataInterpreter data;
    gameCanvas gameUI;

    timeGamemode levelData;
    float currentTime, rewardTime, timePlayed;

    private int wavesCompleted, wavesCompletedWithinRewardGrouping, targetWaves, currentRewardStatus, currentScore;

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
        targetWaves = levelData.gamemodeDifficulties[(int)currentDifficulty].wavesToComplete;

        //Set UI
        gameUI.waveCounter.text = "Current Wave: " + wavesCompleted + "/" + targetWaves;
        gameUI.scoreCounter.text = "Current Score: " + currentScore + "/" + targetWaves;
    }

    public void onUpdate()
    {
        if (currentTime <= 0f)
        {
            if (!gameController.gameEndState)
                controller.gameState = gameStateController.state.lost;
                setRunData("Failed");
                controller.setState(0);
        }
        else
        {
            if (!gameController.pauseState && !gameController.gameEndState)
            {
                currentTime -= Time.deltaTime;
                timePlayed += Time.deltaTime;
                rewardTime += Time.deltaTime;
                
                if (levelData.gamemodeDifficulties[(int)currentDifficulty].timeIntervalForRewards[currentRewardStatus] - timePlayed <= 10f)
                {
                    gameUI.acornAnim.SetBool("acornTrouble", true);
                    gameUI.gameTimer.color = Color.red;
                }
                else
                {
                    gameUI.gameTimer.color = Color.white;
                    gameUI.acornAnim.SetBool("acornTrouble", false);
                }
            }

            if (gameUI != null)
            {
                gameUI.timer.text = "Wave Time: " + currentTime.ToString("F2");
                gameUI.gameTimer.text = "Time Played: " + timePlayed.ToString("F2");
            }

            if (currentRewardStatus < 3)
            {
                if (timePlayed >= levelData.gamemodeDifficulties[(int)currentDifficulty].timeIntervalForRewards[currentRewardStatus])
                {
                    data.gameUI.acorns[currentRewardStatus].color = Color.white;
                    switch (currentRewardStatus)
                    {
                        case 0:
                            data.gameUI.acornAnimController.setTrigger("firstAcornEarned");
                            break;
                        case 1:
                            data.gameUI.acornAnimController.setTrigger("secondAcornEarned");
                            break;
                        case 2:
                            data.gameUI.acornAnimController.setTrigger("thirdAcornEarned");
                            break;
                    }

                    currentRewardStatus++;
                }
            }
        }
    }
     
    public void onWaveCompletion()
    {
        currentTime += levelData.gamemodeDifficulties[(int)currentDifficulty].addedTime;
        wavesCompleted++;
        wavesCompletedWithinRewardGrouping++;
        gameUI.waveCounter.text = "Current Wave: " + wavesCompleted + "/" + targetWaves;
        checkCurrentProgress();
        Debug.Log("Time Added! -> " + currentTime);
    }

    public void checkCurrentProgress()
    {
        if (wavesCompleted >= targetWaves && targetWaves != 0)
        {
            controller.gameState = gameStateController.state.won;
            setRunData("Completed");
            controller.setState(0);
        }
    }

    public void updateAll()
    {
        if (controller.gameState == gameStateController.state.won)
            data.controller.activeStorage.addStar(3 - currentRewardStatus);
        //Rest of the Data Update Here as Well
    }

    public void onUnitDeath()
    {

    }

    public void setRunData(string gameStatus)
    {
        runData.gameStat = gameStatus;
        runData.wavesCompleted = wavesCompleted;
        runData.totalWaves = targetWaves;
        if (controller.gameState == gameStateController.state.won)
            runData.currentRewardStatus = 3 - currentRewardStatus;
        else
            runData.currentRewardStatus = 0;
        runData.unitsLost = 0;
        runData.currentScore = currentScore;
    }

    public void checkNodeData()
    {
        if (controller.grid.gridControl.checkWinNodes())
            controller.setState(1);
    }

    public interpreterData runData { get; set; } = new interpreterData();
}

public class interpreterData
{
    public string gameStat;
    public int wavesCompleted;
    public int totalWaves;
    public int currentRewardStatus;
    public int unitsLost;
    public int currentScore;
}