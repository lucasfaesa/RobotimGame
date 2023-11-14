using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcsAnimation : MonoBehaviour
{
    [Header("Whole Parts")]
    [SerializeField] private Transform wholeBody;
    [SerializeField] private Transform eyes;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;
    [SerializeField] private Transform leftLeg;
    [SerializeField] private Transform rightLeg;
    [Header("Individual Parts")]
    [SerializeField] private Transform upperRightArm;
    [SerializeField] private Transform lowerRightArm;
    [SerializeField] private Transform upperLeftArm;
    [SerializeField] private Transform lowerLeftArm;
    [SerializeField] private Transform upperRightLeg;
    [SerializeField] private Transform lowerRightLeg;
    [SerializeField] private Transform upperLeftLeg;
    [SerializeField] private Transform lowerLeftLeg;

    private Sequence blinkSequence;
    private Sequence breatheSequence;
    private Sequence moveRightArmSequence;
    private Sequence moveLeftArmSequence;
    
    void Start()
    {
        MoveArmsBackAndForward();
        ScaleUpAndDown();
        Blink();
    }

    private void MoveArmsBackAndForward()
    {
        moveLeftArmSequence = DOTween.Sequence();
        moveRightArmSequence = DOTween.Sequence();
        
        float randomDuration = Random.Range(0.8f, 1.3f);
        moveLeftArmSequence.Append(leftArm.DOLocalRotate(new Vector3(353f, 0f, 80f), randomDuration, RotateMode.Fast).SetEase(Ease.InOutSine));
        moveRightArmSequence.Append(rightArm.DOLocalRotate(new Vector3(353f, 0f, 280f), randomDuration, RotateMode.Fast).SetEase(Ease.InOutSine));

        moveLeftArmSequence.SetLoops(-1, LoopType.Yoyo);
        moveRightArmSequence.SetLoops(-1, LoopType.Yoyo);
    }

    private void ScaleUpAndDown()
    {
        breatheSequence = DOTween.Sequence();
        float randomDuration = Random.Range(0.65f, 1.1f);
        breatheSequence.Append(wholeBody.DOScaleY(0.98271f, randomDuration).SetEase(Ease.InOutSine));
        breatheSequence.SetLoops(-1, LoopType.Yoyo);
    }

    private void Blink()
    {
        float randomDelay = Random.Range(3f, 4f);
        
        blinkSequence = DOTween.Sequence();
        blinkSequence.SetDelay(randomDelay);
        blinkSequence.Append(eyes.DOScaleZ(0f, 0.2f));
        blinkSequence.SetLoops(-1,LoopType.Yoyo);
    }

    private void OnEnable()
    {
        blinkSequence.Play();
        breatheSequence.Play();
        moveRightArmSequence.Play();
        moveLeftArmSequence.Play();
    }

    private void OnDisable()
    {
        blinkSequence.Pause();
        breatheSequence.Pause();
        moveRightArmSequence.Pause();
        moveLeftArmSequence.Pause();
    }
}
