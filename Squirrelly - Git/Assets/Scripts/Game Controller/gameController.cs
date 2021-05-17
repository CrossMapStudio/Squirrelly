using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class gameController : MonoBehaviour
{
    gameData gData;
    //This is for the Editor
    [SerializeField] private List<levelElement> groupedLevels;
    private levelElement currentlySelectedLevel;

    //For UI
    [SerializeField] private Slider stars;
    [SerializeField] private InputField levelName;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gData = serializationHandler.LoadGame(serializationHandler.fileTag) != null ? new gameData(serializationHandler.LoadGame(serializationHandler.fileTag)) : new gameData();
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
                    Debug.Log("Element ID: " + element.Key);
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
        //gData.lData.modifyLevelData(id.text, (int)stars.value);
        foreach (levelElement element in groupedLevels)
        {
            if (element.associatedLevel.name == levelName.text)
            {
                currentlySelectedLevel = element;
                break;
            }
        }
    }

    public void printLevelData()
    {
        gData.lData.printData(levelName.text);
    }

    public levelElement getActiveLevel { get { return currentlySelectedLevel; } }
}

#region Level Information for Proper Storage and Backtracking
[Serializable]
public struct levelElement
{
    public level associatedLevel;
}
#endregion