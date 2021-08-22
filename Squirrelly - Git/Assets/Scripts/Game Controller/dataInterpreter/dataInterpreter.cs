using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridHandler;
using UnityEditor.UnityLinker;

public class dataInterpreter : MonoBehaviour
{
    public level activeLevel;
    //Create a Grid Gen and this handle all functionality
    public GridGenerator Grid = new GridGenerator();
    public List<GameObject> unitList;
    public LayerMask inputLayer;

    //For interpreting/Gameplay
    public gameController controller;
    public gameStateController gameStateControl;
    public interpreter gameModeInt;
    public gameCanvas gameUI;
    //Grid Texture
    public GameObject columnTriggerButtons, columnTargets, xpPoints;
    public ParticleSystem directionalParticleSystem;
    public Vector3 columnTargetSymbolModifier;

    public float nodeRadius = .5f;
    public Vector2 unitSpacing;
    private GameObject vehicleSpawnController;
    [SerializeField] private string vehicleTag;

    public void Start()
    {
        //Don't Go Here
    }

    public void initializeOnStart()
    {
        var stage = activeLevel.stageElement == null ? Instantiate(activeLevel.defaultStageElement, transform.position, Quaternion.identity) :
        Instantiate(activeLevel.stageElement, transform.position + activeLevel.levelOriginAdjustment, Quaternion.identity);

        vehicleSpawnController = GameObject.FindGameObjectWithTag(vehicleTag);
        vControl = vehicleSpawnController.GetComponent<vehicleController>();

        gameUI = Camera.main.transform.GetChild(0).GetComponent<gameCanvas>();
        Grid.bakeGrid(activeLevel, activeLevel.originPoint, nodeRadius, unitSpacing, unitList, columnTriggerButtons, activeLevel.columnButtonTriggerModifier, columnTargets, columnTargetSymbolModifier, directionalParticleSystem);


        controller = GetComponent<gameController>();
        gameStateControl = new gameStateController(Grid, this);
        gameStateControl.activeUnits = Grid.gridControl.getUnitList;

        var gamemodeIndex = controller.activeStorage.containerIndex;
        var difficultyIndex = controller.activeStorage.difficultyIndex;
        //Will Handle Creating the Correct Interpreter
        switch (gamemodeIndex)
        {
            case 0:
                gameModeInt = new timeModeInt(difficultyIndex, this, gameStateControl, gameUI);
                break;
            case 1:
                gameModeInt = new waveModeInt(difficultyIndex, this, gameStateControl, gameUI);
                break;
            case 2:
                gameModeInt = new highScoreModeInt(difficultyIndex, this, gameStateControl, gameUI);
                break;
        }

        gameController.gameIntroState = true;
        gameStateControl.beginCountDown();
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

    public vehicleController vControl { get; set; }
}
/// <summary>
/// This class will handle the random placement of Units -> and Connection of Nodes
/// </summary>
public class gameStateController
{
    public List<baseUnit> activeUnits;
    public GridGenerator grid;
    private dataInterpreter data;
    private inputHandler inputController;
    public List<coinData> waveCoins;
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
        inputController = data.controller.GetComponent<inputHandler>();
        waveCoins = new List<coinData>();
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

    public void beginCountDown()
    {
        data.gameUI.startCountDown();
    }

