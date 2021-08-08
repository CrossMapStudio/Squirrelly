using System.Collections.Generic;
using UnityEngine;

namespace GridHandler
{
    public class GridGenerator
    {
        //6-7
        //Used for Assigning Win Side, Etc.
        Node[,] grid;
        public List<columnTriggers> triggerButtons;
        public gridController gridControl;
        List<GameObject> levelUnits;
        GameObject columnTrigger, columnTarget;
        public int gridSizeX, gridSizeY;
        public void bakeGrid(level activeLevel, Vector3 originPoint, float nodeRadius, Vector2 unitSpacing, List<GameObject> _levelUnits, GameObject _columnTrigger, 
            Vector3 _columnTriggerModifier, GameObject _columnTarget, Vector3 _columnTargetModifier)
        {
            float nodeDiameter = nodeRadius * 2f;
            columnTrigger = _columnTrigger;
            columnTarget = _columnTarget;
            gridSizeX = Mathf.RoundToInt(activeLevel.gridSize.x);
            gridSizeY = Mathf.RoundToInt(activeLevel.gridSize.y);
            grid = new Node[gridSizeX, gridSizeY];
            triggerButtons = new List<columnTriggers>();

            Node[,] localAvailableNodes = new Node[2, grid.GetLength(1) * 2];
            Vector3 worldBottomLeft = originPoint - Vector3.right * activeLevel.gridSize.x * unitSpacing.x / 2 - Vector3.forward * activeLevel.gridSize.y * unitSpacing.y / 2;
            for (int i = 0; i < gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * ((i * nodeDiameter + nodeRadius) * unitSpacing.x) + Vector3.forward * ((j * nodeDiameter + nodeRadius) * unitSpacing.y);
                    grid[i, j] = new Node(worldPoint, i, j);

                    if (i == 0)
                    {
                        grid[0, j].isWinPos = true;
                        localAvailableNodes[0, j] = grid[0, j];

                        //Target Symbol
                        var targetSymbol = Object.Instantiate(columnTarget, worldPoint + _columnTargetModifier, Quaternion.identity);
                        grid[0, j].targetSymbol = targetSymbol;
                        targetSymbol.SetActive(false);
                    }
                    else if (i == gridSizeX - 1)
                    {
                        localAvailableNodes[1, j] = grid[i, j];
                        var trigger = Object.Instantiate(columnTrigger, worldPoint + _columnTriggerModifier, Quaternion.identity);
                        triggerButtons.Add(trigger.GetComponent<columnTriggers>());

                        //Target Symbol
                         var targetSymbol = Object.Instantiate(columnTarget, worldPoint + _columnTargetModifier, Quaternion.identity);
                         grid[i, j].targetSymbol = targetSymbol;
                         targetSymbol.SetActive(false);
                    }
                }
            }

            levelUnits = _levelUnits;
            gridControl = new gridController(this, localAvailableNodes);
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
        GridGenerator controller;

        public gridController(GridGenerator _controller, Node[,] _availableNodes)
        {
            controller = _controller;
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

                var clone = Object.Instantiate(units[Random.Range(0, units.Count)], local.worldPosition, Quaternion.identity);
                activeUnits.Add(clone);

                baseUnit unit = clone.GetComponent<baseUnit>();
                unit.worldPosition = local.worldPosition;
                unit.winPos = availableNodes[0, i].worldPosition.x;
                unit.winState = unit.worldPosition.x == unit.winPos ?  true : false;
                if (inputHandler.currentControl == inputHandler.controlSetting.controller)
                {
                    unit.setInput(0, i);
                }
                else
                {
                    unit.setInput(1, i);
                }

                unit.pos1 = availableNodes[0, i].worldPosition;
                unit.pos2 = availableNodes[1, i].worldPosition;
                unit.activeNodes.Add(availableNodes[0, i]); 
                unit.activeNodes.Add(availableNodes[1, i]);
                if (unit.worldPosition == availableNodes[0, i].worldPosition)
                {
                    availableNodes[0, i].inWinPos = true;
                }
                else
                {
                    availableNodes[0, i].inWinPos = false;
                }
                unitList.Add(unit);

                controller.triggerButtons[i].gridControl = this;
                controller.triggerButtons[i].associatedUnit = unit;
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
            destoryUnit(true);
            activeUnits.Clear();
            unitList.Clear();

            for (int i = 0; i < controller.gridSizeY; i++)
            {
                availableNodes[0, i].inWinPos = false;
                availableNodes[0, i].isWinPos = true;
            }
        }

        public void checkNodeSelection(baseUnit element, bool switchAuto = true)
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
                if (switchAuto)
                    startSwitch();
            }
        }

        public void disableTargetSymbols(baseUnit unitToDisable = null)
        {
            if (unitToDisable != null)
            {
                unitToDisable.activeNodes[1].targetSymbol.SetActive(false);
                unitToDisable.activeNodes[0].targetSymbol.SetActive(false);
            } 
            else if (selectedUnit != null)
            {
                selectedUnit.selected = false;
                selectedUnit.triggerNeighbors(false);
                selectedUnit.activeNodes[1].targetSymbol.SetActive(false);
                selectedUnit.activeNodes[0].targetSymbol.SetActive(false);
                selectedUnit = null;
            }
        }

        public void startSwitch()
        {
            if (selectedUnit != null)
            {
                switchNodeSelection(selectedUnit);
                selectedUnit.lockedSelection = true;
                if (selectedUnit.neighbors[0] != null)
                    switchNodeSelection(selectedUnit.neighbors[0]);
                if (selectedUnit.neighbors[1] != null)
                    switchNodeSelection(selectedUnit.neighbors[1]);
            }
        }

        public void destoryUnit(bool destroyAll = false)
        {
            if (!destroyAll)
            {
                if (selectedUnit != null)
                {
                    selectedUnit.changeState(2);
                    unitList.Remove(selectedUnit);
                    selectedUnit = null;
                }
            }
            else
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    if (unitList[i] != null)
                        unitList[i].destroyUnit(1);
                }
            }
        }

        public void switchNodeSelection(baseUnit uni)
        {
            uni.worldPosition = uni.worldPosition == uni.pos1 ? uni.pos2 : uni.pos1;
        }

        public List<baseUnit> getUnitList { get { return unitList; } }

        public bool checkWinNodes()
        {
            for (int i = 0; i < controller.gridSizeY; i++)
            {
                if (!availableNodes[0, i].isWinPos)
                    continue;

                if (availableNodes[0, i].inWinPos)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class Node
    {
        public Vector3 worldPosition;
        public int gridX, gridY;
        public bool active = false, selected = false;
        public GameObject targetSymbol;
        //Instant of instantiated squirrel etc. --- Placeholder for now
        public baseUnit presentEntity;

        public bool isWinPos;
        public bool inWinPos;

        public Node(Vector3 _worldPosition, int _gridX, int _gridY)
        {
            worldPosition = _worldPosition;

            gridX = _gridX;
            gridY = _gridY;
        }
    }
}
