using System;
using System.Collections;
using System.Collections.Generic;
using _Game_Scenes.SpaceshipCombat.Scripts;
using API_Mestrado_Lucas;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionsDisplay : MonoBehaviour
{
    [SerializeField] private QuestionsManagerSO questionManager;
    [Space]
    [SerializeField] private RectTransform questionContentRect;
    [SerializeField] private CanvasGroup questionsContentCanvasGroup;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private List<TextMeshProUGUI> answerTexts;
    [SerializeField] private Image timerSprite;
    
    public event Action timesUp;

    private Sequence timerSequence;

    private QuestionDTO currentQuestion;

    private void OnEnable()
    {
        questionManager.questionAnswered += PauseTimer;
    }

    private void OnDisable()
    {
        questionManager.questionAnswered -= PauseTimer;
    }

    private void Awake()
    {
        questionsContentCanvasGroup.alpha = 0f;
        questionsContentCanvasGroup.gameObject.SetActive(false);
        questionContentRect.anchoredPosition = new Vector2(0, 672f);
    }

    public void ShowQuestionOnCanvas(QuestionDTO question, List<QuestionAnswerDTO> answers)
    {
        currentQuestion = question;
        titleText.text = question.QuestionTitle;
        
        int cont = 0;
        for (int i = 0; i < answerTexts.Count; i++)
        {
            answerTexts[i].text = cont == 0 ? "A. " : cont == 1 ? "B. " : cont == 2 ? "C. " : "D. ";
           
            answerTexts[i].text += answers[i].AnswerString;
            
            cont++;
        }
        
        ShowQuestionCanvas();
    }

    private void ShowQuestionCanvas()
    {
        timerSprite.fillAmount = 1f;
        
        Sequence sequence = DOTween.Sequence();
        
        questionsContentCanvasGroup.gameObject.SetActive(true);
        questionsContentCanvasGroup.alpha = 0f;
        
        questionManager.BeginToShowQuestion();
        sequence.Append(questionContentRect.DOAnchorPosY(-20f, 0.5f).SetEase(Ease.InOutSine));
        sequence.Insert(0,questionsContentCanvasGroup.DOFade(1f, 0.7f).SetEase(Ease.InOutSine));
        sequence.OnComplete(() =>
        {
            questionManager.QuestionShown();
            StartTimer();
        });
        
    }

    public void HideQuestionCanvas()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(questionContentRect.DOAnchorPosY(672f, 0.5f).SetEase(Ease.InOutSine));
        sequence.Insert(0,questionsContentCanvasGroup.DOFade(0f, 0.35f).SetEase(Ease.InOutSine));

        sequence.OnComplete(() =>
        {
            questionsContentCanvasGroup.gameObject.SetActive(false);
        });
    }

    public void StartTimer()
    {
        questionManager.QuestionTimeStarted();
        
        if (timerSequence == null)
        {
            timerSequence = DOTween.Sequence();
            timerSequence.Append(DOTween.To(x => timerSprite.fillAmount = x, 1f, 0f, currentQuestion.QuestionTimeLimit).SetEase(Ease.Linear));
        }
        else
        {
            timerSequence.Restart();
        }

        timerSequence.OnComplete(() =>
        {
            timesUp?.Invoke();
            timerSequence.Rewind();
        });
    }

    public void PauseTimer()
    {
        if(timerSequence.IsPlaying())
            timerSequence.Pause();
    }
}
