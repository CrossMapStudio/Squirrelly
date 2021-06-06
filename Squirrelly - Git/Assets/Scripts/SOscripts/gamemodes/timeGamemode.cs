using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "timeGameMode", menuName = "ScriptableObjects/GameMode/TimeGameMode")]
public class timeGamemode : ScriptableObject
{
    public string id;
    public List<GameObject> units;
    [SerializeField] private timeGameModeParameters[] gamemodeDifficulties;
    public int getCount { get { return gamemodeDifficulties.Length; } }
}

[Serializable]
public struct timeGameModeParameters
{
    //Just PlaceHolders
    [SerializeField] private float totalTime, spawners;
}
