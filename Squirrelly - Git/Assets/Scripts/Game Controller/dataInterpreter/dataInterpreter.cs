using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class dataInterpreter : MonoBehaviour
{
    levelElement activeElement;
    public bool displayGridGizmos = true;
    [SerializeField] private float nodeRadius = .5f;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    Vector3 originPoint;

    //Controller for the Game Loop
    gridController gridControl;
    public int maxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    public void bakeGrid()
    {
        activeElement = GetComponent<gameController>().getActiveLevel;
        originPoint = activeElement.associatedLevel.originPoint;

        nodeDiameter = nodeRadius * 2f;
        gridSizeX = Mathf.RoundToInt(activeElement.associatedLevel.gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(activeElement.associatedLevel.gridSize.y / nodeDiameter);
        grid = new Node[gridSizeX, gridSizeY];
        Node[,] localAvailableNodes = new Node[2, grid.GetLength(1) * 2];

        Vector3 worldBottomLeft = originPoint - Vector3.right * activeElement.associatedLevel.gridSize.x / 2 - Vector3.forward * activeElement.associatedLevel.gridSize.y / 2;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                grid[i, j] = new Node(worldPoint, i, j);

                if (i == 0)
                {
                    localAvailableNodes[0, j] = grid[0, j];
                }
                else if(i == gridSizeX - 1)
                {
                    localAvailableNodes[1, j] = grid[i, j];
                }
            }
        }
        //Give to Grid Handler to Populate the Map
        gridControl = new gridController(localAvailableNodes);
    }

    public Node nodeFromWorldPoint(Vector3 worldPos)
    {
        float perX = (worldPos.x + activeElement.associatedLevel.gridSize.x / 2) / activeElement.associatedLevel.gridSize.x;
        float perY = (worldPos.y + activeElement.associatedLevel.gridSize.y / 2) / activeElement.associatedLevel.gridSize.y;

        //Checks for within the Grid Location will prevent enemies from Attempting to path outside the node structures
        perX = Mathf.Clamp01(perX);
        perY = Mathf.Clamp01(perY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * perX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * perY);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(activeElement.associatedLevel.gridSize.x, 0f, activeElement.associatedLevel.gridSize.y));
        if (grid != null && displayGridGizmos)
        {
            foreach (Node element in grid)
            {
                Gizmos.color = element.active ? Color.green : Color.red;
                Gizmos.DrawWireCube(element.worldPosition, Vector3.one * (nodeDiameter - .3f));
            }
        }
    }
}

public class Node
{
    public Vector3 worldPosition;
    public int gridX, gridY;
    public bool active = false;
    public Node[] neighbors = new Node[2];

    public Node(Vector3 _worldPosition, int _gridX, int _gridY)
    {
        worldPosition = _worldPosition;

        gridX = _gridX;
        gridY = _gridY;
    }
}

/// <summary>
/// This class will handle the random placemeent of Units -> and Connection of Nodes
/// </summary>
public class gridController
{
    int totalFillSlots;
    Node[,] availableNodes;
    List<Node> activeNodes;
    public gridController(Node[,] _availableNodes)
    {
        availableNodes = _availableNodes;
        totalFillSlots = availableNodes.GetLength(1) / 2;
        activeNodes = new List<Node>();
        populateGrid();
    }

    public void populateGrid()
    {
        //This is where the algorithm will play a role in terms of level difficulty -> For now it will be a constant algorithm
        for (int i = 0; i < totalFillSlots; i++)
        {
            int spawnChance = UnityEngine.Random.Range(0, 2);
            if (spawnChance == 1)
            {
                activeNodes.Add(availableNodes[0,i]);
                availableNodes[0, i].active = true;
                continue;
            }
            else
            {
                activeNodes.Add(availableNodes[1,i]);
                availableNodes[1, i].active = true;
                continue;
            }
        }

        //This Section creates the connections
        Node previous = null;
        Node current = activeNodes[0];
        Node next = activeNodes[1];
        Vector3 nextOffset = new Vector3(.2f, 0f, .2f);
        Vector3 previousOffset = new Vector3(-.2f, 0f, -.2f);
        for (int i = 0; i < activeNodes.Count; i++)
        {
            current = activeNodes[i];

            previous = i - 1 >= 0 ? activeNodes[i - 1] : null;
            next = i + 1 < activeNodes.Count ? activeNodes[i + 1] : null;

            current.neighbors[0] = previous;
            current.neighbors[1] = next;

            if (previous != null)
                Debug.DrawLine(current.worldPosition + previousOffset, current.neighbors[0].worldPosition + previousOffset, Color.cyan, Mathf.Infinity);

            if (next != null)
                Debug.DrawLine(current.worldPosition + nextOffset, current.neighbors[1].worldPosition + nextOffset, Color.blue, Mathf.Infinity);
        }
    }
}
