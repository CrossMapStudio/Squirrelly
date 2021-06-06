using System.Collections.Generic;
using UnityEngine;
using GridHandler;

public class dataInterpreter : MonoBehaviour
{
    public level activeLevel;
    //Create a Grid Gen and this handle all functionality
    public GridGenerator Grid = new GridGenerator();
    public List<GameObject> unitList;
    public LayerMask unitLayer;

    //For interpreting/Gameplay
    public gameStateController gameStateControl;

    public void initializeOnStart()
    {
        Grid.bakeGrid(activeLevel, activeLevel.originPoint, .5f, unitList);
        gameStateControl = new gameStateController(Grid, (int)activeLevel.gridSize.x);
    }

    private void Update()
    {
        if (gameStateControl != null)
            gameStateControl.update();
    }
}
/// <summary>
/// This class will handle the random placemeent of Units -> and Connection of Nodes
/// </summary>

public class gameStateController
{
    private List<baseUnit> activeUnits, winStateList;
    private int winNum;
    private GridGenerator grid;
    private enum state
    {
        playing,
        cleared,
        won,
        lost
    }
    private state gameState = state.playing;
    public gameStateController(GridGenerator _grid, int totalNum = 0)
    {
        activeUnits = new List<baseUnit>();
        winNum = totalNum;
        grid = _grid;
        waveTrigger();
    }

    public void update()
    {
        gameState = baseUnit.finalList.Count == winNum ? state.cleared : state.playing;
        if (gameState == state.cleared)
        {
            //units animate and make em do dance or something ->
            waveTrigger();
            Debug.Log("Wave Complete");
        }
    }

    public void waveTrigger()
    {
        //Increment Wave Counter - Give Points - Etc.
        baseUnit.finalList.Clear();
        activeUnits = grid.startWave();
        gameState = state.playing;
    }
}