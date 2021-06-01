using System;
using UnityEngine;

[CreateAssetMenu(fileName = "highScoreGameMode", menuName = "ScriptableObjects/GameMode/HighScoreMode")]
public class highScoreGamemode : ScriptableObject
{
    public string id;
    [SerializeField] private highScoreModeParameters[] gamemodeDifficulties;

    public int getCount { get { return gamemodeDifficulties.Length; } }
}

[Serializable]
public struct highScoreModeParameters
{
    //Just PlaceHolders
    [SerializeField] private float spawn;
}
