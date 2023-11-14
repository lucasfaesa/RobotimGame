using System;
using System.Diagnostics.Tracing;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _MeteorShower.Scripts
{
    public class MeteorUICanvasController : MonoBehaviour
    {
        [SerializeField] private TimeManagerSO timeManager;
        [SerializeField] private PointsManagerSO pointsManager;
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private ActionsTogglerSO actionsToggler;
        [Header("Input")] 
        [SerializeField] private GameObject numbersInputFieldGameObject;
        [SerializeField] private TMP_InputField numbersInputField;
        [Header("Score")] 
        [SerializeField] private GameObject scoreGameObject;
        [SerializeField] private TextMeshProUGUI scoreText;
        [Header("Timer")] 
        [SerializeField] private TextMeshProUGUI timerText;
        [Header("Game Finished Splash")] 
        [SerializeField] private GameFinishedScreenController gameFinishedScreenScript;
        [Header("Tutorial")] 
        [SerializeField] private CanvasGroup tutorialCanvasGroup;

        private int score;
        private Coroutine lerpScoreRoutine;
        private bool updateTimer;
    
        void Start()
        {
            ShowTutorial();
            scoreText.text = "0";
            timerText.text = "";
        }

        private void OnEnable()
        {
            pointsManager.PointsUpdated += UpdateScoreText;
            timeManager.startedTimer += UpdateTimerOnHUD;
            timeManager.timeEnded += StopUpdatingTimer;
            gameManager.gameEnded += SetupEndGame;
        }

        private void OnDisable()
        {
            pointsManager.PointsUpdated -= UpdateScoreText;
            timeManager.startedTimer -= UpdateTimerOnHUD;
            timeManager.timeEnded -= StopUpdatingTimer;
            gameManager.gameEnded -= SetupEndGame;
        }

        private void UpdateTimerOnHUD()
        {
            updateTimer = true;
        }

        private void StopUpdatingTimer()
        {
            updateTimer = false;
        }
    
        private void Update()
        {
            if(updateTimer)
                timerText.text = timeManager.CurrentTime.ToString();
        }

        private void UpdateScoreText(int points)
        {
            score = points;
            Sequence sequence = DOTween.Sequence();
            float previousScore = float.Parse(scoreText.text); 
            float currentScore = float.Parse(scoreText.text);

            sequence.Append(DOTween.To(x => currentScore = x, previousScore, points, 0.5f).SetEase(Ease.InOutSine));
            sequence.OnUpdate(() =>
            {
                scoreText.text = Convert.ToInt32(currentScore).ToString();
            });
        }

        private void SetupEndGame()
        {
            numbersInputFieldGameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            scoreGameObject.SetActive(false);

            gameFinishedScreenScript.ShowGameFinishedScreen();
        }

        public void ShowTutorial()
        {
            actionsToggler.EnableCursor(true);
            tutorialCanvasGroup.gameObject.SetActive(true);
        }
        
        public void HideTutorial()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(tutorialCanvasGroup.DOFade(0f, 0.5f).SetEase(Ease.InOutSine));
            sequence.OnComplete(() =>
            {
                tutorialCanvasGroup.gameObject.SetActive(false);
                actionsToggler.EnableCursor(false);
            });
            
        }
    }
}
