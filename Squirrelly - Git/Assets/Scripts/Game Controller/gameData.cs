using System;
using System.Collections.Generic;
using UnityEditor;
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
            Debug.Log("Stored Levels: " + savedData.lData.levelDictionary.Count);
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
    public levelHandler(gameData _parent)
    {
        parent = _parent;
        levelDictionary = new Dictionary<string, storedLevelData>();
    }

    public void bakestoredLevelData(List<levelElement> element)
    {
        List<string> removeKeyList = new List<string>();
        for (int i = 0; i < element.Count; i++)
        {
            string nameChange = element[i].associatedLevel.name + element[i].id;
            if (!levelDictionary.ContainsKey(element[i].id))
            {
                levelDictionary.Add(element[i].id, new storedLevelData(element[i].id, nameChange));
                continue;
            }
            else
            {
                if (levelDictionary[element[i].id].levelName != nameChange)
                {
                    levelDictionary.Remove(element[i].id);
                    levelDictionary.Add(element[i].id, new storedLevelData(element[i].id, nameChange));
                    Debug.Log(nameChange);
                    continue;
                }
            }
        }

        if (levelDictionary.Count > element.Count)
        {
            foreach (KeyValuePair<string, storedLevelData> pair in levelDictionary)
            {
                for (int i = 0; i < element.Count; i++)
                {
                    string nameChange = element[i].associatedLevel.name + element[i].id;
                    if (element[i].id == pair.Key)
                    {
                        if (pair.Value.levelName != nameChange)
                        {
                            removeKeyList.Add(pair.Key);
                            levelDictionary.Add(element[i].id, new storedLevelData(element[i].id, nameChange));
                            break;
                        }
                        break;
                    }

                    if (i == element.Count - 1)
                    {
                        removeKeyList.Add(pair.Key);
                        break;
                    }
                }
            }

            for (int i = 0; i < removeKeyList.Count; i++)
            {
                levelDictionary.Remove(removeKeyList[i]);
            }
        }
    }
    public void modifyLevelData(string id, int starsEarned)
    {
        levelDictionary[id].stars = starsEarned;
        Debug.Log("Stars Modified: " + levelDictionary[id].stars);
    }
    public void printData(string id)
    {
        storedLevelData lev = levelDictionary[id];
        _ = lev ?? throw errorHandler.keyValueNonExistent;
        Debug.Log("Level Title: " + lev.levelName + '\n' + "Level Stars: " + lev.stars);
    }

    public gameData parent { get; set; }
    public Dictionary<string, storedLevelData> levelDictionary { get; }
}
#endregion
#region Level Data
[Serializable]
public class storedLevelData
{
    //Level ID, Gamemode, Difficulty, Time of Completion, In-Progress, Locations of Units, Location of Vehicles, Current Time, Best Time, Stars Earned
    public storedLevelData(string idValue, string name)
    {
        ID = idValue;
        levelName = name;
    }
    public string ID { get; set; }
    public string levelName { get; set; }

    //Achievements - Time of Completion - Waves Completed - Best Time - If Completed? - 
    public int stars { get; set; }
    public float bestTime { get; set; }
    public int wavesCompleted { get; set; }
    public bool completed { get; set; }
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