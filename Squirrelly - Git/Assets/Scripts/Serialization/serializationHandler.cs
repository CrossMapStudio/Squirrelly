using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class serializationHandler
{
    public static string fileTag = "Squirrelly.cms";
    #region Save Element
    public static void saveGame(string fileTag, object element = null)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        //string pathSet = "D:/SnowFallCollab/Snowfall/ResponseStorage/BinaryText.txt";
        string path = Application.persistentDataPath + fileTag;
        FileStream fileStream = new FileStream(path, FileMode.Create);

        //Create a new one just for testing
        try
        {
            _ = element ?? throw errorHandler.noPassedData;

            formatter.Serialize(fileStream, element);
            Debug.Log("File Saved Successfully!");
            fileStream.Close();
        }
        catch (Exception ex)
        {
            fileStream.Close();
            Debug.LogError(ex.Message + " Element Name: " + nameof(element));
        }
    }
    #endregion
    #region Load Element
    public static gameData LoadGame(string fileTag)
    {
        //string pathSet = "D:/SnowFallCollab/Snowfall/ResponseStorage/BinaryText.txt";
        string path = Application.persistentDataPath + fileTag;
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            gameData element = null;
            if (stream.Length != 0)
                element = formatter.Deserialize(stream) as gameData;

            stream.Close();
            return element;
        }
        Debug.Log(errorHandler.fileNotFound.Message);
        return null;
    }
    #endregion
    #region Clear File
    public static void clearFile(string fileTag)
    {
        string path = Application.persistentDataPath + fileTag;
        Debug.Log(path);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("File Deleted");
        }
        else
        {
            throw errorHandler.fileNotFound;
        }
    }
    #endregion
}

#region Exeption Handler
public static class errorHandler
{
    #region File Errors
    public static void printMessage(string msg)
    {
        Debug.Log(msg);
    }
    public static Exception fileNotFound { get; } = 
        new Exception("File Not Found on Clear - Either the Tag is Incorrect or the File does not Exist!");
    public static Exception noPassedData { get; } = 
        new Exception("Passed Data to the Save Class Cannot be Null - Corresponding Class Was Not Passed Correctly or Null!");
    #endregion

    #region Dictionary Errors
    public static Exception keyValueNonExistent { get; } =
        new Exception("The Key is not Found within the Dictionary or the Value is Null!");
    public static Exception valueOutsideOfSwitchRange { get; } =
    new Exception("The Value Passed to the Switch Statement forced Default Call!");
    #endregion
}
#endregion