    public void waveTrigger()
    {
        //Increment Wave Counter - Give Points - Etc.
        if (gameState != state.start)
        {
            baseCamera.triggerScreenShake(baseCamera.shakePresets.onWaveClear);
            data.gameModeInt.onWaveCompletion();
        }

        List<Vector3> screenPoints = grid.gridControl.clearGrid();
        Vector3 retrieveCenterPoint = data.gameUI.transform.position;

        #region Optimize by including Data all in One List - > Then For Looping the Entirety of the List Comprised of Coin Data
        for (int i = 0; i < screenPoints.Count; i++)
        {
            var clone = Object.Instantiate(data.xpPoints, screenPoints[i], Quaternion.identity, data.gameUI.transform);
            xpCoinTracking element = clone.GetComponent<xpCoinTracking>();
            element.targetPosition = data.gameUI.stats[(int)gameCanvas.stat.scoreCounter].transform;
            element.setValues(xpCoinTracking.coinTypes.singlePoint, (int)gameCanvas.stat.scoreCounter);
        }

        int modifier = 0;
        int tracker = 0;
        int mult = 1;
        for (int i = 0; i < waveCoins.Count; i++)
        {
            Vector3 pos = retrieveCenterPoint;
            modifier = tracker;
            pos.x += modifier;

            var local = -(int)Mathf.Sign(tracker);
            tracker = 0;
            tracker += (local * 50 * mult);

            if ((i + 1) % 2 == 0)
                mult++;

            var clone = Object.Instantiate(data.xpPoints, pos, Quaternion.identity, data.gameUI.transform);
            xpCoinTracking element = clone.GetComponent<xpCoinTracking>();
            element.targetPosition = data.gameUI.stats[(int)gameCanvas.stat.scoreCounter].transform;
            element.setValues(waveCoins[i].type, (int)gameCanvas.stat.scoreCounter);
        }
        #endregion
        waveCoins.Clear();
        grid.startWave();
        if (inputController.activeButtonHover != null)
        {
            inputController.activeButtonHover.onExit();
            inputController.activeButtonHover = null;
        }
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
                gameController.gameEndState = true;
                data.gameModeInt.setUIElements(data.gameUI.endGame(data.gameModeInt.runData));
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
    void onUnitDeath(Transform unit);
    void setRunData(string gameStatus);
    void checkNodeData();
    void onUnitMove();
    void setUIElements(levelCompleteMenu _menu);
    interpreterData runData { get; set; }
}

#region GameMode Interpreters
#region Time Mode
public class timeModeInt : interpreter
{
    #region Members
    gameStateController controller;
    dataInterpreter data;
    gameCanvas gameUI;

    timeGamemode levelData;
    float currentTime, rewardTime, timePlayed;
    int unitDeathCounter, unitMoveCounter;

    bool highScoreAchieved, bestTimeAchieved;

    private int wavesCompleted, wavesCompletedWithinRewardGrouping, targetWaves, currentRewardStatus, currentScore;

    public enum difficulty
    {
        easy = 0,
        moderate = 1,
        hard = 2,
        challenging = 3
    }
    public difficulty currentDifficulty;
    #endregion

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
        gameUI.stats[(int)gameCanvas.stat.timer].text = "Current Wave: " + wavesCompleted + "/" + targetWaves;
        gameUI.stats[(int)gameCanvas.stat.scoreCounter].text = "Current Score: " + runData.currentScore;
        gameUI.stats[(int)gameCanvas.stat.unitsLost].text = "Units Lost: " + runData.unitsLost;
        gameUI.stats[(int)gameCanvas.stat.unitsSaved].transform.parent.gameObject.SetActive(false);

        data.vControl.waveTarget = levelData.gamemodeDifficulties[(int)currentDifficulty].vehicleSpawnIncremental;
        data.vControl.currentSpawnMultiplier = levelData.gamemodeDifficulties[(int)currentDifficulty].vehicleSpawnRatePerWaveIncremental;
        data.vControl.targetSpawnTime = levelData.gamemodeDifficulties[(int)currentDifficulty].startingSpawnTarget;
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
            if (!gameController.pauseState && !gameController.gameEndState && !gameController.gameIntroState)
            {
                currentTime -= Time.deltaTime;
                runData.bestTimeOfCompletion = timePlayed += Time.deltaTime;
                rewardTime += Time.deltaTime;
                
                if (levelData.gamemodeDifficulties[(int)currentDifficulty].timeIntervalForRewards[currentRewardStatus] - timePlayed <= 10f)
                {
                    gameUI.acornAnim.SetBool("acornTrouble", true);
                    gameUI.stats[(int)gameCanvas.stat.gameTimer].color = Color.red;
                }
                else
                {
                    gameUI.stats[(int)gameCanvas.stat.gameTimer].color = Color.white;
                    gameUI.acornAnim.SetBool("acornTrouble", false);
                }
            }

