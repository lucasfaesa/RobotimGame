using System;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    [CreateAssetMenu(fileName = "NpcColliderSO", menuName = "ScriptableObjects/SchoolRoomScene/NpcColliderSO")]
    public class NpcColliderSO : ScriptableObject
    {
        public event Action EnteredCollision;
        public event Action LeftCollision;

        public void CollisionEnter() { EnteredCollision?.Invoke(); }

        public void CollisionExit() { LeftCollision?.Invoke(); }
    }
}
