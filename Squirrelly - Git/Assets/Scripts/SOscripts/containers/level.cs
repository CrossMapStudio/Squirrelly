using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName = "ScriptableObjects/Level")]
public class level : ScriptableObject
{
    public string displayName;
    public string idTag;
    public Vector2 gridSize;
    public Vector3 originPoint = Vector3.zero;
    //This is the prefab level design -> Baked Lighting Etc...
    public GameObject stageElement, defaultStageElement;
    public Vector3 levelOriginAdjustment, columnButtonTriggerModifier;
    [HideInInspector]
    public levelElement parentElement;
    //These are the Added SO to Encapsulate the Difficulty Levels with Associated Gamemodes
    [SerializeField] private timeGamemode timeMode;
    [SerializeField] private waveGamemode waveMode;
    [SerializeField] private highScoreGamemode scoreMode;
    public timeGamemode getTimeMode { get { return timeMode; } }
    public waveGamemode getWaveMode { get { return waveMode; } }
    public highScoreGamemode getHighScoreMode { get { return scoreMode; } }

    //For Checking Against Added/Removed Levels and Optimizing the Dictionary
    public int getTotalCount { get { return getTimeMode.getCount + getWaveMode.getCount + getHighScoreMode.getCount; } } 

    public string id { get { return idTag; } }
}