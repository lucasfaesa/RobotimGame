using System;
using _MeteorShower.Scripts;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class MicroscopeNpcAnimation : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManagerSo;
        [SerializeField] private Transform npcBodyTransform;

        private Sequence sequence;
        
        private float _initialScale = 8.85f;
        private float _targetScale = 9f;
        
        
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
            sequence.Kill();
        }
        
        private void Start()
        {
            sequence = DOTween.Sequence().Append(npcBodyTransform.DOScale(_targetScale, 0.2f).SetEase(Ease.InOutSine));
            sequence.Append(npcBodyTransform.DOScale(_initialScale, 0.2f).SetEase(Ease.InOutSine));
            
            Animate();
        }
        
        

        private void Animate()
        {
            sequence.Restart();
            sequence.OnComplete(Animate);
        }

      
    }
}
