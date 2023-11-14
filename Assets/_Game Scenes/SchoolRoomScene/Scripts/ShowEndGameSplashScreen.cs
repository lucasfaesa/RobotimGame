using System;
using _Lobby._LevelSelector.Scripts;
using _MeteorShower.Scripts;
using _Village.Scripts;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class ShowEndGameSplashScreen : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private GameFinishedScreenController gameFinishedScreenController;

        private void OnEnable() { gameManager.gameEnded += ShowEndGameScreen; }

        private void OnDisable() { gameManager.gameEnded -= ShowEndGameScreen;}

        private void ShowEndGameScreen()
        {

            gameFinishedScreenController.ShowGameFinishedScreen();
        }
    }
}
