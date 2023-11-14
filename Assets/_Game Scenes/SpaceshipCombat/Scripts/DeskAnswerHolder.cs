using System.Collections;
using System.Collections.Generic;
using API_Mestrado_Lucas;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeskAnswerHolder : MonoBehaviour
{
    [SerializeField] private BoxCollider triggerCollider;
    [Space]
    [SerializeField] [ReadOnly] private QuestionAnswerDTO answerOption;
    [SerializeField] private GameObject answerLetterGameObject;
    public bool CanBeInteracted { get; private set; } = true;
    

    public void AssignAnswerOption(QuestionAnswerDTO answer)
    {
//        Debug.Log(answer.answerString + " " + answer.isCorrectAnswer);
        answerOption = answer;
        
        answerLetterGameObject.transform.localScale = new Vector3(1,0,1);
        answerLetterGameObject.SetActive(true);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(answerLetterGameObject.transform.DOScaleY(1f, 0.62f).SetEase(Ease.InOutBack));
        
                
        CanBeInteracted = true;
        triggerCollider.enabled = true;
    }

    public void HideAnswersOptions()
    {
        CanBeInteracted = false;
        triggerCollider.enabled = false;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(answerLetterGameObject.transform.DOScaleY(0f, 0.62f).SetEase(Ease.InOutBack));
        sequence.OnComplete(() =>
        {
            answerLetterGameObject.SetActive(false);
        });
        
    }

    public QuestionAnswerDTO GetAnswer() => answerOption;
}
