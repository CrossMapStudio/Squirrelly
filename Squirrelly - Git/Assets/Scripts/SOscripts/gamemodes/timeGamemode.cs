using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "timeGameMode", menuName = "ScriptableObjects/GameMode/TimeGameMode")]
public class timeGamemode : ScriptableObject
{
    public string id;
    public List<GameObject> units;
    public timeGameModeParameters[] gamemodeDifficulties;
    public int getCount { get { return gamemodeDifficulties.Length; } }
}

[Serializable]
public struct timeGameModeParameters
{
    //Just PlaceHolders
    public float startTime, addedTime;
    //X = Waves Needed to Complete Y = Time in Order to Complete those Waves
    public float[] timeIntervalForRewards;
    public int unitLossTime;

    //The Start will be incremented by the X per (Y Waves) --- Example 
    public bool spawnOnAwake;
    public float startingSpawnTarget;
    //Rate to Increase Multiplier
    public float vehicleSpawnRatePerWaveIncremental;
    //How Many Waves Before we Increment the Multiplier
    public int vehicleSpawnIncremental;

    //This will not allow the spawn rate to go over this amount
    public float vehicleSpawnRateClamp;

    //This will allow a winstate instead of an infinite playthrough
    public int wavesToComplete;
}
