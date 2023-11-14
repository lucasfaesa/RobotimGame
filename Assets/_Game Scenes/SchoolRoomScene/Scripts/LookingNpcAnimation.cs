using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using DG.Tweening;
using UnityEngine;

public class LookingNpcAnimation : MonoBehaviour
{
    [SerializeField] private GameManagerSO gameManagerSo;
    [SerializeField] private Transform rightArmTransform;

    private Sequence moveArmSequence;
    
    void Start()
    {
        moveArmSequence = DOTween.Sequence().Append(rightArmTransform.DOLocalRotate(new Vector3(81.82f,189.58f,168.56f), 0.3f).SetEase(Ease.InOutSine));
        moveArmSequence.Append(rightArmTransform.DOLocalRotate(new Vector3(69.42825f, 183.6568f, 162.4448f), 0.3f)
        .SetEase(Ease.InOutSine));
        moveArmSequence.SetAutoKill(false).Pause();
        
        AnimateArm();
    }
    
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
        moveArmSequence.Kill();
    }

    private void AnimateArm()
    {
        moveArmSequence.Restart();
        moveArmSequence.OnComplete(AnimateArm);
    }
        
}
