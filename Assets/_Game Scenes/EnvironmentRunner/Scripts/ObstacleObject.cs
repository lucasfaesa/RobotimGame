using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class ObstacleObject : MonoBehaviour
    {
        [SerializeField] private ObstacleEventChannelSO obstacleEventChannel;
        [SerializeField] private ObstaclePoolController parentObstaclePoolController;
        [SerializeField] private GameObject obstacleContent;
        [SerializeField] private List<BoxCollider> boxColliders;
        [SerializeField] private RoadObstacleSpawner.ObstacleType type;
        
        public bool ReadyToBeRecycled { get; private set; } = true;
        
        public void SetParentObstaclePoolController(ObstaclePoolController parent)
        {
            parentObstaclePoolController = parent;
        }

        public void ActivateObstacle(Transform parent)
        {
            obstacleContent.gameObject.SetActive(true);
            this.transform.SetParent(parent);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            boxColliders.ForEach(x=>x.enabled = true);
            ReadyToBeRecycled = false;
        }

        public void DeactivateObstacle()
        {
            obstacleContent.gameObject.SetActive(false);
            this.transform.SetParent(parentObstaclePoolController.transform);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            boxColliders.ForEach(x=>x.enabled = false);
            ReadyToBeRecycled = true;
        }

        public void VanishObstacle()
        {
            boxColliders.ForEach(x=>x.enabled = false);
            Sequence vanishSequence = DOTween.Sequence().Append(obstacleContent.transform.DOScale(0f, 1f).SetEase(Ease.InOutSine));
            
            vanishSequence.OnComplete(() =>
            {
                obstacleContent.gameObject.SetActive(false);
                this.transform.SetParent(parentObstaclePoolController.transform);
                this.transform.localPosition = Vector3.zero;
                this.transform.localRotation = Quaternion.identity;
                ReadyToBeRecycled = true;
            });
            
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                obstacleEventChannel.OnObstacleHitPlayer(type);
                boxColliders.ForEach(x=>x.enabled = false);
            }
        }

    }
}
