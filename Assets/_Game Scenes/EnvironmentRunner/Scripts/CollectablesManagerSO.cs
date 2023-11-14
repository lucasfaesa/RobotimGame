using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    [CreateAssetMenu(fileName = "CollectablesManager", menuName = "ScriptableObjects/EnvironmentRunner/CollectablesManager")]
    public class CollectablesManagerSO : ScriptableObject
    {
        [SerializeField] private int totalBirdsToCollect = 10;
        [SerializeField] private int totalWaterToCollect = 10;
        [SerializeField] private int totalBoltsToCollect = 10;
        [SerializeField] private int totalFruitsToCollect = 10;

        private int _birdsCollected;
        private int _waterCollected;
        private int _boltsCollected;
        private int _fruitsCollected;

        public event Action allItemsCollected;

        private bool collectedAllItems;
        
        private int BirdsCollected
        {
            get => _birdsCollected;
            set => _birdsCollected = Mathf.Clamp(value,0, totalBirdsToCollect);
        }

        private int WaterCollected
        {
            get => _waterCollected;
            set => _waterCollected = Mathf.Clamp(value,0, totalWaterToCollect);
        }

        private int BoltsCollected
        {
            get => _boltsCollected;
            set => _boltsCollected = Mathf.Clamp(value,0, totalBoltsToCollect);
        }

        private int FruitsCollected
        {
            get => _fruitsCollected;
            set => _fruitsCollected = Mathf.Clamp(value,0, totalFruitsToCollect);
        }
        
        public event Action<RoadItemSpawner.ItemType, int> itemCollectedUpdated;


        public void CollectedItem(RoadItemSpawner.ItemType type)
        {
            switch (type)
            {
                case RoadItemSpawner.ItemType.Bird:
                    BirdsCollected++;
                    itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Bird, BirdsCollected);
                    break;
                case RoadItemSpawner.ItemType.Bolt:
                    BoltsCollected++;
                    itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Bolt, BoltsCollected);
                    break;
                case RoadItemSpawner.ItemType.Fruit:
                    FruitsCollected++;
                    itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Fruit, FruitsCollected);
                    break;
                case RoadItemSpawner.ItemType.Water:
                    WaterCollected++;
                    itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Water, WaterCollected);
                    break;
            }

            if (_birdsCollected == totalBirdsToCollect && _waterCollected == totalWaterToCollect
                                                       && _fruitsCollected == totalFruitsToCollect
                                                       && _boltsCollected == totalBoltsToCollect)
            {
                if (collectedAllItems) return;
                
                allItemsCollected?.Invoke();
            }
        }

        public void SubtractFromCollectedItems(RoadItemSpawner.ItemType type, int quantity)
        {
            switch (type)
            {
                case RoadItemSpawner.ItemType.Bird:
                    BirdsCollected -= quantity;
                    itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Bird, BirdsCollected);
                    break;
                case RoadItemSpawner.ItemType.Bolt:
                    BoltsCollected -= quantity;
                    itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Bolt, BoltsCollected);
                    break;
                case RoadItemSpawner.ItemType.Fruit:
                    FruitsCollected-= quantity;
                    itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Fruit, FruitsCollected);
                    break;
                case RoadItemSpawner.ItemType.Water:
                    WaterCollected-= quantity;
                    itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Water, WaterCollected);
                    break;
            }
        }
        
        public void SubtractRandomItem(int quantity)
        {
            var randomNumber = Random.Range(0, 4);

            switch (randomNumber)
            {
                case 0:
                    BirdsCollected -= quantity;
                    break;
                case 1:
                    BoltsCollected -= quantity;
                    break;
                case 2:
                    FruitsCollected -= quantity;
                    break;
                case 3:
                    WaterCollected -= quantity;
                    break;
            }
            
            itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Bird, BirdsCollected);
            itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Bolt, BoltsCollected);
            itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Fruit, FruitsCollected);
            itemCollectedUpdated?.Invoke(RoadItemSpawner.ItemType.Water, WaterCollected);
        }

        public int GetTotalToCollect(RoadItemSpawner.ItemType type)
        {
            switch (type)
            {
                case RoadItemSpawner.ItemType.Bird:
                    return totalBirdsToCollect;
                    break;
                case RoadItemSpawner.ItemType.Fruit:
                    return totalFruitsToCollect;
                    break;
                case RoadItemSpawner.ItemType.Water:
                    return totalWaterToCollect;
                    break;
                case RoadItemSpawner.ItemType.Bolt:
                    return totalBoltsToCollect;
                    break;
                default:
                    return 10;
                
            }
        }
        
        public void Reset()
        {
            BirdsCollected = 0;
            WaterCollected = 0;
            FruitsCollected = 0;
            BoltsCollected = 0;
            collectedAllItems = false;
        }
    }
}