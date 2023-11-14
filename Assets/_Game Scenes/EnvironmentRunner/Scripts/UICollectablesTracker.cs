using System;
using System.Net.Mime;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class UICollectablesTracker : MonoBehaviour
    {
        [SerializeField] private ObstacleEventChannelSO obstacleEventChannel;
        [SerializeField] private CollectablesManagerSO collectablesManager;
        [Space] 
        [SerializeField] private Image imageBackground;
        [SerializeField] private RoadItemSpawner.ItemType type;
        [SerializeField] private TextMeshProUGUI currentCollectedQuantityText;
        [SerializeField] private TextMeshProUGUI totalToCollectQuantityText;

        private Sequence collectedSequence;
        private Sequence deductionSequence;
        private int totalToCollect;


        
        private void OnEnable()
        {
            collectablesManager.itemCollectedUpdated += UpdateCurrentQuantityText;
            obstacleEventChannel.obstacleHitPlayer += BlinkRedOnCollectionDeduction;
        }

        private void OnDisable()
        {
            collectablesManager.itemCollectedUpdated -= UpdateCurrentQuantityText;
            obstacleEventChannel.obstacleHitPlayer -= BlinkRedOnCollectionDeduction;
        }
        
        void Start()
        {
            collectedSequence = DOTween.Sequence()
                .Append(this.transform.DOScale(1.25f, 0.15f).SetEase(Ease.InOutSine).SetLoops(4, LoopType.Yoyo)).Pause().SetAutoKill(false);

            deductionSequence = DOTween.Sequence()
                .Append(imageBackground.DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo)).Pause().SetAutoKill(false);
            
            totalToCollect = collectablesManager.GetTotalToCollect(type);
            totalToCollectQuantityText.text = totalToCollect.ToString();
        }

        private void UpdateCurrentQuantityText(RoadItemSpawner.ItemType itemType, int quantity)
        {
            if (itemType == type)
            {
                CheckIfAllCollected(quantity);
                
                currentCollectedQuantityText.text = quantity.ToString();
                collectedSequence.Pause();
                this.transform.localScale = Vector3.one;
                collectedSequence.Restart();
                
            }
        }

        private void CheckIfAllCollected(int collectedQuantity)
        {
            if (collectedQuantity == totalToCollect)
            {
                if (deductionSequence.IsPlaying())
                    deductionSequence.Pause();
                
                imageBackground.color = Color.green;
            }
            else
                imageBackground.color = Color.white;
        }

        private void BlinkRedOnCollectionDeduction(RoadObstacleSpawner.ObstacleType _)
        {
            deductionSequence.Pause();
            imageBackground.color = Color.white;
            deductionSequence.Restart();
        }
        
        
    }
}
