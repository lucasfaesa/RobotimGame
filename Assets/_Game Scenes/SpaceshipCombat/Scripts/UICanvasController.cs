using System;
using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using _SpaceshipCombat.Scripts;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.SpaceshipCombat.Scripts
{
    public class UICanvasController : MonoBehaviour
    {
        [Header("SO")] [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private ActionsTogglerSO actionsToggler;
        [Header("Splashs")] [SerializeField] private GameFinishedScreenController gameWonCanvas;
        [SerializeField] private GameOverScreenController gameLostCanvas;
        [Header("Tutorial")] [SerializeField] private GameObject tutorialObject;
        [Space] [SerializeField] private PlayerSpaceshipTriggerInteractor playerSpaceshipTriggerInteractor;
        [Space] [SerializeField] private GameObject interactDisplayObject;

        private void Start()
        {
            tutorialObject.SetActive(true);
        }

        private void OnEnable()
        {
            playerSpaceshipTriggerInteractor.playerCollidingWithDesk += ToggleInteractDisplay;
            gameManager.gameLost += ShowGameLostScreen;
            gameManager.gameWon += ShowGameWonScreen;
        }

        private void OnDisable()
        {
            playerSpaceshipTriggerInteractor.playerCollidingWithDesk -= ToggleInteractDisplay;
            gameManager.gameLost -= ShowGameLostScreen;
            gameManager.gameWon -= ShowGameWonScreen;
        }

        private void ToggleInteractDisplay(bool show, DeskAnswerHolder n)
        {
            interactDisplayObject.SetActive(show);
        }

        public void HideTutorial()
        {
            tutorialObject.SetActive(false);
            actionsToggler.EnableCursor(false);
            actionsToggler.MovementToggle(true);
        }

        private void ShowGameWonScreen()
        {
            actionsToggler.EnableCursor(true);
            actionsToggler.MovementToggle(false);

            gameWonCanvas.ShowGameFinishedScreen();
        }

        private void ShowGameLostScreen()
        {
            actionsToggler.EnableCursor(true);
            actionsToggler.MovementToggle(false);

            gameLostCanvas.ShowGameOverScreen();
        }
    }
}
