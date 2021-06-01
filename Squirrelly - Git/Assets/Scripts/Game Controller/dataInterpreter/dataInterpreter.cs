using System.Collections.Generic;
using UnityEngine;

public class dataInterpreter : MonoBehaviour
{
    level activeLevel;
    public bool displayGridGizmos = true;
    [SerializeField] private float nodeRadius = .5f;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    Vector3 originPoint;

    //Controller for the Game Loop
    gridController gridControl;
    gameStateController gameStateControl;

    public gameController parent { get; set; }
    public LayerMask unitLayer { get; set; }
    public int maxSize { get { return gridSizeX * gridSizeY; } }
    public int xSize { get { return gridSizeX; } }

    public void bakeGrid()
    {
        activeLevel = GetComponent<gameController>().currentlySelectedLevel;
        originPoint = activeLevel.originPoint;

        nodeDiameter = nodeRadius * 2f;
        gridSizeX = Mathf.RoundToInt(activeLevel.gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(activeLevel.gridSize.y / nodeDiameter);
        grid = new Node[gridSizeX, gridSizeY];

        Node[,] localAvailableNodes = new Node[2, grid.GetLength(1) * 2];
        //Used for win state
        List<baseUnit> winStateNodes = new List<baseUnit>();

        Vector3 worldBottomLeft = originPoint - Vector3.right * activeLevel.gridSize.x / 2 - Vector3.forward * activeLevel.gridSize.y / 2;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                grid[i, j] = new Node(worldPoint, i, j);

                if (i == 0)
                {
                    localAvailableNodes[0, j] = grid[0, j];
                    winStateNodes.Add(grid[0, j].presentEntity);
                }
                else if (i == gridSizeX - 1)
                {
                    localAvailableNodes[1, j] = grid[i, j];
                }
            }
        }
        //Give to Grid Handler to Populate the Map
        gameStateControl = new gameStateController(winStateNodes, gridSizeY);
        gridControl = new gridController(localAvailableNodes, this, gameStateControl);
    }

    public void Update()
    {
        if (!(gameStateControl is null)) gameStateControl.update();
    }

    public Node nodeFromWorldPoint(Vector3 worldPos)
    {
        float perX = (worldPos.x + activeLevel.gridSize.x / 2) / activeLevel.gridSize.x;
        float perY = (worldPos.z + activeLevel.gridSize.y / 2) / activeLevel.gridSize.y;

        //Checks for within the Grid Location will prevent enemies from Attempting to path outside the node structures
        perX = Mathf.Clamp01(perX);
        perY = Mathf.Clamp01(perY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * perX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * perY);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, new Vector3(activeLevel.associatedLevel.gridSize.x, 0f, activeLevel.associatedLevel.gridSize.y));
        if (grid != null && displayGridGizmos)
        {
            foreach (baseUnit element in gridControl.getUnitList)
            {
                Color local = element.potential ? Color.cyan : Color.white;
                local = element.worldPosition.y == 0 ? Color.black : local;
                Gizmos.color = element.selected ? Color.blue : local;
                Vector3 lift = new Vector3(0f, .3f, 0f);
                Gizmos.DrawCube(element.worldPosition + lift, Vector3.one * (nodeDiameter - .3f));
                gridControl.redrawConnections(element);
            }
        }
    }
    public gridController callGrid { get { return gridControl; } }
    public Node[,] getGrid { get { return grid; } }
}
/// <summary>
/// This class will handle the random placemeent of Units -> and Connection of Nodes
/// </summary>
public class gridController
{
    dataInterpreter parent;
    gameStateController gameStateControl;
    int totalFillSlots;
    Node[,] availableNodes;
    List<baseUnit> activeUnits;
    baseUnit selectedUnit = null;
    public gridController(Node[,] _availableNodes, dataInterpreter _parent, gameStateController _gameStateControl)
    {
        availableNodes = _availableNodes;
        gameStateControl = _gameStateControl;
        parent = _parent;
        totalFillSlots = availableNodes.GetLength(1) / 2;
        activeUnits = new List<baseUnit>();
        populateGrid();
    }
    private void populateGrid()
    {
        //This is where the algorithm will play a role in terms of level difficulty -> For now it will be a constant algorithm
        for (int i = 0; i < totalFillSlots; i++)
        {
            int spawnChance = Random.Range(0, 2) == 1 ? 0 : 1;
            Node local = availableNodes[spawnChance, i];
            var clone = Object.Instantiate(parent.parent.gameUnit, local.worldPosition, Quaternion.identity);
            clone.GetComponent<baseUnit>().worldPosition = local.worldPosition;
            activeUnits.Add(clone.GetComponent<baseUnit>());
        }

        //This Section creates the connections
        baseUnit previous = null;
        baseUnit current = activeUnits[0];
        baseUnit next = activeUnits[1];
        Vector3 nextOffset = new Vector3(.2f, 0f, .2f);
        Vector3 previousOffset = new Vector3(-.2f, 0f, -.2f);
        for (int i = 0; i < activeUnits.Count; i++)
        {
            current = activeUnits[i];

            previous = i - 1 >= 0 ? activeUnits[i - 1] : null;
            next = i + 1 < activeUnits.Count ? activeUnits[i + 1] : null;

            current.neighbors[0] = previous;
            current.neighbors[1] = next;
        }
    }

