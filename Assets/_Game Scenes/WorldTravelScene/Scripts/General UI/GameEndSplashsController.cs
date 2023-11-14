using System;
using _MeteorShower.Scripts;
using UnityEngine;

namespace _WorldTravelScene.Scripts.General_UI
{
    public class GameEndSplashsController : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private ActionsTogglerSO actionsToggler;
        [Space]
        [SerializeField] private GameFinishedScreenController gameFinishedScreenController;
        [SerializeField] private GameOverScreenController gameOverScreenController;
        
        private void OnEnable()
        {
            gameManager.gameWon += ShowGameFinishedScreen;
            gameManager.gameLost += ShowGameLostScreen;
        }

        private void OnDisable()
        {
            gameManager.gameWon -= ShowGameFinishedScreen;
            gameManager.gameLost -= ShowGameLostScreen;
        }

        private void ShowGameFinishedScreen()
        {
            actionsToggler.MovementToggle(false);
            actionsToggler.EnableCursor(true);
            
            gameFinishedScreenController.ShowGameFinishedScreen();
        }

        private void ShowGameLostScreen()
        {
            actionsToggler.MovementToggle(false);
            actionsToggler.EnableCursor(true);
            
            gameOverScreenController.ShowGameOverScreen();
        }
    }
}
