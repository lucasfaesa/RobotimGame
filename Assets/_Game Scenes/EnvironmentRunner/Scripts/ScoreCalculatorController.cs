using System;
using _Lobby._LevelSelector.Scripts;
using _MeteorShower.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class ScoreCalculatorController : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private LevelSelectedManagerSO levelSelectedManagerSo;
        [SerializeField] private PointsManagerSO pointsManagerSo;

        private float startTime;
        private float endTime;
        
        private void OnEnable()
        {
            gameManager.gameStarted += GameStarted;
            gameManager.gameGoingToEnd += GameEnded;
        }

        private void OnDisable()
        {
            gameManager.gameStarted -= GameStarted;
            gameManager.gameGoingToEnd -= GameEnded;
        }

        private void GameStarted()
        {
            startTime = Time.time;
        }

        private void GameEnded()
        {
            endTime = Time.time;
            CalculateScore();
        }

        private void CalculateScore()
        {
            int baseScore = levelSelectedManagerSo.CurrentLevelInfo.level.Difficulty switch
            {
                1 => 100,
                2 => 130,
                > 3 => 155,
                _ => 100
            };
            
            float time = endTime - startTime;

            baseScore += endTime switch
            {
                < 30 => Random.Range(21, 31) //21 30
                ,
                < 60 => Random.Range(16, 21) //16 20
                ,
                < 80 => Random.Range(13, 17) //13 16
                ,
                < 100 => Random.Range(10, 14) //10 13
                ,
                < 120 => Random.Range(5, 11) //5 10
                ,
                _ => Random.Range(1, 6)
            };

            pointsManagerSo.AddPoints(baseScore);
        }
        
    }
}
