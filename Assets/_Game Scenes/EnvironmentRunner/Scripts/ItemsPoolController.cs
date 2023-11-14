using System.Collections.Generic;
using _MeteorShower.Scripts;
using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class ItemsPoolController : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManager;
        [Space]
        [SerializeField] private ItemObject birdPrefab;
        [SerializeField] private ItemObject waterPrefab;
        [SerializeField] private ItemObject boltPrefab;
        [SerializeField] private ItemObject fruitsPrefab;
        [Space]
        [SerializeField] private List<ItemObject> birdPool;
        [SerializeField] private List<ItemObject> waterPool;
        [SerializeField] private List<ItemObject> boltPool;
        [SerializeField] private List<ItemObject> fruitsPool;
        
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
            List<ItemObject> allObstacles = new(birdPool);
            allObstacles.AddRange(waterPool);
            allObstacles.AddRange(boltPool);
            allObstacles.AddRange(fruitsPool);
            
            foreach (var itemObject in allObstacles)
            {
                if(itemObject.ReadyToBeRecycled) continue;
                
                itemObject.VanishObject();
            }
        }
        
        public ItemObject RequestItem(RoadItemSpawner.ItemType type)
        {
            return GetDesiredObstacle(type);
        }

        private ItemObject GetDesiredObstacle(RoadItemSpawner.ItemType type)
        {
            List<ItemObject> itemPool = null;
            ItemObject itemPrefab = null;

            switch (type)
            {
                case RoadItemSpawner.ItemType.Bird:
                    itemPool = birdPool;
                    itemPrefab = birdPrefab;
                    break;
                case RoadItemSpawner.ItemType.Bolt:
                    itemPool = boltPool;
                    itemPrefab = boltPrefab;
                    break;
                case RoadItemSpawner.ItemType.Fruit:
                    itemPool = fruitsPool;
                    itemPrefab = fruitsPrefab;
                    break;
                case RoadItemSpawner.ItemType.Water:
                    itemPool = waterPool;
                    itemPrefab = waterPrefab;
                    break;
                case RoadItemSpawner.ItemType.NoItem:
                    return null;
            }

            ItemObject item = itemPool.Find(x => x.ReadyToBeRecycled);
            if (item != null)
            {
                item = InstantiateMoreObstacles(itemPrefab, ref itemPool);
            }

            return item;
        }
        
        private ItemObject InstantiateMoreObstacles(ItemObject gameObject, ref List<ItemObject> listToAdd)
        {
            ItemObject newItem = Instantiate(gameObject, this.transform);
            newItem.SetParentItemPoolController(this);    
            newItem.DeactivateItem();
            
            listToAdd.Add(newItem);
            
            return newItem;
        }
    }
}
