using System;
using UnityEngine;

namespace _MeteorShower.Scripts
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField] private TimeManagerSO timeManager;
        [SerializeField] private GameManagerSO gameManager;

        private float timer;
        private bool countTime;
        
        private void OnEnable()
        {
            gameManager.gameStarted += StartTimer;
        }

        private void OnDisable()
        {
            gameManager.gameStarted -= StartTimer;
        }

        private void StartTimer()
        {
            timeManager.StartTimer();
            timer = timeManager.LevelTimer;
            countTime = true;
        }

        private void Update()
        {
            if (countTime)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    timeManager.CurrentTime = (int)timer;
                }
                    
                else
                {
                    timeManager.TimeEnded();
                    gameManager.GameGoingToEnd();
                    countTime = false;
                }
            }
        }
        
    }    
}

