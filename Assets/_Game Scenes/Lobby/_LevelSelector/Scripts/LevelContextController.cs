using System;
using System.Collections.Generic;
using System.Linq;
using API_Mestrado_Lucas;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Lobby._LevelSelector.Scripts
{
    public class LevelContextController : MonoBehaviour
    {
        [Header("SO")]
        [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
        [SerializeField] private LevelImageContextControllerSO levelImageContextController;
        [Space] 
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI maxScoreText;
        [SerializeField] private Image imagePreview;
        [SerializeField] private Button playButton;
        [Header("Animation")] 
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float shownAnchoredXPos;
        [SerializeField] private float hiddenAnchoredXPos;

        private bool shown;
        
        private void OnEnable()
        {
            levelSelectedManager.levelSelected += SetContext;
        }

        private void OnDisable()
        {
            levelSelectedManager.levelSelected -= SetContext;
            rectTransform.anchoredPosition = new Vector2(hiddenAnchoredXPos, rectTransform.anchoredPosition.y);
            shown = false;
        }

        private void SetContext(LevelInfoAndScore levelInfoAndScore)
        {
            if (levelInfoAndScore.level != null)
            {
                titleText.text = levelInfoAndScore.subjectThemeDto.Name;
            }

            if (levelInfoAndScore.quiz != null)
            {
                titleText.text = levelInfoAndScore.quiz.Name;
            }
            
            maxScoreText.text = "Pontuação Max.: " + levelInfoAndScore.score;

            var preview = levelImageContextController.GetContextMenuPreview(levelInfoAndScore);

            if (preview.ContextImagePreview != null)
                imagePreview.sprite = preview.ContextImagePreview;

            if (!shown)
            {
                float newXPos = 0;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(DOTween.To(x => newXPos = x, rectTransform.anchoredPosition.x, shownAnchoredXPos, 0.3f).SetEase(Ease.InOutSine));
                sequence.OnUpdate(() =>
                {
                    rectTransform.anchoredPosition = new Vector2(newXPos, rectTransform.anchoredPosition.y);
                });
            }
        }
    }
}
