using System;
using UnityEngine;

[CreateAssetMenu(fileName = "waveGameMode", menuName = "ScriptableObjects/GameMode/WaveGameMode")]
public class waveGamemode : ScriptableObject
{
    public string id;
    public waveGameModeParameters[] gamemodeDifficulties;
    public int getCount { get { return gamemodeDifficulties.Length; } }
}

[Serializable]
public struct waveGameModeParameters
{
    //X = Waves Needed to Complete Y = Time in Order to Complete those Waves
    public int[] unitLossLimitsForRewards;

    //The Start will be incremented by the X per (Y Waves) --- Example 
    public bool spawnOnAwake;
    public float startingSpawnTarget;
    public float vehicleSpawnRatePerWaveIncremental;
    //How Many Waves Before we Increment the Multiplier
    public int vehicleSpawnIncremental;

    //This will not allow the spawn rate to go under this amount
    public float vehicleSpawnRateClamp;

    //This will allow a winstate instead of an infinite playthrough
    public int wavesToComplete;
    public int unitLossTarget;
}
