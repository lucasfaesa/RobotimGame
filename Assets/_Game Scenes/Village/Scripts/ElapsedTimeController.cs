using System;
using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using UnityEngine;

public class ElapsedTimeController : MonoBehaviour
{
    [SerializeField] private ElapsedTimeManagerSO elapsedTimeManager;
    [SerializeField] private GameManagerSO gameManager;
    
    private void OnEnable()
    {
        gameManager.gameStarted += GameStarted;
        gameManager.gameEnded += GameEnded;
    }

    private void OnDisable()
    {
        gameManager.gameStarted -= GameStarted;
        gameManager.gameEnded -= GameEnded;
    }

    private void Awake()
    {
        elapsedTimeManager.Reset();
    }

    private void GameStarted()
    {
        elapsedTimeManager.GameStarted();
    }

    private void GameEnded()
    {
        elapsedTimeManager.GameEnded();
    }
}
