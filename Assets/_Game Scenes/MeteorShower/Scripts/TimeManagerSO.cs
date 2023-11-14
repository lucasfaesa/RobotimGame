using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _MeteorShower.Scripts
{
    [CreateAssetMenu(fileName = "TimeManager", menuName = "ScriptableObjects/MeteorShowerScene/TimeManager")]
    public class TimeManagerSO : ScriptableObject
    {
        public int LevelTimer { get; set; } = 25;
        public int CurrentTime { get; set; }
        
        public event Action startedTimer;
        public event Action timeEnded;
    
        public void StartTimer()
        {
            startedTimer?.Invoke();
        }

        public void TimeEnded()
        {
            timeEnded?.Invoke();
        }
        
        
    
    }
}
