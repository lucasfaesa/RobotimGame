using System.Collections;
using System.Collections.Generic;
using _Game_Scenes.EnvironmentRunner.Scripts;
using DG.Tweening;
using UnityEngine;

public class CameraShakeOnObstacleCollision : MonoBehaviour
{
    [SerializeField] private ObstacleEventChannelSO obstacleEventChannel;
    [SerializeField] private Camera camera;
    
    [SerializeField] private float strength = 1f;
    [SerializeField] private float duration = 0.3f;

    private void OnEnable()
    {
        obstacleEventChannel.obstacleHitPlayer += DoCameraShake;
    }

    private void OnDisable()
    {
        obstacleEventChannel.obstacleHitPlayer -= DoCameraShake;
    }

    private void DoCameraShake(RoadObstacleSpawner.ObstacleType _)
    {
        camera.DOShakePosition(duration, strength);
    }
    
    [ContextMenu("Do shake")]
    private void DoShake()
    {
        DoCameraShake(RoadObstacleSpawner.ObstacleType.NoObstacle);
    }
}
