using System.Collections.Generic;
using System.Linq;
using _MeteorShower.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class ObstaclePoolController : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private ObstacleObject factoryPrefab;
        [SerializeField] private ObstacleObject oilSpillPrefab;
        [SerializeField] private ObstacleObject trashPrefab;
        [Space]
        [SerializeField] private List<ObstacleObject> factoryPool;
        [SerializeField] private List<ObstacleObject> oilSpillPool;
        [SerializeField] private List<ObstacleObject> trashPool;

        private void OnEnable()
        {
            gameManager.gameGoingToEnd += GameGoingToEnd;
        }

        private void OnDisable()
        {
            gameManager.gameGoingToEnd += GameGoingToEnd;
        }

        private void GameGoingToEnd()
        {
            List<ObstacleObject> allObstacles = new(factoryPool);
            allObstacles.AddRange(oilSpillPool);
            allObstacles.AddRange(trashPool);
            
            foreach (var obstacleObject in allObstacles)
            {
                if(obstacleObject.ReadyToBeRecycled) continue;
                
                obstacleObject.VanishObstacle();
            }
        }

        public ObstacleObject RequestObstacle(RoadObstacleSpawner.ObstacleType type)
        {
            return GetDesiredObstacle(type);
        }

        private ObstacleObject GetDesiredObstacle(RoadObstacleSpawner.ObstacleType type)
        {
            List<ObstacleObject> obstaclePool = null;
            ObstacleObject obstaclePrefab = null;

            switch (type)
            {
                case RoadObstacleSpawner.ObstacleType.OilSpillObstacle:
                    obstaclePool = oilSpillPool;
                    obstaclePrefab = oilSpillPrefab;
                    break;
                case RoadObstacleSpawner.ObstacleType.TrashCanObstacle:
                    obstaclePool = trashPool;
                    obstaclePrefab = trashPrefab;
                    break;
                case RoadObstacleSpawner.ObstacleType.FactoryObstacle:
                    obstaclePool = factoryPool;
                    obstaclePrefab = factoryPrefab;
                    break;
                case RoadObstacleSpawner.ObstacleType.NoObstacle:
                    return null;
            }

            ObstacleObject obstacle = obstaclePool.Find(x => x.ReadyToBeRecycled);
            if (obstacle != null)
            {
                obstacle = InstantiateMoreObstacles(obstaclePrefab, ref obstaclePool);
            }

            return obstacle;
        }
        
        private ObstacleObject InstantiateMoreObstacles(ObstacleObject gameObject, ref List<ObstacleObject> listToAdd)
        {
            ObstacleObject newObstacle = Instantiate(gameObject, this.transform);
            newObstacle.SetParentObstaclePoolController(this);    
            newObstacle.DeactivateObstacle();
            
            listToAdd.Add(newObstacle);
            
            return newObstacle;
        }
    }
}
