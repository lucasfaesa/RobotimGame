using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using DG.Tweening;
using UnityEngine;

public class EyesAnimation : MonoBehaviour
{
    [SerializeField] private GameManagerSO gameManagerSo;
    [SerializeField] private Transform eyesTransform;
    private Sequence eyesBlinkSequence;
    
    private void OnEnable()
    {
        gameManagerSo.gameEnded += KillTweens;
    }

    private void OnDisable()
    {
        gameManagerSo.gameEnded -= KillTweens;
    }

    private void KillTweens()
    {
        eyesBlinkSequence.Kill();
    }
    
    void Start()
    {
        AnimateEyes();
    }
    
    private void AnimateEyes()
    {
        int randomDelay = Random.Range(5, 9);
        eyesBlinkSequence = DOTween.Sequence().Append(eyesTransform.DOScaleZ(0f, 0.2f).SetEase(Ease.Linear));
        eyesBlinkSequence.Append(eyesTransform.DOScaleZ(1f, 0.2f).SetEase(Ease.Linear)).AppendInterval(randomDelay);
        eyesBlinkSequence.OnComplete(AnimateEyes);
    }

}
