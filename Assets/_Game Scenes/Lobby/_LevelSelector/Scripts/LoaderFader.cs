using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Lobby._LevelSelector.Scripts
{
    public class LoaderFader : MonoBehaviour
    {
        [Header("SO")] 
        [SerializeField] private SceneLoaderControllerSO sceneLoaderController;
        [Space]
        [SerializeField] private Image black;
        
        public event Action fadeToBlackCompleted;
        public event Action unfadeCompleted;

        public void LoadSelectedLevel()
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, 0f);
            black.gameObject.SetActive(true);
            
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(black.DOFade(1f, 0.3f));
            sequence.OnComplete(() =>
            {
                fadeToBlackCompleted?.Invoke();
                sceneLoaderController.GoToSelectedLevelScene();
            });
        }

        public void LoadScene(string sceneName)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, 0f);
            black.gameObject.SetActive(true);
            
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(black.DOFade(1f, 0.3f));
            sequence.OnComplete(() =>
            {
                fadeToBlackCompleted?.Invoke();
                sceneLoaderController.GoToScene(sceneName);
            });
        }

        public void LoadSceneAfterDelay(string sceneName)
        {
            StartCoroutine(LoadSceneAfterSeconds(sceneName));
        }
        
        private IEnumerator LoadSceneAfterSeconds(string sceneName)
        {
            yield return new WaitForSeconds(1f);
            
            black.color = new Color(black.color.r, black.color.g, black.color.b, 0f);
            black.gameObject.SetActive(true);
            
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(black.DOFade(1f, 0.3f));
            sequence.OnComplete(() =>
            {
                fadeToBlackCompleted?.Invoke();
                sceneLoaderController.GoToScene(sceneName);
            });
        }
        
        public void LoadSceneWithoutLoadingScene(string sceneName)
        {
            black.color = new Color(black.color.r, black.color.g, black.color.b, 0f);
            black.gameObject.SetActive(true);
            
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(black.DOFade(0f, 0.3f));
            sequence.OnComplete(() =>
            {
                fadeToBlackCompleted?.Invoke();
                sceneLoaderController.GoToScene(sceneName);
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
