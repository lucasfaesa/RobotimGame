using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApiCommsController", menuName = "ScriptableObjects/APIComms/ApiCommsController")]
public class ApiCommsControllerSO : ScriptableObject
{
    [SerializeField] private bool useComms;
    [SerializeField] private bool useCloudPath;


    public bool UseComms
    {
        get
        {
            #if UNITY_EDITOR
                return useComms;
            #else
                return true;
            #endif
        }
        set => useComms = value;
    }

    
    public bool UseCloudPath
    {
        get
        {
            #if UNITY_EDITOR
                return useCloudPath;
            #else
                return true;
            #endif
        }
        set => useCloudPath = value;
    }

    [field:Header("Only to be used in editor")]
    [field:SerializeField] public bool SaveScoreOnTxt { get; set;}

    public event Action startedDatabaseGameStartCommunication;
    public event Action startedDatabaseGameEndCommunication;
    public event Action gameStartDataSentSuccessfully;
    public event Action gameStartDataSentError;
    public event Action gameEndDataSentSuccessfully;
    public event Action gameEndDataSentError;
    public event Action finishedDatabaseGameStartCommunication;
    public event Action finishedDatabaseGameEndCommunication;
    
    public void StartedDatabaseGameStartCommunication()
    {
        startedDatabaseGameStartCommunication?.Invoke();
    }

    public void GameStartDataSentSuccessfully()
    {
        gameStartDataSentSuccessfully?.Invoke();
    }

    public void GameStartDataSentError()
    {
        gameStartDataSentError?.Invoke();
    }

    public void FinishedDatabaseGameStartCommunication()
    {
        finishedDatabaseGameStartCommunication?.Invoke();
    }
        
    public void StartedDatabaseEndGameCommunication()
    {
        startedDatabaseGameEndCommunication?.Invoke();
    }

    public void GameEndDataSentSuccessfully()
    {
        gameEndDataSentSuccessfully?.Invoke();
    }

    public void GameEndDataSentError()
    {
        gameEndDataSentError?.Invoke();
    }

    public void FinishedDatabaseEndGameCommunication()
    {
        finishedDatabaseGameEndCommunication?.Invoke();
    }
    
}
