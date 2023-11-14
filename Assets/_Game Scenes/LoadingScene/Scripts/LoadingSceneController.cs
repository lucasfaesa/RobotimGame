using System;
using System.Collections;
using System.Collections.Generic;
using _LoadingScene.Scripts;
using _Lobby._LevelSelector.Scripts;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    [Header("SO")] 
    [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
    [SerializeField] private SceneLoaderControllerSO sceneLoaderController;
    [SerializeField] private LevelImageContextControllerSO levelImageContextController;
    [Space]
    [SerializeField] private Fader fader;
    [Header("LevelName")] 
    [SerializeField] private TextMeshProUGUI levelName;
    [Header("ImagePreview")] 
    [SerializeField] private Image levelPreviewImage;
    [Header("Animation")] 
    [SerializeField] private GameObject wrenchSprite;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image loadingProgressBar;

    private float delayBetweenLoadingDots = 0.7f;
    private float elapsedTime;
    private bool canGoToScene;
    private bool fadeStarted;
    private Sequence animationSequence;

    private ContextMenuPreviewDataSO sceneContext;
    
    private void OnDisable()
    {
        animationSequence.Kill();
    }

    private void FadeToBlackCompleted()
    {
        canGoToScene = true;
    }
    
    void Start()
    {
        sceneContext = levelImageContextController.GetContextMenuPreviewBySceneName(sceneLoaderController.SceneToBeLoaded);
        
        levelName.text = sceneContext.LevelName;
        SetPreviewImage();
        Animate();
        StartCoroutine(LoadSceneAsync());
    }

    private void SetPreviewImage()
    {
        if (sceneContext.LoadingImagePreview != null)
            levelPreviewImage.sprite = sceneContext.LoadingImagePreview;
    }
    
    private void Animate()
    {
        animationSequence = DOTween.Sequence();
        
        animationSequence.Append(wrenchSprite.transform.DORotate(new Vector3(0,0,-360f), 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        animationSequence.SetLoops(-1, LoopType.Restart);
        animationSequence.OnUpdate(() =>
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= delayBetweenLoadingDots && elapsedTime < delayBetweenLoadingDots*2)
            {
                loadingText.text = "Carregando.";
            }
            if (elapsedTime >= delayBetweenLoadingDots*2 && elapsedTime < delayBetweenLoadingDots*3)
            {
                loadingText.text = "Carregando..";
            }
            if (elapsedTime >= delayBetweenLoadingDots*3)
            {
                loadingText.text = "Carregando...";
                elapsedTime = 0f;
            }
        });


    }

    private IEnumerator LoadSceneAsync()
    {
        if (sceneLoaderController.SceneToBeLoaded == "")
            throw new Exception("String de cena vazio");

        loadingProgressBar.fillAmount = 0f;
        
        yield return new WaitForSeconds(1f);
        //canGoToScene = true;
        

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLoaderController.SceneToBeLoaded);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            loadingProgressBar.fillAmount = operation.progress;

            if (operation.progress >= 0.9f && !fadeStarted)
            {
                fadeStarted = true;
                fader.FadeToBlack();
            }
            
            //if(canGoToScene)
            operation.allowSceneActivation = true;

            yield return null;
        }
    }
}
