using System;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class WorldBeautifier : MonoBehaviour
    {
        [SerializeField] private CollectablesManagerSO collectablesManager;
        [Space]
        [SerializeField] private Renderer waterRenderer;
        [SerializeField] private Renderer sideGround;
        [SerializeField] private Light directionalLight;
        [Space]
        [SerializeField] private Color32 badWaterColor;
        [SerializeField] private Color32 goodWaterColor;
        [Space]
        [SerializeField] private Color32 badGrassColor;
        [SerializeField] private Color32 goodGrassColor;
        [Space]
        [SerializeField] private float badLightIntensity;
        [SerializeField] private float goodLightIntensity;
        [SerializeField] private float badFogIntensity = 0.02f;
        [SerializeField] private float goodFogIntensity = 0f;
        
        
        private int totalBoltToCollect;
        private int totalWaterToCollect;
        private int totalFruitsToCollect;
        
        private void OnEnable()
        {
            collectablesManager.itemCollectedUpdated += UpdateWorld;
        }

        private void OnDisable()
        {
            collectablesManager.itemCollectedUpdated -= UpdateWorld;
        }

        
        // Start is called before the first frame update
        void Start()
        {
            Reset();
        }

        private void UpdateWorld(RoadItemSpawner.ItemType itemType, int quantity)
        {
            switch (itemType)
            {
                case RoadItemSpawner.ItemType.Water:
                    waterRenderer.sharedMaterial.DOColor(GetColorProportionBasedOnCollectables(quantity,totalWaterToCollect, badWaterColor,goodWaterColor),"_BaseColor", 1f);
                    break;
                case RoadItemSpawner.ItemType.Bolt:
                    directionalLight.DOIntensity(GetValueProportionBasedOnCollectables(quantity,totalBoltToCollect, badLightIntensity, goodLightIntensity), 1f);
                    DOTween.To(x => RenderSettings.fogDensity = x, RenderSettings.fogDensity, GetValueProportionBasedOnCollectables(quantity,totalBoltToCollect, badFogIntensity,goodFogIntensity), 1f);
                    break;
                case RoadItemSpawner.ItemType.Fruit:
                    sideGround.sharedMaterial.DOColor(GetColorProportionBasedOnCollectables(quantity,totalFruitsToCollect, badGrassColor,goodGrassColor),"_BaseColor", 1f);
                    break;
                
            }
        }

        private Color32 GetColorProportionBasedOnCollectables(int quantity, int quantityToCollect, Color32 badColor, Color32 goodColor)
        {
            double proportion = (double)quantity / quantityToCollect;
            
            int red = (int)(badColor.r + proportion * (goodColor.r - badColor.r));
            int green = (int)(badColor.g + proportion * (goodColor.g - badColor.g));
            int blue = (int)(badColor.b + proportion * (goodColor.b - badColor.b));
            
            return new Color32((byte)red, (byte)green, (byte)blue,(byte)255f);
        }

        private float GetValueProportionBasedOnCollectables(int quantity, int quantityToCollect, float badValue, float goodValue)
        {
            double proportion = (double)quantity / quantityToCollect;

            return (float)(badValue + proportion * (goodValue - badValue));
        }
        
        private void Reset()
        {
            waterRenderer.sharedMaterial.SetColor("_BaseColor", badWaterColor);
            sideGround.sharedMaterial.SetColor("_BaseColor", badGrassColor);
            directionalLight.intensity = badLightIntensity;

            totalFruitsToCollect = collectablesManager.GetTotalToCollect(RoadItemSpawner.ItemType.Fruit);
            totalBoltToCollect = collectablesManager.GetTotalToCollect(RoadItemSpawner.ItemType.Bolt);
            totalWaterToCollect = collectablesManager.GetTotalToCollect(RoadItemSpawner.ItemType.Water);
        }
    }
}
