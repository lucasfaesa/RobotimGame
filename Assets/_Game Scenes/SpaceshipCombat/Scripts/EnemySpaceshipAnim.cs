using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpaceshipAnim : MonoBehaviour
{
    [SerializeField] private Transform rootTransform;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private List<Transform> thrustersTransform;
    [Space]
    [SerializeField] private bool animateUpAndDown;
    [SerializeField] private bool animateRotation;
    [SerializeField] private bool animateThrusters;
    [Header("Shake settings")] 
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float strength = 1f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float randomness = 90f;
    [Header("Positioning Settings")] 
    [SerializeField] private List<Transform> movementNodes;

    private int startNode = 5;
    private int currentNode = 0;
    private int lastNodeLeft = 0;
    private int lastNodeRight = 10;
    
    private Sequence upDownSequence;
    private Sequence rotationSequence;
    private Sequence leftThrusterSequence;
    private Sequence rightThrusterSequence;

    public event Action enemyReachedMaxLeftPos;
    public event Action enemyReachedMaxRightPos;
    
    void Start()
    {
        currentNode = startNode;
        
        AnimateUpDownMovement();
        RotationMovement();
        ScaleThrusters();
    }

    public void MoveEnemyForwards(int totalWrongAnswers)
    {
        currentNode = startNode - totalWrongAnswers;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(rootTransform.DOMoveX(movementNodes[currentNode].position.x, 1.4f).SetEase(Ease.InOutSine));

        sequence.OnComplete(() =>
        {
            if (currentNode <= lastNodeLeft)
            {
                enemyReachedMaxLeftPos?.Invoke();
            }   
        });
    }

    public void MoveEnemyBackwards(int totalRightAnswers)
    {
        currentNode = startNode + totalRightAnswers;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(rootTransform.DOMoveX(movementNodes[currentNode].position.x, 1.4f).SetEase(Ease.InOutSine));
        Shake();
        sequence.OnComplete(() =>
        {
            if (currentNode >= lastNodeRight)
            {
                enemyReachedMaxRightPos?.Invoke();
            }   
        });
    }

    private void AnimateUpDownMovement()
    {
        if (!animateUpAndDown) return;

        upDownSequence = DOTween.Sequence();

        upDownSequence.Append(bodyTransform.DOMoveY(2.22f, 2.5f).SetEase(Ease.InOutSine)); 
        upDownSequence.SetLoops(-1, LoopType.Yoyo);

    }

    private void RotationMovement()
    {
        if (!animateRotation) return;

        rotationSequence = DOTween.Sequence();
        
        rotationSequence.Append(bodyTransform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(343.5f, 0, 0)), 2.1f).SetEase(Ease.InOutSine));
        rotationSequence.SetLoops(-1, LoopType.Yoyo);
    }

    private void ScaleThrusters()
    {
        if (!animateThrusters) return;

        leftThrusterSequence = DOTween.Sequence();
        rightThrusterSequence = DOTween.Sequence();
        
        leftThrusterSequence.Append(thrustersTransform[0].DOScaleZ(0.375f, 0.2f).SetEase(Ease.InBack));
        leftThrusterSequence.SetLoops(-1, LoopType.Yoyo);
        
        rightThrusterSequence.Append(thrustersTransform[1].DOScaleZ(0.375f, 0.2f).SetEase(Ease.InBack));
        rightThrusterSequence.SetLoops(-1, LoopType.Yoyo);
    }
    
    private void Shake()
    {
        upDownSequence.Pause();
        rotationSequence.Pause();
        
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(bodyTransform.DOShakePosition(duration, strength, vibrato, randomness));
        sequence.OnComplete(() =>
        {
            upDownSequence.Play();
            rotationSequence.Play();
        });

    }

    public void SlowDown()
    {
        upDownSequence.timeScale = 0.1f;
        rotationSequence.timeScale = 0.1f;
        leftThrusterSequence.timeScale = 0.1f;
        rightThrusterSequence.timeScale = 0.1f;
    }

    public void NormalSpeed()
    {
        upDownSequence.timeScale = 1f;
        rotationSequence.timeScale = 1f;
        leftThrusterSequence.timeScale = 1f; 
        rightThrusterSequence.timeScale = 1f;
    }

    private void OnDisable()
    {
        upDownSequence.Pause();
        rotationSequence.Pause();
        leftThrusterSequence.Pause();
        rightThrusterSequence.Pause();
    }
}
