using System;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _WorldTravelScene.Scripts.Timer
{
    [CreateAssetMenu(fileName = "TimerScoreData", menuName = "ScriptableObjects/WorldTravelScene/TimerScoreData")]
    public class TimerScoreDataSO : ScriptableObject
    {
        [SerializeField] private PointsManagerSO pointsManager;
        
        public List<float> Timers { get; set; } = new();
        public List<int> Points { get; set; } = new();

        public event Action<float, int> timerAndScoreAdded;
        
        public void AddTimer(float time)
        {
            Timers.Add(time);

            int points = time switch
            {
                < 60 => Random.Range(15, 19),
                < 90 => Random.Range(13, 16),
                < 120 => Random.Range(10, 14),
                < 180 => Random.Range(8, 11),
                < 240 => Random.Range(5, 9),
                _ => Random.Range(1, 6),
            };
            
            Points.Add(points);
            
            pointsManager.AddPoints(points);
            
            timerAndScoreAdded?.Invoke(time, points);
        }
        
        public void Reset()
        {
            Timers = new();
            Points = new();
        }

    }
}
