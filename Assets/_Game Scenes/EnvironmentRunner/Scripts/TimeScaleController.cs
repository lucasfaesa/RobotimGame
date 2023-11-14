using System;
using _MeteorShower.Scripts;
using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class TimeScaleController : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private bool stopTimeOnAwake;


        private void OnEnable()
        {
            gameManager.gameStarted += GameStarted;
            
        }

        private void OnDisable()
        {
            gameManager.gameStarted -= GameStarted;
            
        }

        private void Awake()
        {
            if (stopTimeOnAwake)
                Time.timeScale = 0;
        }

        private void GameStarted()
        {
            Time.timeScale = 1;
        }

    }
}
