using System;
using _MeteorShower.Scripts;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class DefeatNpcAnimation : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManagerSo;
        [SerializeField] private Transform xObject;
        
        private Sequence xSequence;
        
        void Start()
        {
            xSequence = DOTween.Sequence().Append(xObject.DOScale(1.1679f, 0.3f).SetEase(Ease.InOutSine));
            xSequence.Append(xObject.DOScale(1f, 0.3f).SetEase(Ease.InOutSine));
            
            AnimateX();
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
            xSequence.Kill();
        }

        private void AnimateX()
        {
            xSequence.Restart();
            xSequence.OnComplete(AnimateX);
        }
    }
}