            if (gameUI != null)
            {
                gameUI.stats[(int)gameCanvas.stat.gameTimer].text = "Wave Time: " + timeConversion.convertToTime(currentTime);
                gameUI.stats[(int)gameCanvas.stat.timer].text = "Time Played: " + timeConversion.convertToTime(timePlayed);
                gameUI.stats[(int)gameCanvas.stat.scoreCounter].text = "Current Score: " + runData.currentScore;
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
        if (unitDeathCounter == 0)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fivePoint);
            controller.waveCoins.Add(local);
        }

        if (unitMoveCounter == 1)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.hundredPoint);
            controller.waveCoins.Add(local);
        }
        
        if (unitMoveCounter < 5)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fiftyPoint);
            controller.waveCoins.Add(local);
        }
        
        if (unitMoveCounter < 10)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.tenPoint);
            controller.waveCoins.Add(local);
        }
        
        if (unitMoveCounter < 15)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fivePoint);
            controller.waveCoins.Add(local);
            coinData l = new coinData(xpCoinTracking.coinTypes.singlePoint);
            controller.waveCoins.Add(l);
        }

        if (rewardTime <= 2f)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.hundredPoint);
            controller.waveCoins.Add(local);
        }

        if (rewardTime <= 5f)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fiftyPoint);
            controller.waveCoins.Add(local);
        }

        if (rewardTime <= 8f)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.tenPoint);
            controller.waveCoins.Add(local);
        }

        unitMoveCounter = 0;
        unitDeathCounter = 0;
        rewardTime = 0f;
        currentTime += levelData.gamemodeDifficulties[(int)currentDifficulty].addedTime;
        wavesCompleted++;
        wavesCompletedWithinRewardGrouping++;
        gameUI.stats[(int)gameCanvas.stat.waveCounter].text = "Current Wave: " + wavesCompleted + "/" + targetWaves;
        checkCurrentProgress();
        data.vControl.checkWaveIncrementation();
        baseCamera.audioControl.playSoundOnIndex((int)baseCamera.onePlaySounds.completionSound);
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
        {
            data.controller.activeStorage.setGameDetailsToSave(0, 3 - currentRewardStatus);
            highScoreAchieved = data.controller.activeStorage.setGameDetailsToSave(1, runData.currentScore);
            data.controller.activeStorage.setGameDetailsToSave(2, runData.completedLevel = 1);
            bestTimeAchieved = data.controller.activeStorage.setGameDetailsToSave(3, runData.bestTimeOfCompletion);
        }
    }

    public void onUnitDeath(Transform _unit)
    {
        unitDeathCounter++;
        runData.unitsLost++;
        gameUI.stats[(int)gameCanvas.stat.unitsLost].text = "Units Lost: " + runData.unitsLost;
        currentTime -= levelData.gamemodeDifficulties[(int)currentDifficulty].unitLossTime;
        textGenerator.textOnUnit(_unit, "-"+ levelData.gamemodeDifficulties[(int)currentDifficulty].unitLossTime.ToString()+"s", Color.white);
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
    }

    public void checkNodeData()
    {
        if (controller.grid.gridControl.checkWinNodes())
            controller.setState(1);
    }

    public void onUnitMove()
    {
        unitMoveCounter++;
    }

    public void setUIElements(levelCompleteMenu menu)
    {
        menu.UIElements[(int)levelCompleteMenu.UITargetNames.gameStatus].text = runData.gameStat;
        
        if (highScoreAchieved)
        {
            menu.UIElements[(int)levelCompleteMenu.UITargetNames.scoreText].text = "New High Score: " + runData.currentScore;
        }
        else
        {
            menu.UIElements[(int)levelCompleteMenu.UITargetNames.scoreText].text = "Score: " + runData.currentScore;
        }

        menu.UIElements[(int)levelCompleteMenu.UITargetNames.wavesCompleted].text = "Waves Completed: " + runData.wavesCompleted + "/" + targetWaves;
        menu.UIElements[(int)levelCompleteMenu.UITargetNames.unitsLost].text = "Units Lost: " + runData.unitsLost;
        
        if (bestTimeAchieved)
        {
            menu.UIElements[(int)levelCompleteMenu.UITargetNames.timeOfCompletion].text = "Best Time: " + timeConversion.convertToTime(timePlayed);
        }
        else
        {
            menu.UIElements[(int)levelCompleteMenu.UITargetNames.timeOfCompletion].text = "Time: " + timeConversion.convertToTime(timePlayed);
        }
    }

    public interpreterData runData { get; set; } = new interpreterData();
}
#endregion
#region Wave Mode
public class waveModeInt : interpreter
{
    #region Members
    gameStateController controller;
    dataInterpreter data;
    gameCanvas gameUI;