    public void checkNodeSelection(baseUnit element)
    {
        if (selectedUnit != null && selectedUnit != element)
        {
            selectedUnit.selected = false;
            selectedUnit.triggerNeighbors(false);
            selectedUnit = null;
        }
        //Now set new one
        if (element != null)
        {
            selectedUnit = element;
            selectedUnit.selected = true;
            selectedUnit.triggerNeighbors(true);
        }
    }

    public void startSwitch()
    {
        if (selectedUnit != null)
        {
            switchNodeSelection(selectedUnit);
            if (selectedUnit.neighbors[0] != null)
                switchNodeSelection(selectedUnit.neighbors[0]);
            if (selectedUnit.neighbors[1] != null)
                switchNodeSelection(selectedUnit.neighbors[1]);
        }
    }

    public void switchNodeSelection(baseUnit uni)
    {
        Node checkNode = parent.nodeFromWorldPoint(uni.worldPosition);
        if (checkNode.gridX == 0)
        {
            Node target = parent.getGrid[parent.xSize - 1, checkNode.gridY];
            uni.worldPosition = target.worldPosition;
            gameStateControl.updateList(uni, false);
        }
        else
        {
            Node target = parent.getGrid[0, checkNode.gridY];
            uni.worldPosition = target.worldPosition;
            gameStateControl.updateList(uni, true);
        }
    }

    public void redrawConnections(baseUnit current)
    {
        Vector3 nextOffset = new Vector3(.2f, 0f, .2f);
        Vector3 previousOffset = new Vector3(-.2f, 0f, -.2f);

        if (current.neighbors[0] != null)
            Debug.DrawLine(current.worldPosition + previousOffset, current.neighbors[0].worldPosition + previousOffset, Color.cyan);

        if (current.neighbors[1] != null)
            Debug.DrawLine(current.worldPosition + nextOffset, current.neighbors[1].worldPosition + nextOffset, Color.blue);
    }

    public List<baseUnit> getUnitList { get { return activeUnits; } }
}

public class Node
{
    public Vector3 worldPosition;
    public int gridX, gridY;
    public bool active = false, selected = false;
    //Instant of instantiated squirrel etc. --- Placeholder for now
    public baseUnit presentEntity;

    public Node(Vector3 _worldPosition, int _gridX, int _gridY)
    {
        worldPosition = _worldPosition;

        gridX = _gridX;
        gridY = _gridY;

        if (!(presentEntity is null))
        {
            //Do something ---> 
        }
    }
}

public class gameStateController
{
    private List<baseUnit> winStateList;
    private int winNum;
    private enum state
    {
        playing,
        won,
        lost
    }
    private state gameState = state.playing;
    public gameStateController(List<baseUnit> activeUnits, int totalNum)
    {
        winStateList = activeUnits;
        winNum = totalNum;
    }

    public void update()
    {
        gameState = winStateList.Count == winNum ? state.won : state.playing;
        if (gameState == state.won)
        {
            //units animate and make em do dance or something ->
            //Generate new random grid layout ->
            Debug.Log("Level Complete");
        }
    }

    public void updateList(baseUnit toCheck, bool active)
    {
        if (active)
        {
            winStateList.Add(toCheck);
        }
        else
        {
           winStateList.Remove(toCheck);
        }
    }
}