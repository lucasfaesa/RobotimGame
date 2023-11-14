using System;
using _MeteorShower.Scripts;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class IdleNpcAnimation : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManagerSo;
        [SerializeField] private Transform headTransform;
        [SerializeField] private Transform armsTransform;

        private Sequence headMovementSequence;
      
        private Sequence armsMovementSequence;

        private int headTarget = 0;

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
            headMovementSequence.Kill();
            armsMovementSequence.Kill();
        }

        private void Start()
        {
            AnimateHead();
            AnimateArms();
        }

        private void AnimateHead()
        {
            int randomDelay = Random.Range(3, 7);
            Vector3 targetPos = new Vector3(headTarget = headTarget == 0 ? 330 : 0, headTransform.localRotation.y,
                headTransform.localRotation.z);

            headMovementSequence = DOTween.Sequence();
            
            headMovementSequence.Append(headTransform.DOLocalRotate(targetPos, 0.7f,RotateMode.Fast).SetEase(Ease.InOutSine)).AppendInterval(randomDelay);
            headMovementSequence.OnComplete(()=>
            {
                AnimateHead();
            });
        }

        private void AnimateArms()
        {
            armsMovementSequence = DOTween.Sequence();
            armsMovementSequence.Append(armsTransform.DOLocalRotate(new Vector3(1,0,0), 1f).SetEase(Ease.InOutSine));
            armsMovementSequence.Append(armsTransform.DOLocalRotate(new Vector3(0,0,0), 1f).SetEase(Ease.InOutSine));
            armsMovementSequence.OnComplete(()=>
            {
                AnimateArms();
            });
        }
        
       
    }
}