    waveGamemode levelData;
    float timePlayed, rewardTime;
    int unitDeathCounter, unitDeathTarget, rewardDeathCounter;

    private int wavesCompleted, wavesCompletedWithinRewardGrouping, targetWaves, currentRewardStatus, currentScore, unitMoveCounter;
    bool highScoreAchieved, bestTimeAchieved;
    public enum difficulty
    {
        easy = 0,
        moderate = 1,
        hard = 2,
        challenging = 3
    }
    public difficulty currentDifficulty;
    #endregion
    public waveModeInt(int diff, dataInterpreter _data, gameStateController _controller, gameCanvas _gameUI)
    {
        currentDifficulty = (difficulty)diff;
        controller = _controller;
        data = _data;
        gameUI = _gameUI;
        //Based on difficulty set all Parameters to the Passed Values Correspondence
        levelData = data.controller.currentlySelectedLevel.getWaveMode;

        //Give Value based on Difficulty
        targetWaves = levelData.gamemodeDifficulties[(int)currentDifficulty].wavesToComplete;
        unitDeathTarget = levelData.gamemodeDifficulties[(int)currentDifficulty].unitLossTarget;

        gameUI.stats[(int)gameCanvas.stat.timer].text = "Current Wave: " + wavesCompleted + "/" + targetWaves;
        gameUI.stats[(int)gameCanvas.stat.scoreCounter].text = "Current Score: " + runData.currentScore;
        gameUI.stats[(int)gameCanvas.stat.unitsLost].text = "Units Lost: " + runData.unitsLost;
        gameUI.stats[(int)gameCanvas.stat.gameTimer].transform.parent.gameObject.SetActive(false);
        gameUI.stats[(int)gameCanvas.stat.unitsSaved].transform.parent.gameObject.SetActive(false);

        data.vControl.waveTarget = levelData.gamemodeDifficulties[(int)currentDifficulty].vehicleSpawnIncremental;
        data.vControl.currentSpawnMultiplier = levelData.gamemodeDifficulties[(int)currentDifficulty].vehicleSpawnRatePerWaveIncremental;
        data.vControl.targetSpawnTime = levelData.gamemodeDifficulties[(int)currentDifficulty].startingSpawnTarget;
    }

