using System;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.SkinChanger.Scripts
{
    public class Animation : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform linesTransform;
        [SerializeField] private Transform[] colorPickersTransform;
        [SerializeField] private CanvasGroup saveAndExitButton;
        private void Awake()
        {
            playerTransform.localScale = Vector3.zero;
            linesTransform.localPosition = new Vector3(-0.492f, 0f, -15f);
            saveAndExitButton.alpha = 0f;
            foreach (var t in colorPickersTransform)
            {
                t.localScale = Vector3.zero;
            }
            
            Animate();
        }

        private void Animate()
        {
            Sequence playerSequence =
                DOTween.Sequence().Append(playerTransform.DOScale(1f, 0.65f).SetEase(Ease.OutBack)).PrependInterval(0.4f);

            Sequence linesSequence = DOTween.Sequence().Append(linesTransform.DOMoveZ(4.066f, 0.3f).SetEase(Ease.InOutSine)).PrependInterval(1f);

            Sequence colorPickers = DOTween.Sequence()
                .Append(colorPickersTransform[0].DOScale(1f, 0.3f).SetEase(Ease.OutBack)).PrependInterval(1.3f);
            colorPickers.Append(colorPickersTransform[1].DOScale(1f, 0.3f).SetEase(Ease.OutBack));
            colorPickers.Append(colorPickersTransform[2].DOScale(1f, 0.3f).SetEase(Ease.OutBack));

            Sequence buttonSequence =
                DOTween.Sequence().Append(saveAndExitButton.DOFade(1f, 0.3f)).PrependInterval(2.2f);
        }
    }
}
