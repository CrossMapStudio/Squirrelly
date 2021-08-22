using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class gameController : MonoBehaviour
{
    gameData gData;
    [HideInInspector]
    public sceneManager sceneControl;
    //This is for the Editor
    [SerializeField] private List<levelElement> groupedLevels;
    [SerializeField] private List<levelElement> campaignLevels;
    //Might need to make private ---
    [HideInInspector]
    public level currentlySelectedLevel;
    [HideInInspector]
    public storedLevelData activeStorage;

    //This will be temp we can set most of this with the level system
    public GameObject gameUnit;
    public LayerMask inputLayer;
    public List<storedLevelData> updatedLevels { get; set; }
    private Dictionary<string, level> levelListing;
    //This will change with the level data element actually feeding the correct units -> For now this is for input handling
    //Pause State ->
    public static bool pauseState = false, gameEndState = false, gameIntroState = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gData = serializationHandler.LoadGame(serializationHandler.fileTag) != null ? new gameData(serializationHandler.LoadGame(serializationHandler.fileTag)) : new gameData();
        sceneControl = GetComponent<sceneManager>();
        groupedLevels.AddRange(campaignLevels);
        updatedLevels = gData.generatestoredLevelData(groupedLevels);
        levelListing = new Dictionary<string, level>();
        //Create the Dictionary to Access the Updated Levels with the Grouped Levels
        for (int i = 0; i < updatedLevels.Count; i++)
        {
            levelListing.Add(updatedLevels[i].id, groupedLevels[i].associatedLevel);
        }
    }

    //Really for Debugging (AutoSave for Later)
    public void serializationMenu(int value)
    {
        switch (value)
        {
            case 0:
                serializationHandler.saveGame(serializationHandler.fileTag, gData);
                break;
            case 1:
                gData = (serializationHandler.LoadGame(serializationHandler.fileTag) != null) ? new gameData(serializationHandler.LoadGame(serializationHandler.fileTag)) : gData;
                break;
            case 2:
                serializationHandler.clearFile(serializationHandler.fileTag);
                break;
            case 3:
                _ = gData.lData.levelDictionary ?? throw errorHandler.fileNotFound;
                foreach (KeyValuePair<string, storedLevelData> element in gData.lData.levelDictionary)
                {
                    Debug.Log("Element ID: " + element.Key);
                }
                Debug.Log("Total Coins: " + gData.pData.coins);
                break;
            case 4:
                gData.pData.coins++;
                break;
            case 5:
                //serializationHandler.saveGame(serializationHandler.fileTag, gData);
                sceneControl.quitGame();
                break;
        }
    }

    public void modifyLevelData()
    {
        //This is the function to call first before the serielization
        Debug.Log("Active Storage Stars: " + activeStorage.getContainer(activeStorage.gameModeTags[0]).getGameInfo(0).starsEarned);
        gData.lData.modifyStorage(activeStorage.id, activeStorage);
    }

    public void printLevelData()
    {

    }

    public void setActiveLevel(string id, storedLevelData _activeStorage)
    {
        currentlySelectedLevel = levelListing[id];
        activeStorage = _activeStorage;
    }

    public bool compareCurrentLevel(string id)
    {
        bool local = currentlySelectedLevel == levelListing[id] ? true : false;
        return local;
    }

    public void launchActiveLevel(Animator loadSceneAnim = null, string animToTrigger = null, string animationNameForLoadStart = null)
    {
        if (currentlySelectedLevel != null)
        {
            activeStorage.retrieveStorage();
            sceneControl.loadScene(2, loadSceneAnim, animToTrigger, animationNameForLoadStart);
        }
    }

    public gameData getGameDataForSave { get { return gData; } }
}

#region Level Information for Proper Storage and Backtracking
[Serializable]
public struct levelElement
{
    public bool campaignLevel;
    public string tag;
    public level associatedLevel;
}
#endregion