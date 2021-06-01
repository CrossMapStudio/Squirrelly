using System;
using UnityEngine;

[CreateAssetMenu(fileName = "waveGameMode", menuName = "ScriptableObjects/GameMode/WaveGameMode")]
public class waveGamemode : ScriptableObject
{
    public string id;
    [SerializeField] private waveGameModeParameters[] gamemodeDifficulties;
    public int getCount { get { return gamemodeDifficulties.Length; } }
}

[Serializable]
public struct waveGameModeParameters
{
    //Just PlaceHolders
    [SerializeField] private float totalWaves, unitDeathLimit;
}
