using System;
using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    [CreateAssetMenu(fileName = "ObstacleEventChannelSO", menuName = "ScriptableObjects/EnvironmentRunner/ObstacleEventChannelSO")]

    public class ObstacleEventChannelSO : ScriptableObject
    {
        public event Action<RoadObstacleSpawner.ObstacleType> obstacleHitPlayer;

        public void OnObstacleHitPlayer(RoadObstacleSpawner.ObstacleType type)
        {
            obstacleHitPlayer?.Invoke(type);
        }
    }
}
