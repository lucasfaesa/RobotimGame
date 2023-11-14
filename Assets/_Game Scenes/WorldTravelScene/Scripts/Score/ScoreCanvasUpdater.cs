using System;
using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreCanvasUpdater : MonoBehaviour
{
    [SerializeField] private PointsManagerSO pointsManager;
    [Space]
    [SerializeField] private TextMeshProUGUI scoreTxt;
    
    private void OnEnable() { pointsManager.PointsUpdated += UpdateScore; }

    private void OnDisable() { pointsManager.PointsUpdated -= UpdateScore; }

    private void UpdateScore(int score)
    {
        float currentScore = Convert.ToInt32(scoreTxt.text);
        Sequence sequence = DOTween.Sequence().Append(DOTween.To(x => currentScore = x, currentScore, score, 1f));
        sequence.OnUpdate(() =>
        {
            scoreTxt.text = Convert.ToInt32(currentScore).ToString();
        });
    }
}
