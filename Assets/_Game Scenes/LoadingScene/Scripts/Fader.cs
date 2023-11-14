using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _LoadingScene.Scripts
{
    public class Fader : MonoBehaviour
    {
        [Space]
        [SerializeField] private Image black;
            
        public event Action fadeToBlackCompleted;
        public event Action unfadeCompleted;
    
        public void FadeToBlack()
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, 0f);
            black.gameObject.SetActive(true);
                
            Sequence sequence = DOTween.Sequence();
                
            sequence.Append(black.DOFade(1f, 0.3f));
            sequence.OnComplete(() =>
            {
                fadeToBlackCompleted?.Invoke();
            });
        }

        public void Unfade()
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, 1f);
            black.gameObject.SetActive(true);
                
            Sequence sequence = DOTween.Sequence();
                
            sequence.Append(black.DOFade(0f, 0.3f));
            sequence.OnComplete(() =>
            {
                unfadeCompleted?.Invoke();
                black.gameObject.SetActive(false);
            });
        }
            
    }
}

