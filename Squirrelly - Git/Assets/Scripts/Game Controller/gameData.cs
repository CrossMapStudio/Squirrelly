using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game Data will Collect Class Information --- This allows Serielization to Handle One Class and Read Back Data
/// </summary>
[Serializable]
public class gameData
{
    public gameData(gameData savedData = null)
    {
        if (savedData == null)
        {
            pData = new progressData(this);
            lData = new levelHandler(this);
        }
        else
        {
            pData = savedData.pData;
            lData = savedData.lData;
        }
    }

    //These are the stored Class Information we Can Access Directly from the Game Data
    public progressData pData { get; set; }
    public levelHandler lData { get; set; }

    public void generatestoredLevelData(List<levelElement> element)
    {
        lData.bakestoredLevelData(element);
    }
}

#region Progress Handler/Data Storage Class
[Serializable]
public class progressData
{
    public progressData(gameData _controller)
    {
        controller = _controller;
    }
    //Each Data Storage Needs Empty Constructors
    public int coins { get; set; } = 0;
    private gameData controller { get; set; }
}
#endregion
#region Level Handler/Level Data Storage
[Serializable]
public class levelHandler
{
    private enum gameModeID
    {
        timeMode,
        waveMode,
        scoreMode
    }
    public levelHandler(gameData _parent)
    {
        parent = _parent;
        levelDictionary = new Dictionary<string, storedLevelData>();
    }

    public void bakestoredLevelData(List<levelElement> element)
    {
        //Set the Gamemodes first and create a total index
        timeGamemode tMode;
        waveGamemode wMode;
        highScoreGamemode sMode;

        //Set all to false before parsing and checking
        foreach (KeyValuePair<string, storedLevelData> el in levelDictionary)
        {
            el.Value.found = false;
        }

        //Looking for new or duplicated Levels (Same Number or Mor eLevels)
        for (int i = 0; i < element.Count; i++)
        {
            tMode = element[i].associatedLevel.getTimeMode;
            if (tMode != null)
            for (int j = 0; j < tMode.getCount; j++)
            {
                checkLevelSer(element[i], gameModeID.timeMode.ToString(), j);
            }
            wMode = element[i].associatedLevel.getWaveMode;
            if (wMode != null)
            for (int j = 0; j < wMode.getCount; j++)
            {
                checkLevelSer(element[i], gameModeID.waveMode.ToString(), j);
            }
            sMode = element[i].associatedLevel.getHighScoreMode;
            if (sMode != null)
            for (int j = 0; j < sMode.getCount; j++)
            {
                checkLevelSer(element[i], gameModeID.scoreMode.ToString(), j);
            }
        }

        List<string> elementsToRemove = new List<string>();
        //Looking for Removed Levels -> Optimize the Dictionary (Less Levels Than Expected)
        foreach (KeyValuePair<string, storedLevelData> el in levelDictionary)
        {
            if (!el.Value.found)
                elementsToRemove.Add(el.Key);
        }

        for (int i = 0; i < elementsToRemove.Count; i++)
        {
            levelDictionary.Remove(elementsToRemove[i]);
        }
    }
 
    public void checkLevelSer(levelElement element, string gamemode, int diff)
    {
        string idToCheck = element.associatedLevel.id + gamemode + diff;
        if (!levelDictionary.ContainsKey(idToCheck))
        {
            var local = new storedLevelData(idToCheck, element.associatedLevel.id);
            levelDictionary.Add(idToCheck, local);

            local.found = true;
        }
        else
        {
            if (levelDictionary[idToCheck].levelName != element.associatedLevel.id)
            {
                levelDictionary.Remove(idToCheck);
                var local = new storedLevelData(idToCheck, element.associatedLevel.id);
                levelDictionary.Add(idToCheck, local);
                local.found = true;
                Debug.Log(idToCheck + " Level was not found -> New Level Instantiated.");
            }
            else
            {
                levelDictionary[idToCheck].found = true;
                Debug.Log(idToCheck + " Level Found!");
            }
        }
    }

    #region Debugging
    public void modifyLevelData(string id, int starsEarned)
    {
        levelDictionary[id].stars = starsEarned;
        Debug.Log("Stars Modified: " + levelDictionary[id].stars);
    }
    public void printData(string id)
    {
        storedLevelData lev = levelDictionary[id];
        _ = lev ?? throw errorHandler.keyValueNonExistent;
        Debug.Log("Level Title: " + lev.ID + '\n');
    }
    #endregion

    public gameData parent { get; set; }
    public Dictionary<string, storedLevelData> levelDictionary { get; }
}
#endregion
#region Level Data
[Serializable]
public class storedLevelData
{
    //Level ID, Gamemode, Difficulty, Time of Completion, In-Progress, Locations of Units, Location of Vehicles, Current Time, Best Time, Stars Earned
    public storedLevelData(string idValue, string _levelName)
    {
        ID = idValue;
        levelName = _levelName;
    }
    public string ID { get; set; }
    public string levelName { get; set; }
    //Achievements - Time of Completion - Waves Completed - Best Time - If Completed? - 
    public int stars { get; set; }
    public float bestTime { get; set; }
    public int wavesCompleted { get; set; }
    //For Serielization
    public bool found { get; set; } = false;
    //Will need Get Sets For Some of These (Time, Level Type, Etc.)
}
#endregion


/* The Level Scriptable Object is the actual level data -> This will determine difficulty, spawning percentanges, wave systems ->
 * It is what the interpreter will use to generate each level -> Grid Size, Etc.
 * 
 * The Level Data will Hold an ID Associated with each unique level within the game, and data that needs to be stored ->
 * Stars earned, time of completion, Best time, current wave, unit placement, etc.
 * 
 * We need to have a list of stored levels that a system can process through and generate the proper save data using it
 * Sorting by category, then difficulty.
 */