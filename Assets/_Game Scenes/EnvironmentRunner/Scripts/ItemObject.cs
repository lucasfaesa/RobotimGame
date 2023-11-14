using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private ItemsPoolController parentItemPoolController;
        [SerializeField] private CollectablesManagerSO collectablesManager;
        [SerializeField] private RoadItemSpawner.ItemType type;
        [SerializeField] private ParticleSystem collectEffectParticle;
        [SerializeField] private GameObject itemContent;
        [SerializeField] private BoxCollider boxCollider;
        
        public bool ReadyToBeRecycled { get; private set; } = true;
        
        public void SetParentItemPoolController(ItemsPoolController parent)
        {
            parentItemPoolController = parent;
        }

        public void ActivateItem(Transform parent)
        {
            itemContent.gameObject.SetActive(true);
            this.transform.SetParent(parent);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            boxCollider.enabled = true;
            ReadyToBeRecycled = false;
        }

        public void DeactivateItem()
        {
            itemContent.gameObject.SetActive(false);
            this.transform.SetParent(parentItemPoolController.transform);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            boxCollider.enabled = false;
            ReadyToBeRecycled = true;
        }
        
        public void VanishObject()
        {
            boxCollider.enabled = false;
            Sequence vanishSequence = DOTween.Sequence().Append(itemContent.transform.DOScale(0f, 1f).SetEase(Ease.InOutSine));
            
            vanishSequence.OnComplete(() =>
            {
                this.transform.SetParent(parentItemPoolController.transform);
                this.transform.localPosition = Vector3.zero;
                this.transform.localRotation = Quaternion.identity;
                ReadyToBeRecycled = true;
            });
            
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                collectablesManager.CollectedItem(type);
                StartCoroutine(DeParentParticleAndPlay());
                DeactivateItem();
            }
        }

        private IEnumerator DeParentParticleAndPlay()
        {
            collectEffectParticle.transform.SetParent(null);
            collectEffectParticle.Play();
            yield return new WaitForSeconds(collectEffectParticle.main.duration);
            collectEffectParticle.transform.SetParent(this.transform);
        }
    }
}