    public void onUpdate()
    {
        if (unitDeathCounter >= unitDeathTarget)
        {
            if (!gameController.gameEndState)
            {
                controller.gameState = gameStateController.state.lost;
                setRunData("Failed");
                controller.setState(0);
            }
        }
        else
        {
            if (!gameController.pauseState && !gameController.gameEndState && !gameController.gameIntroState)
            {
                runData.bestTimeOfCompletion = timePlayed += Time.deltaTime;
                rewardTime += Time.deltaTime;
            }

            if (gameUI != null)
            {
                //gameUI.timer.text = "Wave Time: " + currentTime.ToString("F2");
                gameUI.stats[(int)gameCanvas.stat.timer].text = "Time Played: " + timeConversion.convertToTime(timePlayed);
                gameUI.stats[(int)gameCanvas.stat.scoreCounter].text = "Current Score: " + runData.currentScore;
            }

            if (currentRewardStatus < 3)
            {
                if (unitDeathCounter >= levelData.gamemodeDifficulties[(int)currentDifficulty].unitLossLimitsForRewards[currentRewardStatus])
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
        if (rewardDeathCounter == 0)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fivePoint);
            controller.waveCoins.Add(local);
        }

        if (unitMoveCounter == 1)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.hundredPoint);
            controller.waveCoins.Add(local);
        }

        if (unitMoveCounter < 5)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fiftyPoint);
            controller.waveCoins.Add(local);
        }

        if (unitMoveCounter < 10)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.tenPoint);
            controller.waveCoins.Add(local);
        }

        if (unitMoveCounter < 15)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fivePoint);
            controller.waveCoins.Add(local);
            coinData l = new coinData(xpCoinTracking.coinTypes.singlePoint);
            controller.waveCoins.Add(l);
        }

        if (rewardTime <= 2f)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.hundredPoint);
            controller.waveCoins.Add(local);
        }

        if (rewardTime <= 5f)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fiftyPoint);
            controller.waveCoins.Add(local);
        }

        if (rewardTime <= 8f)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.tenPoint);
            controller.waveCoins.Add(local);
        }

        unitMoveCounter = 0;
        rewardDeathCounter = 0;
        rewardTime = 0f;
        wavesCompleted++;
        wavesCompletedWithinRewardGrouping++;
        gameUI.stats[(int)gameCanvas.stat.waveCounter].text = "Current Wave: " + wavesCompleted + "/" + targetWaves;
        checkCurrentProgress();
        data.vControl.checkWaveIncrementation();
        baseCamera.audioControl.playSoundOnIndex((int)baseCamera.onePlaySounds.completionSound);
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
        {
            data.controller.activeStorage.setGameDetailsToSave(0, 3 - currentRewardStatus);
            highScoreAchieved = data.controller.activeStorage.setGameDetailsToSave(1, runData.currentScore);
            data.controller.activeStorage.setGameDetailsToSave(2, runData.completedLevel = 1);
            bestTimeAchieved = data.controller.activeStorage.setGameDetailsToSave(3, runData.bestTimeOfCompletion);
        }
        //Rest of the Data Update Here as Well
    }

    public void onUnitDeath(Transform _unit)
    {
        unitDeathCounter++;
        rewardDeathCounter++;
        runData.unitsLost++;
        gameUI.stats[(int)gameCanvas.stat.unitsLost].text = "Units Lost: " + runData.unitsLost;
        textGenerator.textOnUnit(_unit, "-1", Color.white);
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
    }

    public void checkNodeData()
    {
        if (controller.grid.gridControl.checkWinNodes())
            controller.setState(1);
    }

    public void onUnitMove()
    {
        unitMoveCounter++;
    }

    public void setUIElements(levelCompleteMenu menu)
    {
        menu.UIElements[(int)levelCompleteMenu.UITargetNames.gameStatus].text = runData.gameStat;

        if (highScoreAchieved)
        {
            menu.UIElements[(int)levelCompleteMenu.UITargetNames.scoreText].text = "New High Score: " + runData.currentScore;
        }
        else
        {
            menu.UIElements[(int)levelCompleteMenu.UITargetNames.scoreText].text = "Score: " + runData.currentScore;
        }

        menu.UIElements[(int)levelCompleteMenu.UITargetNames.wavesCompleted].text = "Waves Completed: " + runData.wavesCompleted + "/" + targetWaves;
        menu.UIElements[(int)levelCompleteMenu.UITargetNames.unitsLost].text = "Units Lost: " + runData.unitsLost + "/" + unitDeathTarget;

        if (bestTimeAchieved)
        {
            menu.UIElements[(int)levelCompleteMenu.UITargetNames.timeOfCompletion].text = "Best Time: " + timeConversion.convertToTime(timePlayed);
        }
        else
        {
            menu.UIElements[(int)levelCompleteMenu.UITargetNames.timeOfCompletion].text = "Time: " + timeConversion.convertToTime(timePlayed);
        }
    }

    public interpreterData runData { get; set; } = new interpreterData();
}
#endregion
#region High Score Mode
public class highScoreModeInt : interpreter
{
    #region Members
    gameStateController controller;
    dataInterpreter data;
    gameCanvas gameUI;

