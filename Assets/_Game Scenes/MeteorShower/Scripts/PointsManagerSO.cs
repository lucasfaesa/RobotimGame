using System;
using UnityEngine;

namespace _MeteorShower.Scripts
{
    [CreateAssetMenu(fileName = "PointsManager", menuName = "ScriptableObjects/MeteorShowerScene/PointsManager")]
    public class PointsManagerSO : ScriptableObject
    {
        [field: SerializeField] public int CurrentScore { get; private set; }

        private int penaltyPoints = 4;
        public event Action<int> PointsUpdated;
    
        public void AddPoints(int points)
        {
            CurrentScore += points;
            Debug.Log("Current Points: " + CurrentScore);
            UpdateScore();
        }

        public void AddPointsWithoutUpdatingScore(int points)
        {
            CurrentScore += points;
        }

        public void UpdateScore()
        {
            PointsUpdated?.Invoke(CurrentScore);
        }

        public int GetCurrentScore(bool canBeNegative)
        {
            if (canBeNegative)
                return CurrentScore;
            else
            {
                if (CurrentScore < 0)
                    return CurrentScore = 0;
                else
                    return CurrentScore;
            }
        }
        
        public void RemovePoints()
        {
            CurrentScore -= penaltyPoints;
            if (CurrentScore < 0)
                CurrentScore = 0;

            UpdateScore();
        }
    
        public void RemovePoints(int points, bool canBeNegative)
        {
            CurrentScore -= points;
            
            if (!canBeNegative)
            {
                if (CurrentScore < 0)
                    CurrentScore = 0;
            }
            
            Debug.Log("Current Points: " + CurrentScore);
            UpdateScore();
        }

        public void RemovePointsButHasMinimumScore(int points, int minimumScore)
        {
            CurrentScore -= points;
            
            if (CurrentScore < minimumScore)
                CurrentScore = minimumScore;
            
            UpdateScore();
        }
    
        public void Reset()
        {
            CurrentScore = 0;
        }

        private void OnDisable()
        {
            Reset();
        }
    
    }
}
