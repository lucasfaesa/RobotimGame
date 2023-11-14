using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class RoadTreesRandomizer : MonoBehaviour
    {
        [SerializeField] private CollectablesManagerSO collectablesManagerSo;
        [SerializeField] private ParallaxEventChannelSO roadParallaxEventChannel;
        [Space]
        [SerializeField] private List<Transform> leftSideTreeSpots;
        [SerializeField] private List<Transform> rightSideTreeSpots;
        [Space]
        [SerializeField] private List<Transform> deadTreesPool;
        [SerializeField] private List<Transform> goodTreesPool;

        private List<bool> leftSideTreeOccupiedSpots = new();
        private List<bool> rightSideTreeOccupiedSpots = new();

        private int birdsCollected;
        
        private void OnEnable()
        {
            SetTreesPositions();
            collectablesManagerSo.itemCollectedUpdated +=UpdateBirdsCollected;
            roadParallaxEventChannel.gotParallaxed += SetTreesPositions;
        }

        private void OnDisable()
        {
            collectablesManagerSo.itemCollectedUpdated -= UpdateBirdsCollected;
            roadParallaxEventChannel.gotParallaxed -= SetTreesPositions;
        }

        private void UpdateBirdsCollected(RoadItemSpawner.ItemType itemType, int quantity)
        {
            if (itemType == RoadItemSpawner.ItemType.Bird)
                birdsCollected = quantity;
        }

        public void SetTreesPositions()
        {
            DeactivateAllTrees();
            
            leftSideTreeOccupiedSpots = GetOccupiedSpots(leftSideTreeSpots);
            rightSideTreeOccupiedSpots = GetOccupiedSpots(rightSideTreeSpots);
            
            SpawnTrees(leftSideTreeOccupiedSpots, leftSideTreeSpots);
            SpawnTrees(rightSideTreeOccupiedSpots, rightSideTreeSpots);
        }

        private void DeactivateAllTrees()
        {
            deadTreesPool.ForEach(x=>x.gameObject.SetActive(false));
            goodTreesPool.ForEach(x=>x.gameObject.SetActive(false));
        }

        private List<bool> GetOccupiedSpots(List<Transform> treeSpots)
        {
            List<bool> occupiedSpots = new List<bool>();

            foreach (var spot in treeSpots)
            {
                bool randomTreeOnSpotChance = Random.Range(0, 2) == 1;
                occupiedSpots.Add(randomTreeOnSpotChance);
            }

            return occupiedSpots;
        }

        private void SpawnTrees(List<bool> occupiedSpots, List<Transform> treeSpots)
        {
            for (int i = 0; i < treeSpots.Count; i++)
            {
                if(!occupiedSpots[i])
                    continue;
                
                int deadTreeChance = Random.Range(1, 11) - birdsCollected;
                int liveTreeChance = Random.Range(0, birdsCollected);

                Transform randomTree = GetRandomTree(liveTreeChance > deadTreeChance);
                
                randomTree.transform.SetParent(treeSpots[i].transform);
                randomTree.transform.localPosition = Vector3.zero;
                randomTree.transform.localRotation = Quaternion.Euler(randomTree.transform.localRotation.x,
                                                                        Random.Range(0f, 270f),
                                                                            randomTree.transform.localRotation.z);
                randomTree.gameObject.SetActive(true);
            }
        }

        private Transform GetRandomTree(bool liveTree)
        {
            while (true)
            {
                int randomIndex = 0;
                
                if (!liveTree)
                {
                    randomIndex = Random.Range(0, deadTreesPool.Count);
                    
                    if(deadTreesPool[randomIndex].gameObject.activeSelf)
                        continue;

                    return deadTreesPool[randomIndex];
                }
                else
                {
                    randomIndex = Random.Range(0, goodTreesPool.Count);
                    
                    if(goodTreesPool[randomIndex].gameObject.activeSelf)
                        continue;

                    return goodTreesPool[randomIndex];
                }
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(RoadTreesRandomizer))] // Replace "MyScript" with the name of the script you want to customize.
    public class CustomInspector3 : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector GUI first
            DrawDefaultInspector();

            // Get the target script instance
            RoadTreesRandomizer myScript = (RoadTreesRandomizer)target;

            // Add a custom button
            if (GUILayout.Button("Custom Button"))
            {
                // Call a method in the target script when the button is clicked
                myScript.SetTreesPositions();
            }
        }
    }
    #endif
}
