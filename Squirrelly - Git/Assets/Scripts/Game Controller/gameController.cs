using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class gameController : MonoBehaviour
{
    gameData gData;
    //This is for the Editor
    public List<levelElement> timeTrials, waveBased, highScore;
    //This is used for all levels
    private List<levelElement> groupedLevels;

    //For UI
    [SerializeField] private Slider stars;
    [SerializeField] private InputField id;
    private void Awake()
    {
        gData = serializationHandler.LoadGame(serializationHandler.fileTag) != null ? new gameData(serializationHandler.LoadGame(serializationHandler.fileTag)) : new gameData();
        groupedLevels = new List<levelElement>();

        //Combine all Levels into Single List that will be Generated into a Map/Key Value -> This will make Access 1-1 -> Allows for Re-structuring
        groupedLevels.AddRange(timeTrials);
        groupedLevels.AddRange(waveBased);
        groupedLevels.AddRange(highScore);
    }

    private void Start()
    {
        gData.generatestoredLevelData(groupedLevels);
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
                    Debug.Log("Element ID: " + element.Key + " Element Value Name: " + element.Value.levelName);
                }
                Debug.Log("Total Coins: " + gData.pData.coins);
                break;
            case 4:
                gData.pData.coins++;
                break;
        }
    }

    public void modifyLevelData()
    {
        gData.lData.modifyLevelData(id.text, (int)stars.value);
    }

    public void printLevelData()
    {
        gData.lData.printData(id.text);
    }
}

#region Level Information for Proper Storage and Backtracking
[Serializable]
public struct levelElement
{
    public level associatedLevel;
    public string id;
    public levelType type;
    public difficulty difficultySetting;

    public enum levelType
    {
        waveBased,
        timeTrial,
        highScore
    }

    public enum difficulty
    {
        easy,
        moderate,
        challenging,
        impossible
    }
}
#endregion