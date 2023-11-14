using System;
using UnityEngine;

namespace _WorldTravelScene.ScriptableObjects.Planet
{
    [CreateAssetMenu(fileName = "LandSeaCollisionManagerSO", menuName = "ScriptableObjects/WorldTravelScene/LandSeaCollisionManagerSO")]
    public class LandSeaCollisionManagerSO : ScriptableObject
    {
        public event Action<bool> collidedWithLand;

        public void CollidedOnLand(bool status) { collidedWithLand?.Invoke(status); }
    }
}
