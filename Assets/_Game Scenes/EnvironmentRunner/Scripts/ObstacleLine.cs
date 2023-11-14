using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class ObstacleLine : MonoBehaviour
    {
        [field:SerializeField] public List<Transform> Spots { get; set; }

        private List<ObstacleObject> currentObstaclesObjectsOnThisLine = new();
        
        public void SetObstacleOnSpot(ObstacleObject obstacleObject, int spotIndex)
        {
            currentObstaclesObjectsOnThisLine.Add(obstacleObject);
            obstacleObject.ActivateObstacle(Spots[spotIndex].transform);
        }
        
        public void ClearLine()
        {
            if (currentObstaclesObjectsOnThisLine.Count == 0) return;
            
            foreach (var obstacleObject in currentObstaclesObjectsOnThisLine)
            {
                if(obstacleObject != null)
                    obstacleObject.DeactivateObstacle();
            }

            currentObstaclesObjectsOnThisLine = new();
        }
    }
}
