using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game_Scenes.EnvironmentRunner.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class BirdFlocksController : MonoBehaviour
{
    [SerializeField] private CollectablesManagerSO collectablesManager;
    [SerializeField] private List<BirdFlyingAnimation> birds;

    private void OnEnable()
    {
        collectablesManager.itemCollectedUpdated += UpdateBirdFlocks;
    }

    private void OnDisable()
    {
        collectablesManager.itemCollectedUpdated -= UpdateBirdFlocks;
    }

    private void UpdateBirdFlocks(RoadItemSpawner.ItemType itemType, int quantity)
    {
        if (itemType != RoadItemSpawner.ItemType.Bird) return;

        int birdsToSpawn = quantity / 2;

        // Ensure birdsToSpawn is within the valid range
        birdsToSpawn = Mathf.Clamp(birdsToSpawn, 0, birds.Count);

        for (int i = 0; i < birdsToSpawn; i++)
        {
            birds[i].gameObject.SetActive(true);
            birds[i].ShowOnScreen();
        }

        // Hide any birds that exceed the desired quantity
        for (int i = birds.Count - 1; i >= birdsToSpawn; i--)
        {
            birds[i].HideOffscreen();
        }
    }
}
