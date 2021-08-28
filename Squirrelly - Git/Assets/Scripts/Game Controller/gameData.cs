using System;
using System.Collections.Generic;
using System.Linq;
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
            sData = new settingStorage();
            sData.setStorage();
        }
        else
        {
            pData = savedData.pData;
            lData = savedData.lData;
            //Needed More Attention than the Others allows for Static Management which is good!
            sData = savedData.sData;
            //Set Static Settings for rest of the Game->
            gameSettingsController.masterVolume = sData.masterVolume;
            gameSettingsController.vehicleVolume = sData.vehicleVolume;
            gameSettingsController.effectsVolume = sData.effectsVolume;
            gameSettingsController.musicVolume = sData.musicVolume;
            sData.setStorage();
        }
    }

    //These are the stored Class Information we Can Access Directly from the Game Data
    public progressData pData { get; set; }
    public levelHandler lData { get; set; }
    public settingStorage sData { get; set; }

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
        List<storedLevelData> levels = new List<storedLevelData>();

        foreach (KeyValuePair<string, storedLevelData> el in levelDictionary)
        {
            el.Value.found = false;
        }

        int index = 0;
        foreach (levelElement el in element)
        {
            el.associatedLevel.parentElement = el;
            //Check for Addition
            if (el.campaignLevel)
            {
                if (!levelDictionary.ContainsKey(el.associatedLevel.id + "Campaign" + el.tag) || levelDictionary[el.associatedLevel.id + "Campaign" + el.tag] == null)
                {
                    //Create a new stored Data
                    storedLevelData sData = new storedLevelData(el.associatedLevel);
                    levelDictionary.Add(el.associatedLevel.id + "Campaign" + el.tag, sData);
                    sData.found = true;
                    sData.id = el.associatedLevel.id + "Campaign" + el.tag;
                    sData.campaignLevel = true;
                    sData.updatedLevelIndex = index++;
                    levels.Add(sData);
                }
                else
                {
                    //All Checks for Changes -> Some Can Be Handled in Class
                    storedLevelData active = levelDictionary[el.associatedLevel.id + "Campaign" + el.tag];
                    active.found = true;
                    active.updatedLevelIndex = index++;
                    active.update(el.associatedLevel);
                    levels.Add(active);
                }
            }
            else
            {
                if (!levelDictionary.ContainsKey(el.associatedLevel.id + el.tag) || levelDictionary[el.associatedLevel.id + el.tag] == null)
                {
                    //Create a new stored Data
                    storedLevelData sData = new storedLevelData(el.associatedLevel);
                    levelDictionary.Add(el.associatedLevel.id + el.tag, sData);
                    sData.found = true;
                    sData.id = el.associatedLevel.id + el.tag;
                    sData.updatedLevelIndex = index++;
                    levels.Add(sData);
                }
                else
                {
                    //All Checks for Changes -> Some Can Be Handled in Class
                    storedLevelData active = levelDictionary[el.associatedLevel.id + el.tag];
                    active.found = true;
                    active.update(el.associatedLevel);
                    active.updatedLevelIndex = index++;
                    levels.Add(active);
                }
            }
        }

        //Check for Remove
        for (int i = 0; i < levels.Count; i++)
        {
            if (!levels[i].found)
                levelDictionary.Remove(levels[i].id);
        }

        return levels;
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
    public int difficultyIndex, containerIndex, updatedLevelIndex;
    Dictionary<string, container> gamemodes;
    //Array Must always stay the same ---
    public readonly string[] gameModeTags = { "timeMode", "waveMode", "highScore" };
    gameInfo activeRoute;

    //Vars
    public bool found;
    public bool campaignLevel;
    public storedLevelData(level associatedLevel)
    {
        id = associatedLevel.id;
        displayName = associatedLevel.displayName;
        gamemodes = new Dictionary<string, container>();
        if (associatedLevel.getTimeMode != null)
        {
            int local = associatedLevel.getTimeMode.getCount;
            container c = new container(local, associatedLevel.getTimeMode.name);
            gamemodes.Add(gameModeTags[0], c);
        }

        if (associatedLevel.getWaveMode != null)
        {
            int local = associatedLevel.getWaveMode.getCount;
            container c = new container(local, associatedLevel.getWaveMode.name);
            gamemodes.Add(gameModeTags[1], c);
        }

        if (associatedLevel.getHighScoreMode != null)
        {
            int local = associatedLevel.getHighScoreMode.getCount;
            container c = new container(local, associatedLevel.getHighScoreMode.name);
            gamemodes.Add(gameModeTags[2], c);
        }
    }

    public void update(level associatedLevel)
    {
        //Will Appropriately check all Data and Remove as Necessary
        Debug.Log("Update Fired");
        Debug.Log("GameMode Size: " + gamemodes.Count);
        //Debug.Log("Stars: " + getContainer(gameModeTags[0]).getGameInfo(0).starsEarned);
        //Also Check for Name Switch ->
        if (associatedLevel.displayName != displayName)
            displayName = associatedLevel.displayName;
        //Hard Coded GameMode Types for Right Now --- If GameModes Change/Are Added - 
        if (associatedLevel.getTimeMode != null)
        {
            if (gamemodes.ContainsKey(gameModeTags[0]))
            {
                if (!gamemodes[gameModeTags[0]].compareName(associatedLevel.getTimeMode.name))
                {
                    gamemodes.Remove(gameModeTags[0]);
                    int local = associatedLevel.getTimeMode.getCount;
                    container c = new container(local, associatedLevel.getTimeMode.name);
                    gamemodes.Add(gameModeTags[0], c);
                }
                else
                {
                    //Check Difficulties/Compare
                    gamemodes[gameModeTags[0]].compareDataSize(associatedLevel.getTimeMode.getCount);
                }
            }
            else
            {
                //Add the container
                int local = associatedLevel.getTimeMode.getCount;
                container c = new container(local, associatedLevel.getTimeMode.name);
                gamemodes.Add(gameModeTags[0], c);
            }
        }
        else if (gamemodes.ContainsKey(gameModeTags[0]))
        {
            gamemodes.Remove(gameModeTags[0]);
        }

        /*************************************/
        if (associatedLevel.getWaveMode != null)
        {
            if (gamemodes.ContainsKey(gameModeTags[1]))
            {
                if (!gamemodes[gameModeTags[1]].compareName(associatedLevel.getWaveMode.name))
                {
                    gamemodes.Remove(gameModeTags[1]);
                    int local = associatedLevel.getWaveMode.getCount;
                    container c = new container(local, associatedLevel.getWaveMode.name);
                    gamemodes.Add(gameModeTags[1], c);
                }
                else
                {
                    //Check Difficulties/Compare
                    gamemodes[gameModeTags[1]].compareDataSize(associatedLevel.getWaveMode.getCount);
                }
            }
            else
            {
                //Add the container
                int local = associatedLevel.getWaveMode.getCount;
                container c = new container(local, associatedLevel.getWaveMode.name);
                gamemodes.Add(gameModeTags[1], c);
            }
        }
        else if (gamemodes.ContainsKey(gameModeTags[1]))
        {
            gamemodes.Remove(gameModeTags[1]);
        }
        /*********************************************/

        if (associatedLevel.getHighScoreMode != null)
        {
            if (gamemodes.ContainsKey(gameModeTags[2]))
            {
                if (!gamemodes[gameModeTags[2]].compareName(associatedLevel.getHighScoreMode.name))
                {
                    gamemodes.Remove(gameModeTags[2]);
                    int local = associatedLevel.getHighScoreMode.getCount;
                    container c = new container(local, associatedLevel.getHighScoreMode.name);
                    gamemodes.Add(gameModeTags[2], c);
                }
                else
                {
                    //Check Difficulties/Compare
                    gamemodes[gameModeTags[2]].compareDataSize(associatedLevel.getHighScoreMode.getCount);
                }
            }
            else
            {
                //Add the container
                int local = associatedLevel.getHighScoreMode.getCount;
                container c = new container(local, associatedLevel.getHighScoreMode.name);
                gamemodes.Add(gameModeTags[2], c);
            }
        }
        else if (gamemodes.ContainsKey(gameModeTags[2]))
        {
            //Garbage Collection --- Etc.
            gamemodes.Remove(gameModeTags[2]);
        }
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
            //Ultimately just Null Return
            return new container(0, "");
        }
    }

    public void setContainer(string key, container element)
    {
        gamemodes[key] = element;
    }

    public gameInfo retrieveStorage()
    {
        return activeRoute = getContainer(gameModeTags[containerIndex]).getGameInfo(difficultyIndex);
    }

    //Will Save the Most Stars
    public bool setGameDetailsToSave(int indexToSet, float setValue)
    {
        switch (indexToSet) {
            case 0:
                if (activeRoute.starsEarned < (int)setValue)
                {
                    activeRoute.starsEarned = (int)setValue;
                    getContainer(gameModeTags[containerIndex]).setGameInfo(difficultyIndex, activeRoute);
                    return true;
                }
                else
                {
                    return false;
                }
            case 1:
                if (activeRoute.scoreEarned < (int)setValue)
                {
                    activeRoute.scoreEarned = (int)setValue;
                    getContainer(gameModeTags[containerIndex]).setGameInfo(difficultyIndex, activeRoute);
                    return true;
                }
                else
                {
                    return false;
                }
            case 2:
                if (setValue != 0)
                {
                    activeRoute.completed = 1;
                    getContainer(gameModeTags[containerIndex]).setGameInfo(difficultyIndex, activeRoute);
                    return true;
                }
                else
                {
                    return false;
                }
            case 3:
                if (activeRoute.completionTime > setValue || activeRoute.completionTime == 0f)
                {
                    activeRoute.completionTime = setValue;
                    getContainer(gameModeTags[containerIndex]).setGameInfo(difficultyIndex, activeRoute);
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }
}

[Serializable]
public struct container
{
    //Difficulties
    List<gameInfo> data;
    //Distinguish Different Scriptable Object
    string gameModeUniqueName;
    public container(int index, string uniqueName)
    {
        data = new List<gameInfo>();
        gameModeUniqueName = uniqueName;
        for (int i = 0; i < index; i++)
        {
            gameInfo local = new gameInfo();
            data.Add(local);
        }
    }

    public int dataSize { get { return data.Count; } }
    public gameInfo getGameInfo(int index){return data[index];}
    public void setGameInfo(int index, gameInfo element) { data[index] = element; }
    public bool compareName(string name) { return name == gameModeUniqueName ? true : false; }
    public void compareDataSize (int size)
    {
        if (size > dataSize)
        {
            for (int i = dataSize; i < size; i++)
            {
                gameInfo local = new gameInfo();
                data.Add(local);
            }
        }
        else if (size < dataSize)
        {
            for (int i = dataSize; i > size; i--)
            {
                data.RemoveAt(i - 1);
            }
        }
    }
}

[Serializable]
public struct gameInfo
{
    //Completed is 1 - Not Completed is 0
    public int starsEarned, scoreEarned, completed;
    public float completionTime;
}
#endregion