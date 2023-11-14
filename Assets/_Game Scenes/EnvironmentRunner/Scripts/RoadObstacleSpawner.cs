using System;
using System.Collections.Generic;
using System.Linq;
using _Lobby._LevelSelector.Scripts;
using _MeteorShower.Scripts;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class RoadObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
        [SerializeField] private ParallaxEventChannelSO parallaxEventChannel;
        [Space]
        [SerializeField] private ObstaclePoolController obstaclePoolController;
        [Space]
        [SerializeField] private List<ObstacleLine> obstacleLines = new();

        public enum ObstacleType { NotDefined, NoObstacle, OilSpillObstacle, TrashCanObstacle, FactoryObstacle }

        public enum EasyDifficultyObstacleChance { NoObstacleChance = 72, OilSpillChance = 27, TrashCanChance = 1, 
                                                    FactoryChance = 0}
        public enum NormalDifficultyObstacleChance { NoObstacleChance = 70, OilSpillChance = 20, TrashCanChance = 9, 
                                                        FactoryChance = 1}
        public enum HardDifficultyObstacleChance { NoObstacleChance = 40, OilSpillChance = 29, TrashCanChance = 24, 
                                                        FactoryChance = 7}

        private bool stopSpawningObstacles;

        private void OnEnable()
        {
            gameManager.gameGoingToEnd += GameGoingToEnd;
            parallaxEventChannel.gotParallaxed += SetObstaclesPositions;
        }

        private void OnDisable()
        {
            gameManager.gameGoingToEnd -= GameGoingToEnd;
            parallaxEventChannel.gotParallaxed += SetObstaclesPositions;
        }

        private void GameGoingToEnd()
        {
            stopSpawningObstacles = true;
        }
        
        public void SetObstaclesPositions()
        {
            if (stopSpawningObstacles) return;
            
            foreach (var obstacleLine in obstacleLines)
            {
                obstacleLine.ClearLine();
            }

            foreach (var obstacleLine in obstacleLines)
            {
                switch (levelSelectedManager.CurrentLevelInfo.level.Difficulty)
                {
                    case 1:
                        GenerateEasyRoad(obstacleLine);
                        break;
                    case 2 or 3:
                        GenerateMediumRoad(obstacleLine);
                        break;
                    case > 3:
                        GenerateHardRoad(obstacleLine);
                        break;
                }
            }
        }

        private void GenerateEasyRoad(ObstacleLine obstacleLine)
        {
            GetObstaclesChancesOnSpots((int)EasyDifficultyObstacleChance.NoObstacleChance, 
                                        (int)EasyDifficultyObstacleChance.OilSpillChance,
                                            (int)EasyDifficultyObstacleChance.TrashCanChance,
                                                (int)EasyDifficultyObstacleChance.FactoryChance, 
                                                    out List<ObstacleType> chosenObstaclesList);
            
            GenerateObstaclesLine(obstacleLine, chosenObstaclesList);
        }
        
        private void GenerateMediumRoad(ObstacleLine obstacleLine)
        {
            GetObstaclesChancesOnSpots((int)NormalDifficultyObstacleChance.NoObstacleChance, 
                (int)NormalDifficultyObstacleChance.OilSpillChance,
                (int)NormalDifficultyObstacleChance.TrashCanChance,
                (int)NormalDifficultyObstacleChance.FactoryChance, 
                out List<ObstacleType> chosenObstaclesList);
            
            GenerateObstaclesLine(obstacleLine, chosenObstaclesList);
        }
        
        private void GenerateHardRoad(ObstacleLine obstacleLine)
        {
            GetObstaclesChancesOnSpots((int)HardDifficultyObstacleChance.NoObstacleChance, 
                (int)HardDifficultyObstacleChance.OilSpillChance,
                (int)HardDifficultyObstacleChance.TrashCanChance,
                (int)HardDifficultyObstacleChance.FactoryChance, 
                out List<ObstacleType> chosenObstaclesList);
            
            GenerateObstaclesLine(obstacleLine, chosenObstaclesList);
        }

        private void GetObstaclesChancesOnSpots(int noObstacleChance, int oilSpillChance, int trashCanChance, 
                                                int factoryChance, out List<ObstacleType> chosenObstaclesList)
        {
            int maxObstaclesPerType = 2;
            int numberOfSpots = 3;
            List<ObstacleType> chosenObstaclesForSpots = new();
            
            while (chosenObstaclesForSpots.Count < numberOfSpots)
            {
                ObstacleType obstacleChosenByRandomNumber = ObstacleType.NotDefined;
                
                float randomValue = Random.Range(1f, 100f);

                //Debug.Log($"number: {randomValue}");
                
                if (randomValue <= factoryChance && 
                    chosenObstaclesForSpots.Count(x=>x == ObstacleType.FactoryObstacle) < maxObstaclesPerType)
                {
                    obstacleChosenByRandomNumber = ObstacleType.FactoryObstacle;
                }
                else if (randomValue <= trashCanChance &&
                         chosenObstaclesForSpots.Count(x=>x == ObstacleType.TrashCanObstacle) < maxObstaclesPerType)
                {
                    obstacleChosenByRandomNumber = ObstacleType.TrashCanObstacle;
                }
                else if (randomValue <= oilSpillChance &&
                         chosenObstaclesForSpots.Count(x=>x == ObstacleType.OilSpillObstacle) < maxObstaclesPerType)
                {
                    obstacleChosenByRandomNumber = ObstacleType.OilSpillObstacle;
                }
                else if(randomValue <= noObstacleChance &&
                        chosenObstaclesForSpots.Count(x=>x == ObstacleType.NoObstacle) < maxObstaclesPerType)
                {
                    obstacleChosenByRandomNumber = ObstacleType.NoObstacle;
                }
                
                if(obstacleChosenByRandomNumber != ObstacleType.NotDefined)
                    chosenObstaclesForSpots.Add(obstacleChosenByRandomNumber);
            }

            chosenObstaclesList = chosenObstaclesForSpots;
        }

        private void GenerateObstaclesLine(ObstacleLine obstacleLine, List<ObstacleType> chosenObstaclesList)
        {
            for (int i = 0; i < obstacleLine.Spots.Count; i++)
            {
                ObstacleObject obstacleObject = obstaclePoolController.RequestObstacle(chosenObstaclesList[i]);
                
                if (obstacleObject == null) //if null its because its a noObstacle
                    continue;
                
                obstacleLine.SetObstacleOnSpot(obstacleObject, i);
            }
        }
        
        [ContextMenu("Re-generate Track")]
        public void DoSomething()
        {
            
            
            SetObstaclesPositions();
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(RoadObstacleSpawner))] // Replace "MyScript" with the name of the script you want to customize.
    public class CustomInspector2 : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector GUI first
            DrawDefaultInspector();

            // Get the target script instance
            RoadObstacleSpawner myScript = (RoadObstacleSpawner)target;

            // Add a custom button
            if (GUILayout.Button("Custom Button"))
            {
                // Call a method in the target script when the button is clicked
                myScript.SetObstaclesPositions();
            }
        }
    }
    #endif
}

