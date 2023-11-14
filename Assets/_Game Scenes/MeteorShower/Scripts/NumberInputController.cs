using System;
using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumberInputController : MonoBehaviour
{
    [SerializeField] private AnswersManagerSO answersManager;
    [Space]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private MeteorsSpawnController meteorsSpawnController;
    [SerializeField] private CrystalPowerController crystalPowerController;
    [Space] 
    [SerializeField] private UnityEvent correctAnswer;
    [SerializeField] private UnityEvent incorrectAnswer;
    
    void Start()
    {
        inputField.ActivateInputField();
    }

    private void OnEnable()
    {
        inputField.onEndEdit.AddListener(CheckAnswer);
    }

    private void OnDisable()
    {
        inputField.onEndEdit.RemoveListener(CheckAnswer);
    }

    public void CheckAnswer(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        answersManager.CheckAnswer(text);
    }
}