    highScoreGamemode levelData;
    float currentTime, rewardTime;
    int unitDeathCounter, unitMoveCounter;

    private int wavesCompleted, targetWaves, currentRewardStatus, currentScore;

    //Used for UI and Extra Tokens
    bool highScoreAchieved;

    public enum difficulty
    {
        easy = 0,
        moderate = 1,
        hard = 2,
        challenging = 3
    }
    public difficulty currentDifficulty;
    #endregion

    public highScoreModeInt(int diff, dataInterpreter _data, gameStateController _controller, gameCanvas _gameUI)
    {
        currentDifficulty = (difficulty)diff;
        controller = _controller;
        data = _data;
        gameUI = _gameUI;
        //Based on difficulty set all Parameters to the Passed Values Correspondence
        levelData = data.controller.currentlySelectedLevel.getHighScoreMode;

        //Give Value based on Difficulty
        currentTime = levelData.gamemodeDifficulties[(int)currentDifficulty].startTime;
        //Set UI
        gameUI.stats[(int)gameCanvas.stat.scoreCounter].text = "Current Score: " + runData.currentScore;
        gameUI.stats[(int)gameCanvas.stat.timer].text = "Time: " + timeConversion.convertToTime(currentTime);
        gameUI.stats[(int)gameCanvas.stat.unitsLost].transform.parent.gameObject.SetActive(false);
        gameUI.stats[(int)gameCanvas.stat.unitsSaved].transform.parent.gameObject.SetActive(false);
        gameUI.stats[(int)gameCanvas.stat.waveCounter].transform.parent.gameObject.SetActive(false);
        gameUI.stats[(int)gameCanvas.stat.gameTimer].transform.parent.gameObject.SetActive(false);

        data.vControl.waveTarget = levelData.gamemodeDifficulties[(int)currentDifficulty].vehicleSpawnIncremental;
        data.vControl.currentSpawnMultiplier = levelData.gamemodeDifficulties[(int)currentDifficulty].vehicleSpawnRatePerWaveIncremental;
        data.vControl.targetSpawnTime = levelData.gamemodeDifficulties[(int)currentDifficulty].startingSpawnTarget;

        //Moving Up the Acorns ^
        data.gameUI.acorns[0].color = Color.white;
        data.gameUI.acorns[1].color = Color.white;
        data.gameUI.acorns[2].color = Color.white;
    }

    public void onUpdate()
    {
        if (currentTime <= 0f)
        {
            if (!gameController.gameEndState)
                checkCurrentProgress();
        }
        else
        {
            if (!gameController.pauseState && !gameController.gameEndState && !gameController.gameIntroState)
            {
                currentTime -= Time.deltaTime;
                rewardTime += Time.deltaTime;
            }

            if (gameUI != null)
            {
                gameUI.stats[(int)gameCanvas.stat.timer].text = "Time: " + timeConversion.convertToTime(currentTime);
                gameUI.stats[(int)gameCanvas.stat.scoreCounter].text = "Current Score: " + runData.currentScore;
            }

            if (currentRewardStatus < 3)
            {
                if (runData.currentScore >= levelData.gamemodeDifficulties[(int)currentDifficulty].scoreToReachForAcornReward[currentRewardStatus])
                {
                    data.gameUI.acorns[currentRewardStatus].color = gameUI.acornEarnedColor;
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
        if (unitDeathCounter == 0)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fivePoint);
            controller.waveCoins.Add(local);
        }

        if (unitMoveCounter == 1)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.hundredPoint);
            controller.waveCoins.Add(local);
        }

