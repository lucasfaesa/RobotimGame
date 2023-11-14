using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _MeteorShower.Scripts
{
    [CreateAssetMenu(fileName = "GameManager", menuName = "ScriptableObjects/MeteorShowerScene/GameManager")]
    public class GameManagerSO : ScriptableObject
    {
        public event Action sceneLoaded;
        public event Action preparingToStartGame;
        public event Action gameStarted;
        public event Action gameExited;
        public event Action gameGoingToEnd;
        public event Action gameEnded; 
        public event Action gameWon; //used only in levels that has a lose factor
        public event Action gameLost; //used only in levels that has a lose factor
        
        
        public void SceneLoaded()
        {
            sceneLoaded?.Invoke();
        }

        public void PreparingToStartGame()
        {
            preparingToStartGame?.Invoke();
        }

        public void GameStarted()
        {
            gameStarted?.Invoke();
        }

        public void GameExited()
        {
            gameExited?.Invoke();
        }

        public void GameGoingToEnd()
        {
            gameGoingToEnd?.Invoke();
        }
        
        public void GameEnded()
        {
            gameEnded?.Invoke();
        }

        public void GameWon()
        {
            gameWon?.Invoke();
        }

        public void GameLost()
        {
            gameLost?.Invoke();
        }
    }
}
