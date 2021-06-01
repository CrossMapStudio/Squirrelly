using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    public List<storedLevelData> generatestoredLevelData(List<levelElement> element)
    {
        return lData.bakestoredLevelData(element);
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

    public List<storedLevelData> bakestoredLevelData(List<levelElement> element)
    {
        List<storedLevelData> levels = levelDictionary.Values.ToList();

        foreach (KeyValuePair<string, storedLevelData> el in levelDictionary)
        {
            el.Value.found = false;
        }


        foreach (levelElement el in element)
        {
            //Check for Addition
            if (!levelDictionary.ContainsKey(el.associatedLevel.id) || levelDictionary[el.associatedLevel.id] == null)
            {
                //Create a new stored Data
                storedLevelData sData = new storedLevelData(el.associatedLevel);
                levelDictionary.Add(el.associatedLevel.id, sData);
                sData.found = true;

                levels.Add(sData);
            }
            else
            {
                //All Checks for Changes -> Some Can Be Handled in Class
                storedLevelData active = levelDictionary[el.associatedLevel.id];
                active.found = true;
                active.update();
            }
        }

        //Check for Remove
        for (int i = 0; i < levels.Count; i++)
        {
            if (!levels[i].found)
                levelDictionary.Remove(levels[i].id);
        }

        return levelDictionary.Values.ToList();
    }

    #region Debugging
    public void modifyLevelData(string id)
    {
        
    }
    public void printData(string id)
    {

    }
    #endregion

    public gameData parent { get; set; }
    public Dictionary<string, storedLevelData> levelDictionary { get; }
    public void modifyStorage(string id, storedLevelData data) { levelDictionary[id] = data; }
}
#endregion

#region Level Data
[Serializable]
public class storedLevelData
{
    public string id, displayName;
    public int difficultyIndex, containerIndex;
    Dictionary<string, container> gamemodes;
    //Array Must always stay the same ---
    public readonly string[] gameModeTags = { "timeMode", "waveMode", "highScore" };
    gameInfo activeRoute;

    //Vars
    public bool found;
    public storedLevelData(level associatedLevel)
    {
        id = associatedLevel.id;
        displayName = associatedLevel.displayName;
        gamemodes = new Dictionary<string, container>();
        if (associatedLevel.getTimeMode != null)
        {
            int local = associatedLevel.getTimeMode.getCount;
            container c = new container(local);
            gamemodes.Add(gameModeTags[0], c);
        }

        if (associatedLevel.getWaveMode != null)
        {
            int local = associatedLevel.getWaveMode.getCount;
            container c = new container(local);
            gamemodes.Add(gameModeTags[1], c);
        }

        if (associatedLevel.getHighScoreMode != null)
        {
            int local = associatedLevel.getHighScoreMode.getCount;
            container c = new container(local);
            gamemodes.Add(gameModeTags[2], c);
        }
    }

    public void update()
    {
        //Will Appropriately check all Data and Remove as Necessary
        Debug.Log("Update Fired");
        Debug.Log("GameMode Size: " + gamemodes.Count);
        Debug.Log("Stars: " + getContainer(gameModeTags[0]).getGameInfo(0).starsEarned);
    }

    //Need Methods to Grab Correct Data
    public container getContainer(string key)
    {
        if (gamemodes.ContainsKey(key))
        {
            return gamemodes[key];
        }
        else
        {
            //Will need to write an Instantiation Method but Update should handle this!
            throw errorHandler.keyValueNonExistent;
        }
    }

    public void retrieveStorage()
    {
        activeRoute = getContainer(gameModeTags[containerIndex]).getGameInfo(difficultyIndex);
    }

    //Will Save the Most Stars
    public void addStar(int stars)
    {
        activeRoute.starsEarned = activeRoute.starsEarned < stars ? stars : activeRoute.starsEarned;
        getContainer(gameModeTags[containerIndex]).setGameInfo(difficultyIndex, activeRoute);
    }
}

[Serializable]
public struct container
{
    List<gameInfo> data;
    public container(int index)
    {
        data = new List<gameInfo>();
        for (int i = 0; i < index; i++)
        {
            gameInfo local = new gameInfo();
            data.Add(local);
        }
    }

    public int dataSize { get { return data.Count; } }
    public gameInfo getGameInfo(int index){return data[index];}
    public void setGameInfo(int index, gameInfo element) { data[index] = element; }
}

[Serializable]
public struct gameInfo
{
    public int starsEarned, lastWaveCompleted;
    public bool completed;
}
#endregion