using System;
using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using UnityEngine;

public class AnswersController : MonoBehaviour
{
    [SerializeField] private PointsManagerSO pointsManager;
    [SerializeField] private AnswersManagerSO answersManager;

    private void OnEnable()
    {
        answersManager.wrongAnswer += WrongAnswer;
    }

    private void OnDisable()
    {
        answersManager.wrongAnswer -= WrongAnswer;
    }

    public void WrongAnswer()
    {
        pointsManager.RemovePoints();
    }
}
