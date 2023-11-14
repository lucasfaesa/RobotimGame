using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollisionManager", menuName = "ScriptableObjects/CollisionManager/CollisionManager")]
public class CollisionManagerSO : ScriptableObject
{
    public event Action<bool> collisionWithMissionBoard;
    public event Action<bool> collisionWithSkinChanger;

    public void MissionBoardCollision(bool status)
    {
        collisionWithMissionBoard?.Invoke(status);
    }

    public void SkinChangerCollision(bool status)
    {
        collisionWithSkinChanger?.Invoke(status);
    }
}
