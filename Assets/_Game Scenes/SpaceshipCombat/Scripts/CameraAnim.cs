using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAnim : MonoBehaviour
{
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float targetYPos;
    [SerializeField] private bool animateUpAndDown;
    [Header("Shake settings")]
    [SerializeField] private float strength = 1f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float randomness = 90f;
    
    private Sequence upDownSequence;
    
    
    void Start()
    {
        AnimateUpDownMovement();
    }
    
    private void AnimateUpDownMovement()
    {
        if (!animateUpAndDown) return;

        upDownSequence = DOTween.Sequence();

        upDownSequence.Append(mainCamera.transform.DOMoveY(targetYPos, 2.5f).SetEase(Ease.InOutSine)); 
        upDownSequence.SetLoops(-1, LoopType.Yoyo);

    }

    public void Shake(float duration)
    {
        upDownSequence.Pause();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(cameraHolder.DOShakePosition(duration, strength, vibrato, randomness));
        sequence.OnComplete(() =>
        {
            upDownSequence.Play();
        });
    }

    public void ZoomOutAndIn(float inDuration, float outDuration, float stayDuration)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(x => mainCamera.fieldOfView = x, 60, 64, inDuration).SetEase(Ease.InOutSine));
        sequence.Append(
            DOTween.To(x => mainCamera.fieldOfView = x, 64, 60, outDuration).SetEase(Ease.InOutSine).SetDelay(stayDuration));
    }
    
    public void ZoomInAndOut(float inDuration, float outDuration, float stayDuration)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(x => mainCamera.fieldOfView = x, 60, 58, inDuration).SetEase(Ease.InOutSine));
        sequence.Append(
            DOTween.To(x => mainCamera.fieldOfView = x, 58, 60, outDuration).SetEase(Ease.InOutSine).SetDelay(stayDuration));
    }
    
    public void MoveLeftAndBack(float inDuration, float outDuration, float stayDuration){
        
        Sequence sequence = DOTween.Sequence();

        sequence.Append(mainCamera.transform.DOMoveX(-1f, inDuration).SetEase(Ease.InOutBounce));
        sequence.Append(mainCamera.transform.DOMoveX(0f, outDuration).SetEase(Ease.InOutBounce).SetDelay(stayDuration));
    }
    
    public void MoveRightAndBack(float inDuration, float outDuration, float stayDuration){
        Sequence sequence = DOTween.Sequence();

        sequence.Append(mainCamera.transform.DOMoveX(1f, inDuration).SetEase(Ease.InOutBack));
        sequence.Append(mainCamera.transform.DOMoveX(0f, outDuration).SetEase(Ease.InOutBack).SetDelay(stayDuration));
    }
}
