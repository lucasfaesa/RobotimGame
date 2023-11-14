using System;
using System.Collections.Generic;
using _Game_Scenes.Village.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Village.Scripts
{
    public class FindObjectQuestWindowAnimation : MonoBehaviour
    {
        [Header("SO")] 
        [SerializeField] private QuestManagerSO questManager;
        [SerializeField] private FindObjectQuestSO findObjectQuest;
        [Header("Content")] 
        [SerializeField] private GameObject content;
        [Header("Answer Options Settings")] 
        [SerializeField] private Button[] answersButtons;
        [SerializeField] private Image[] answersBorders;
        [SerializeField] private TextMeshProUGUI[] answersText;
        [SerializeField] private Color32 rightAnswerColor;
        [SerializeField] private Color32 wrongAnswerColor;
        
        private void OnEnable()
        {
            findObjectQuest.wrongObjectChoice += AnimateWrongAnswer;
            findObjectQuest.correctObjectChoice += AnimateRightAnswer;
            questManager.startedFindObjectQuestObjectSelection += ShowUI;
        }

        private void OnDisable()
        {
            findObjectQuest.wrongObjectChoice -= AnimateWrongAnswer;
            findObjectQuest.correctObjectChoice -= AnimateRightAnswer;
            questManager.startedFindObjectQuestObjectSelection -= ShowUI;
        }

        public void AnimateWrongAnswer(int index)
        {
            answersButtons[index].interactable = false;
            answersBorders[index].color = wrongAnswerColor;
            answersText[index].color = wrongAnswerColor;
            
            Sequence sequence = DOTween.Sequence();

            sequence.Append(answersBorders[index].transform.DOScale(0.95f, 0.2f).SetEase(Ease.InOutSine));
        }

        public void ShowUI()
        {
            Reset();
            
            content.SetActive(true);
            MovementToggle.CameraOrbitActive(false);
            MovementToggle.PlayerMovementActive(false);
            LockCursor(false);
            questManager.InOverlayToggle(true);
        }
        
        public void HideUI()
        {
            content.SetActive(false);
            MovementToggle.CameraOrbitActive(true);
            MovementToggle.PlayerMovementActive(true);
            LockCursor(true);
            questManager.InOverlayToggle(false);
        }
        
        public void AnimateRightAnswer(int index)
        {
            foreach (var button in answersButtons) { button.interactable = false; }
            
            answersBorders[index].color = rightAnswerColor;
            answersText[index].color = rightAnswerColor;
            
            Sequence sequence = DOTween.Sequence();

            sequence.Append(answersBorders[index].transform.DOScale(1.07f, 0.2f).SetEase(Ease.InOutSine)
                .SetLoops(8, LoopType.Yoyo));
            sequence.OnComplete(() =>
            {
                HideUI();
                questManager.EndQuest();
            });
        }

        private void Reset()
        {
            for (int i = 0; i < answersBorders.Length; i++)
            {
                answersButtons[i].interactable = true;
                answersBorders[i].color = Color.white;
                answersText[i].color = Color.white;
                answersBorders[i].transform.localScale = Vector3.one;
            }
        }
        
        private void LockCursor(bool status)
        {
            if (status)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        
    }
}