        if (unitMoveCounter < 5)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fiftyPoint);
            controller.waveCoins.Add(local);
        }

        if (unitMoveCounter < 10)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.tenPoint);
            controller.waveCoins.Add(local);
        }

        if (unitMoveCounter < 15)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fivePoint);
            controller.waveCoins.Add(local);
            coinData l = new coinData(xpCoinTracking.coinTypes.singlePoint);
            controller.waveCoins.Add(l);
        }

        if (rewardTime <= 2f)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.hundredPoint);
            controller.waveCoins.Add(local);
        }

        if (rewardTime <= 5f)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.fiftyPoint);
            controller.waveCoins.Add(local);
        }

        if (rewardTime <= 8f)
        {
            coinData local = new coinData(xpCoinTracking.coinTypes.tenPoint);
            controller.waveCoins.Add(local);
        }

        unitMoveCounter = 0;
        unitDeathCounter = 0;
        rewardTime = 0f;
        wavesCompleted++;
        data.vControl.checkWaveIncrementation();
        baseCamera.audioControl.playSoundOnIndex((int)baseCamera.onePlaySounds.completionSound);
    }

    public void checkCurrentProgress()
    {
        controller.gameState = gameStateController.state.won;
        setRunData("Completed");
        controller.setState(0);
    }

    public void updateAll()
    {
        if (controller.gameState == gameStateController.state.won)
        {
            data.controller.activeStorage.setGameDetailsToSave(0, currentRewardStatus);
            highScoreAchieved = data.controller.activeStorage.setGameDetailsToSave(1, runData.currentScore);
            data.controller.activeStorage.setGameDetailsToSave(2, runData.completedLevel = 1);
            data.controller.activeStorage.setGameDetailsToSave(3, runData.bestTimeOfCompletion);
        }
    }

    public void onUnitDeath(Transform _unit)
    {
        unitDeathCounter++;
        runData.unitsLost++;
        gameUI.stats[(int)gameCanvas.stat.unitsLost].text = "Units Lost: " + runData.unitsLost;
        textGenerator.textOnUnit(_unit, "-1", Color.white);
    }

    public void setRunData(string gameStatus)
    {
        runData.gameStat = gameStatus;
        runData.wavesCompleted = wavesCompleted;
        runData.totalWaves = targetWaves;
        if (controller.gameState == gameStateController.state.won)
            runData.currentRewardStatus = currentRewardStatus;
        else
            runData.currentRewardStatus = 0;
    }

    public void checkNodeData()
    {
        if (controller.grid.gridControl.checkWinNodes())
            controller.setState(1);
    }

    public void onUnitMove()
    {
        unitMoveCounter++;
    }

    public void setUIElements(levelCompleteMenu menu)
    {
        menu.UIElements[(int)levelCompleteMenu.UITargetNames.gameStatus].text = runData.gameStat;
        if (highScoreAchieved)
            menu.UIElements[(int)levelCompleteMenu.UITargetNames.scoreText].text = "New High Score: " + runData.currentScore;
        else
            menu.UIElements[(int)levelCompleteMenu.UITargetNames.scoreText].text = "Score: " + runData.currentScore;

        menu.UIElements[(int)levelCompleteMenu.UITargetNames.wavesCompleted].text = "Waves Completed: " + runData.wavesCompleted;
        menu.UIElements[(int)levelCompleteMenu.UITargetNames.unitsLost].text = "Units Lost: " + runData.unitsLost;
        menu.UIElements[(int)levelCompleteMenu.UITargetNames.timeOfCompletion].gameObject.SetActive(false);
        /*
        gameStatus.text = data.gameStat;
        scoreText.text = "Score: " + data.currentScore.ToString();
        wavesCompleted.text = "Waves Completed: " + data.wavesCompleted.ToString() + "/" + data.totalWaves.ToString();
        unitsLost.text = "Units Lost: " + data.unitsLost.ToString();
        timeOfCompletion.text = "Completed Time: " + data.bestTimeOfCompletion.ToString();
        */
    }
    public interpreterData runData { get; set; } = new interpreterData();
}
#endregion
#endregion

public class interpreterData
{
    public string gameStat;
    public int wavesCompleted;
    public int totalWaves;
    public int currentRewardStatus;
    public int unitsLost;
    public int unitsSaved;
    public int currentScore;
    public int completedLevel;
    public float bestTimeOfCompletion;
}

public struct coinData
{
    public xpCoinTracking.coinTypes type;
    public coinData(xpCoinTracking.coinTypes _type)
    {
        type = _type;
    }
}