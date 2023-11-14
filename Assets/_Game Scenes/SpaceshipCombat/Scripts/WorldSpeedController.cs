using System;
using _Game_Scenes.SpaceshipCombat.Scripts;
using _MeteorShower.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _SpaceshipCombat.Scripts
{
    public class WorldSpeedController : MonoBehaviour
    {
        [Header("SO")]
        [SerializeField] private QuestionsManagerSO questionsManager;
        [SerializeField] private GameManagerSO gameManager;
        [Space]
        [SerializeField] private UnityEvent normalSpeedWorld;
        [SerializeField] private UnityEvent slowDownWorld;

        private bool slowWorld;

        private void Awake()
        {
            PauseTimeScale();
        }

        private void OnEnable()
        {
            gameManager.gameStarted += UnpauseTimeScale;
            questionsManager.beginToShowQuestion += SlowDownTime;
            questionsManager.questionAnswered += NormalTime;
        }

        private void OnDisable()
        {
            gameManager.gameStarted -= UnpauseTimeScale;
            questionsManager.beginToShowQuestion -= SlowDownTime;
            questionsManager.questionAnswered -= NormalTime;
        }

        private void PauseTimeScale()
        {
            Time.timeScale = 0f;
        }

        private void UnpauseTimeScale()
        {
            Debug.Log("Unpaused");
            Time.timeScale = 1f;
        }
        
        private void NormalTime()
        {
            normalSpeedWorld?.Invoke();
        }

        private void SlowDownTime()
        {
            slowDownWorld?.Invoke();
        }

        private void Update()
        {
            if (Keyboard.current.kKey.wasPressedThisFrame)
            {
                slowWorld = !slowWorld;

                if (slowWorld)
                    slowDownWorld?.Invoke();
                else
                    normalSpeedWorld?.Invoke();
            }
        }
    }
}
