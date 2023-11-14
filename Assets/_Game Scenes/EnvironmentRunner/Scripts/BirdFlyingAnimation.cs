using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BirdFlyingAnimation : MonoBehaviour
{
    [SerializeField] private Transform birdTotalTransform;
    [SerializeField] private Transform birdModelTransform;
    [SerializeField] private Transform birdLeftWing;
    [SerializeField] private Transform birdRightWing;
    [Space]
    [SerializeField] private Vector3 targetRotationLeftWing;
    [SerializeField] private Vector3 targetRotationRightWing;
    [Space]
    [SerializeField] private Vector3 positionToBeShow;
    [SerializeField] private Vector3 positionToHide;

    private Sequence shownSequence;
    private Sequence hideSequence;
    public bool IsShowOnScreen { get; private set; }
    
    
    // Start is called before the first frame update
    void Start()
    {
        birdModelTransform.DOLocalMoveY(-1f, 2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        birdModelTransform.DOLocalMoveZ(0.66f, 1.7f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        
        Sequence leftWingSequence = DOTween.Sequence().Append(birdLeftWing.DOLocalRotate(targetRotationLeftWing,
            0.4f).SetEase(Ease.InOutSine)).SetLoops(-1, LoopType.Yoyo);
        Sequence rightWingSequence = DOTween.Sequence().Append(birdRightWing.DOLocalRotate(targetRotationRightWing,
            0.4f).SetEase(Ease.InOutSine)).SetLoops(-1, LoopType.Yoyo);

        Sequence rotateSequence = DOTween.Sequence()
            .Append(birdModelTransform.DOLocalRotate(new Vector3(0, 0, 360f), 1f, RotateMode.LocalAxisAdd)).SetEase(Ease.Linear)
            .SetDelay(Random.Range(5, 9)).SetLoops(-1);

        shownSequence = DOTween.Sequence().Append(birdTotalTransform.transform.DOMove(positionToBeShow, 2f).SetEase(Ease.InOutSine)).Pause().SetAutoKill(false);
        hideSequence = DOTween.Sequence().Append(birdTotalTransform.transform.DOMove(positionToHide, 2f).SetEase(Ease.InOutSine)).Pause().SetAutoKill(false);
        
        this.gameObject.SetActive(false);
    }

    public void ShowOnScreen()
    {
        if (IsShowOnScreen) return;
        IsShowOnScreen = true;

        hideSequence.Pause();
        shownSequence.Restart();
    }
    
    public void HideOffscreen()
    {
        if (!IsShowOnScreen) return;
        
        IsShowOnScreen = false;

        shownSequence.Pause();
        hideSequence.Restart();
        
        hideSequence.OnComplete(()=>
        {
            this.gameObject.SetActive(false);
        });
    }
}
