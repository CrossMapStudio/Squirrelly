using System.Collections.Generic;
using UnityEngine;

namespace GridHandler
{
    public class GridGenerator
    {
        //Used for Assigning Win Side, Etc.
        Node[,] grid;
        public gridController gridControl;
        List<GameObject> levelUnits;
        public void bakeGrid(level activeLevel, Vector3 originPoint, float nodeRadius, List<GameObject> _levelUnits)
        {
            float nodeDiameter = nodeRadius * 2f;
            int gridSizeX = Mathf.RoundToInt(activeLevel.gridSize.x / nodeDiameter);
            int gridSizeY = Mathf.RoundToInt(activeLevel.gridSize.y / nodeDiameter);
            grid = new Node[gridSizeX, gridSizeY];

            Node[,] localAvailableNodes = new Node[2, grid.GetLength(1) * 2];
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
                    }
                    else if (i == gridSizeX - 1)
                    {
                        localAvailableNodes[1, j] = grid[i, j];
                    }
                }
            }

            levelUnits = _levelUnits;
            gridControl = new gridController(localAvailableNodes);
        }

        public List<baseUnit> startWave()
        {
            return gridControl.populateGrid(levelUnits);
        }
    }

    public class gridController
    {
        int totalFillSlots;
        Node[,] availableNodes;
        baseUnit selectedUnit = null;
        List<GameObject> activeUnits;
        List<baseUnit> unitList;

        public gridController(Node[,] _availableNodes)
        {
            availableNodes = _availableNodes;
            totalFillSlots = availableNodes.GetLength(1) / 2;
            activeUnits = new List<GameObject>();
            unitList = new List<baseUnit>();
        }

        public List<baseUnit> populateGrid(List<GameObject> units)
        {
            //Wipe it clean
            clearGrid();
            //This is where the algorithm will play a role in terms of level difficulty -> For now it will be a constant algorithm
            for (int i = 0; i < totalFillSlots; i++)
            {
                int spawnChance = Random.Range(0, 2) == 1 ? 0 : 1;
                Node local = availableNodes[spawnChance, i];

                var clone = Object.Instantiate(units[0], local.worldPosition, Quaternion.identity);
                activeUnits.Add(clone);
                baseUnit unit = clone.GetComponent<baseUnit>();
                unit.worldPosition = local.worldPosition;
                unit.winPos = availableNodes[0, i].worldPosition.x;

                unit.pos1 = unit.worldPosition;
                unit.pos2 = availableNodes[spawnChance == 0 ? 1 : 0, i].worldPosition;
                unitList.Add(clone.GetComponent<baseUnit>());
            }

            //This Section creates the connections
            baseUnit previous = null;
            baseUnit current = unitList[0];
            baseUnit next = unitList[1];
            Vector3 nextOffset = new Vector3(.2f, 0f, .2f);
            Vector3 previousOffset = new Vector3(-.2f, 0f, -.2f);
            for (int i = 0; i < unitList.Count; i++)
            {
                current = unitList[i];

                previous = i - 1 >= 0 ? unitList[i - 1] : null;
                next = i + 1 < unitList.Count ? unitList[i + 1] : null;

                current.neighbors[0] = previous;
                current.neighbors[1] = next;
            }

            return unitList;
        }

        public void clearGrid()
        {
            for (int i = 0; i < unitList.Count; i++)
            {
                deleteUnit(unitList[i]);
            }
            gameStateController.compareNum = 0;
            activeUnits.Clear();
            unitList.Clear();
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

        public bool deleteUnit(baseUnit unit = null)
        {
            if (unit != null)
            {
                unit.destroyUnit();
                return true;
            }
            return false;
        }

        public void switchNodeSelection(baseUnit uni)
        {
            uni.worldPosition = uni.worldPosition == uni.pos1 ? uni.pos2 : uni.pos1;
        }
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
}
