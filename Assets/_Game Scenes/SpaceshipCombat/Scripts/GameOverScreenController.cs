using System;
using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using APIComms;
using DG.Tweening;
using UnityEngine;

public class GameOverScreenController : MonoBehaviour
{
    [Header("SO")]
    [SerializeField] private ApiCommsControllerSO apiCommsController;
    [SerializeField] private PlayerDataSO playerData;
    [Space]
    [SerializeField] private CanvasGroup contentCanvasGroup;
    [SerializeField] private CanvasGroup titleCanvasGroup;
    [SerializeField] private CanvasGroup buttonsCanvasGroup;
    
    private void OnEnable()
    {
        if(apiCommsController.UseComms && !playerData.GuestMode)
            apiCommsController.finishedDatabaseGameEndCommunication += ShowButtons;
    }

    private void OnDisable()
    {
        if(apiCommsController.UseComms && !playerData.GuestMode)
            apiCommsController.finishedDatabaseGameEndCommunication -= ShowButtons;
    }


    public void ShowGameOverScreen()
    {
        contentCanvasGroup.alpha = 0f;
        titleCanvasGroup.alpha = 0f;
        buttonsCanvasGroup.alpha = 0f;
        contentCanvasGroup.gameObject.SetActive(true);
        
        Sequence contentSequence = DOTween.Sequence();
        Sequence titleSequence = DOTween.Sequence();

        titleSequence.Pause();

        contentSequence.Append(contentCanvasGroup.DOFade(1f, 0.7f).SetEase(Ease.InOutSine));
        contentSequence.OnComplete(() =>
        {
            titleSequence.Play();
        });
        
        titleSequence.Append(titleCanvasGroup.DOFade(1f, 0.5f).SetEase(Ease.InOutSine));
        titleSequence.OnComplete(() =>
        {
            if (!apiCommsController.UseComms)
                ShowButtons();
        });
        
        if(playerData.GuestMode)
            ShowButtons();
        
    }

    private void ShowButtons()
    {
        Sequence buttonsSequence = DOTween.Sequence();
        buttonsSequence.Append(buttonsCanvasGroup.DOFade(1f, 0.5f).SetEase(Ease.InOutSine));
        //buttonsSequence.Play();
    }
}
