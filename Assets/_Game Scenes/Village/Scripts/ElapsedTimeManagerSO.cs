using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElapsedTimeManager", menuName = "ScriptableObjects/VillageScene/ElapsedTimeManager")]
public class ElapsedTimeManagerSO : ScriptableObject
{
    public float GameStartedTime { get; set; }
    public float GameEndedTime { get; set; }

    public float GetGameElapsedTime()
    {
        return Time.time - GameStartedTime;
    } 

    public void GameStarted()
    {
        GameStartedTime = Time.time;
    }

    public void GameEnded()
    {
        GameEndedTime = Time.time;
    }

    public void Reset()
    {
        GameStartedTime = 0;
        GameEndedTime = 0;
    }
}
