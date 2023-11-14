using System;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    [CreateAssetMenu(fileName = "ResearchTableColliderData", menuName = "ScriptableObjects/SchoolRoomScene/ResearchTableColliderData")]
    public class ResearchTableColliderSO : ScriptableObject
    {
        public event Action<ResearchTableSO> EnteredCollision;
        public event Action<ResearchTableSO> LeftCollision;

        public void CollisionEnter(ResearchTableSO table) { EnteredCollision?.Invoke(table); }

        public void CollisionExit(ResearchTableSO table) { LeftCollision?.Invoke(table); }
    }
}
