using System;
using _MeteorShower.Scripts;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace _Village.Scripts
{
    public class DollyCameraController : MonoBehaviour
    {
        [Header("SO")] 
        [SerializeField] private GameManagerSO gameManager;
        [Space] 
        [SerializeField] private CinemachineVirtualCamera dollyCamera;
        [Header("Events")] 
        [SerializeField] private UnityEvent dollyAnimationEnded;

        private CinemachineTrackedDolly dollyComponent;
        private void OnEnable()
        {
            gameManager.preparingToStartGame += AnimateDolly;
        }

        private void OnDisable()
        {
            gameManager.preparingToStartGame -= AnimateDolly;
        }

        private void Start()
        {
            dollyComponent = dollyCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }

        private void AnimateDolly()
        {
            Sequence dollySequence = DOTween.Sequence();

            dollySequence.Append(DOTween.To(x=> dollyComponent.m_PathPosition = x, 0, 1, 3f).SetEase(Ease.InOutSine).SetDelay(0.5f));

            dollySequence.OnComplete(()=>
            {
                dollyAnimationEnded?.Invoke();
                gameManager.GameStarted();
            });
        }        
    }
}
