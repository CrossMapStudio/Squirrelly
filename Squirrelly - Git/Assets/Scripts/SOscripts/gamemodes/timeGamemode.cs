using System;
using UnityEngine;

[CreateAssetMenu(fileName = "timeGameMode", menuName = "ScriptableObjects/GameMode/TimeGameMode")]
public class timeGamemode : ScriptableObject
{
    [SerializeField] private timeGameModeParameters[] gamemodeDifficulties;
    public int getCount { get { return gamemodeDifficulties.Length; } }
}

[Serializable]
public struct timeGameModeParameters
{
    //Just PlaceHolders
    [SerializeField] private float totalTime, spawners;
}
