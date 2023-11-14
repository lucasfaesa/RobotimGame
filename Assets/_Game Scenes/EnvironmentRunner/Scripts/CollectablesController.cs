using System;
using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using _Village.Scripts;
using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class CollectablesController : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManagerSo;
        [SerializeField] private ActionsTogglerSO actionsTogglerSo;
        [SerializeField] private LevelCompletionSO levelCompletionSo;
        [SerializeField] private ObstacleEventChannelSO obstacleEventChannel;
        [SerializeField] private CollectablesManagerSO collectablesManager;
        [SerializeField] private GameFinishedScreenController gameFinishedScreenController;
        
        private bool gameEnding;
        
        private void OnEnable()
        {
            obstacleEventChannel.obstacleHitPlayer += DeductItemsFromPlayer;
            collectablesManager.allItemsCollected += GameGoingToEnd;
        }

        private void OnDisable()
        {
            obstacleEventChannel.obstacleHitPlayer -= DeductItemsFromPlayer;
            collectablesManager.allItemsCollected -= GameGoingToEnd;
        }

        private void GameGoingToEnd()
        {
            levelCompletionSo.LevelCompleted = true;
            gameManagerSo.GameGoingToEnd();
            StartCoroutine(EndGameAfterSeconds(3f));
        }

        private IEnumerator EndGameAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            
            gameManagerSo.GameEnded();
            actionsTogglerSo.EnableCursor(true);
            gameFinishedScreenController.ShowGameFinishedScreen();
        }

        private void DeductItemsFromPlayer(RoadObstacleSpawner.ObstacleType type)
        {
            switch (type)
            {
                case RoadObstacleSpawner.ObstacleType.OilSpillObstacle:
                    collectablesManager.SubtractRandomItem(1);
                    break;
                case RoadObstacleSpawner.ObstacleType.TrashCanObstacle:
                    collectablesManager.SubtractRandomItem(1);
                    break;
                case RoadObstacleSpawner.ObstacleType.FactoryObstacle:
                    collectablesManager.SubtractRandomItem(2);
                    break;
            }
        }

        void Start()
        {
            collectablesManager.Reset();
        }

    }
}
