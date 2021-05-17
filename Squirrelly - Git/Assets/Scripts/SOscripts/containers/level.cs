using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName = "ScriptableObjects/Level")]
public class level : ScriptableObject
{
    [SerializeField] private string idTag;
    public Vector2 gridSize;
    public Vector3 originPoint = Vector3.zero;

    //These are the Added SO to Encapsulate the Difficulty Levels with Associated Gamemodes
    [SerializeField] private timeGamemode timeMode;
    [SerializeField] private waveGamemode waveMode;
    [SerializeField] private highScoreGamemode scoreMode;
    public timeGamemode getTimeMode { get { return timeMode; } }
    public waveGamemode getWaveMode { get { return waveMode; } }
    public highScoreGamemode getHighScoreMode { get { return scoreMode; } }

    //For Checking Against Added/Removed Levels and Optimizing the Dictionary
    public int getTotalCount { get { return getTimeMode.getCount + getWaveMode.getCount + getHighScoreMode.getCount; } } 

    public string id { get { return name + idTag; } }
}