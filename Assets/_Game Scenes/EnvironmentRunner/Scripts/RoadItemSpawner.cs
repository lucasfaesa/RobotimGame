using System;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class RoadItemSpawner : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private CollectablesManagerSO collectablesManager;
        [SerializeField] private ParallaxEventChannelSO parallaxEventChannel;
        [Space]
        [SerializeField] private ItemsPoolController itemPoolController;
        [Space]
        [SerializeField] private List<ItemSpot> itemSpots = new();

        public enum ItemType { NotDefined, NoItem, Bird, Water, Bolt, Fruit }

        public enum ItemSpawnChance { NoItemChance = 84, BirdChance = 88, WaterChance = 92, 
                                                            FruitChance = 96, BoltChance = 100}

        [Header("Debug")] 
        [Range(1, 4)] [SerializeField] private int levelDifficulty;

        private int totalBirdsToCollect;
        private int totalWaterToCollect;
        private int totalBoltToCollect;
        private int totalFruitToCollect;
        
        private bool allBirdsCollected;
        private bool allWaterCollected;
        private bool allFruitCollected;
        private bool allBoltCollected;

        private bool stopSpawningItems;
        private void OnEnable()
        {
            gameManager.gameGoingToEnd += GameGoingToEnd;
            collectablesManager.itemCollectedUpdated += UpdateCollectedQuantity;
            parallaxEventChannel.gotParallaxed += SetItemsPositions;
        }

        private void OnDisable()
        {
            gameManager.gameGoingToEnd += GameGoingToEnd;
            collectablesManager.itemCollectedUpdated -= UpdateCollectedQuantity;
            parallaxEventChannel.gotParallaxed += SetItemsPositions;
        }
        
        private void GameGoingToEnd()
        {
            stopSpawningItems = true;
        }

        private void UpdateCollectedQuantity(ItemType itemType, int quantity)
        {
            switch (itemType)
            {
                case ItemType.Bird:
                    allBirdsCollected = quantity == totalBirdsToCollect;
                    break;
                case ItemType.Bolt:
                    allBoltCollected = quantity == totalBoltToCollect;
                    break;
                case ItemType.Fruit:
                    allFruitCollected = quantity == totalFruitToCollect;
                    break;
                case ItemType.Water:
                    allWaterCollected = quantity == totalWaterToCollect;
                    break;
            }
        }

        private void Start()
        {
            totalBirdsToCollect = collectablesManager.GetTotalToCollect(ItemType.Bird);
            totalWaterToCollect = collectablesManager.GetTotalToCollect(ItemType.Water);
            totalBoltToCollect = collectablesManager.GetTotalToCollect(ItemType.Bolt);
            totalFruitToCollect = collectablesManager.GetTotalToCollect(ItemType.Fruit);
        }

        public void SetItemsPositions()
        {
            if (stopSpawningItems) return;
            
            foreach (var itemSpot in itemSpots)
            {
                itemSpot.Clear();
            }

            GenerateItemsOnRoad();
        }

        private void GenerateItemsOnRoad()
        {
            GetItemsChancesOnSpots((int)ItemSpawnChance.NoItemChance, 
                                    (int)ItemSpawnChance.BirdChance,
                                    (int)ItemSpawnChance.WaterChance,
                                    (int)ItemSpawnChance.BoltChance,
                                    (int)ItemSpawnChance.FruitChance,
                                    out List<ItemType> chosenItemsList);
            
            GenerateItemsPositions(chosenItemsList);
        }

        private void GetItemsChancesOnSpots(int noItemChance, int birdChance, int waterChance, 
                                                int boltChance, int fruitChance, out List<ItemType> chosenItemsList)
        {
            List<ItemType> chosenItemsForSpots = new();
            
            while (chosenItemsForSpots.Count < itemSpots.Count)
            {
                ItemType itemChosenByRandomNumber = ItemType.NotDefined;
                
                float randomValue = Random.Range(1f, 100f);

                //Debug.Log($"number: {randomValue}");
                
                switch (randomValue)
                {
                    case var n when (n <= noItemChance):
                        itemChosenByRandomNumber = ItemType.NoItem;
                        break;
                
                    case var n when (n <= birdChance):
                        itemChosenByRandomNumber = ItemType.Bird;
                        break;
                
                    case var n when (n <= waterChance):
                        itemChosenByRandomNumber = ItemType.Water;
                        break;
                
                    case var n when (n <= fruitChance):
                        itemChosenByRandomNumber = ItemType.Fruit;
                        break;
                
                    case var n when (n <= boltChance):
                        itemChosenByRandomNumber = ItemType.Bolt;
                        break;
                }
                
                /*if (randomValue <= fruitChance)
                {
                    itemChosenByRandomNumber = ItemType.Fruit;
                }
                
                else if (randomValue <= boltChance)
                {
                    itemChosenByRandomNumber = ItemType.Bolt;
                }
                else if (randomValue <= waterChance)
                {
                    itemChosenByRandomNumber = ItemType.Water;
                }
                else if (randomValue <= birdChance)
                {
                    itemChosenByRandomNumber = ItemType.Bird;
                }
                else if(randomValue <= noItemChance)
                {
                    itemChosenByRandomNumber = ItemType.NoItem;
                }*/
                
                if(itemChosenByRandomNumber != ItemType.NotDefined)
                    chosenItemsForSpots.Add(itemChosenByRandomNumber);
            }

            chosenItemsList = chosenItemsForSpots;
        }

        private void GenerateItemsPositions(List<ItemType> chosenItemsList)
        {
            for (int i = 0; i < itemSpots.Count; i++)
            {
                ItemObject itemObject = itemPoolController.RequestItem(chosenItemsList[i]);
                
                if (itemObject == null) //if null its because its a noItem
                    continue;
                
                itemSpots[i].SetItemOnSpot(itemObject, i);
            }
        }
        
        [ContextMenu("Re-generate Track")]
        public void DoSomething()
        {
            
            
            SetItemsPositions();
        }
    }
    #if UNITY_EDITOR
    [CustomEditor(typeof(RoadItemSpawner))] // Replace "MyScript" with the name of the script you want to customize.
    public class CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector GUI first
            DrawDefaultInspector();

            // Get the target script instance
            RoadItemSpawner myScript = (RoadItemSpawner)target;

            // Add a custom button
            if (GUILayout.Button("Custom Button"))
            {
                // Call a method in the target script when the button is clicked
                myScript.SetItemsPositions();
            }
        }
    }
    #endif
}
