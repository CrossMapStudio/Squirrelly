using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "highScoreGameMode", menuName = "ScriptableObjects/GameMode/HighScoreGameMode")]
public class highScoreGamemode : ScriptableObject
{
    public string id;
    public List<GameObject> units;
    public highScoreGameModeParameters[] gamemodeDifficulties;
    public int getCount { get { return gamemodeDifficulties.Length; } }
}

[Serializable]
public struct highScoreGameModeParameters
{
    //Just PlaceHolders
    public float startTime;
    //X = Waves Needed to Complete Y = Time in Order to Complete those Waves
    public int[] scoreToReachForAcornReward;

    //The Start will be incremented by the X per (Y Waves) --- Example 
    public bool spawnOnAwake;
    public float startingSpawnTarget;
    //Rate to Increase Multiplier
    public float vehicleSpawnRatePerWaveIncremental;
    //How Many Waves Before we Increment the Multiplier
    public int vehicleSpawnIncremental;

    //This will not allow the spawn rate to go over this amount
    public float vehicleSpawnRateClamp;
}

