using System;
using System.Collections;
using _WorldTravelScene.ScriptableObjects.Planet;
using _WorldTravelScene.Scripts.Countries;
using _WorldTravelScene.Scripts.Planet;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace _WorldTravelScene.Scripts.Crate
{
    public class CrateBehavior : MonoBehaviour
    {
        
        [SerializeField] private LandSeaCollisionManagerSO landSeaCollisionManager;
        
        [Header("Crate Settings")]
        [SerializeField] private float crateFallSpeed;
        [SerializeField] private float timeToDisappear = 5f;
        [field:SerializeField] public bool ReadyToBeDeployed { get; private set; }
        
        [Header("References")]
        [SerializeField] private Transform model;
        [SerializeField] private Transform parachuteRef;
        [SerializeField] private Transform parentTransform;
        [SerializeField] private Transform worldRef;

        private CratesController cratesController;
        
        private bool canMove ;
        private bool parachuteOpened;
        private bool hitSomething;
        
        private float timer;
        private float timeToOpenParachute = 2f;
        
        private Vector3 direction;

        private void OnEnable()
        {
            Reset();
        }

        private void Reset()
        {
            parachuteRef.localScale = Vector3.zero;
            timer = 0;
            canMove = false;
            this.transform.localScale = Vector3.one;
            parachuteOpened = false;
            hitSomething = false;
        }
        
        void Update()
        {
            Debug.DrawRay(transform.position, model.TransformDirection(Vector3.forward) * 0.2f, Color.yellow);
            if (canMove)
            {
                transform.Translate((direction * (Time.deltaTime * crateFallSpeed)));
                timer += Time.deltaTime;
                
                if (timer >= timeToOpenParachute && !parachuteOpened)
                {
                    OpenParachute();
                }
                
                model.transform.LookAt(worldRef);

                if (!hitSomething)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, 
                                                model.TransformDirection(Vector3.forward), out hit, 0.2f))
                    {
                        if (hit.transform.CompareTag("ProximityDetector")) return;
                        
//                        Debug.Log("hit: " + hit.transform.name);
                        
                        canMove = false;
                        CloseParachute();
                        
                        hitSomething = true;
                        if (hit.transform.TryGetComponent(out TargetDivision targetDivision))
                        {
                            targetDivision.CrateCollided();
                            return;
                        }

                        if (hit.transform.TryGetComponent(out LandSeaReferenceHolder landSeaReferenceHolder))
                        {
                            Texture2D texture = landSeaReferenceHolder.imageMap;
                            Vector2 pixelUV = hit.textureCoord;
                            pixelUV.x *= texture.width;
                            pixelUV.y *= texture.height;
            
                            Color color = texture.GetPixel(Mathf.FloorToInt(pixelUV.x), Mathf.FloorToInt(pixelUV.y));
                            
                            if(FindIndexFromColor(color) == 0)
                                landSeaCollisionManager.CollidedOnLand(true);
                            else
                            {
                                landSeaCollisionManager.CollidedOnLand(false);
                                Disappear();
                            }

                            return;
                        }
                        
                    }
                }
            }
        }
        
        private int FindIndexFromColor(Color color)
        {
            if (color.r >= 0.5)
                return 0;
        
            return 1;
        }

        public void SetCrate(Vector3 dir, Transform world, Transform crateRef, CratesController cratesContr)
        {
            cratesController = cratesContr;
            model.gameObject.SetActive(true);
            
            worldRef = world;
            direction = dir;
            canMove = true;
            parentTransform = crateRef;
            ReadyToBeDeployed = false;
        }

        private void OpenParachute()
        {
            parachuteOpened = true;
            parachuteRef.DOScale(1f, 1f).SetEase(Ease.OutBack, 2);
        }

        private void CloseParachute()
        {
            parachuteRef.DOScale(0f, 0.5f).SetEase(Ease.InBack, 2);
            StartCoroutine(DisappearDelayed());
        }

        private IEnumerator DisappearDelayed()
        {
            yield return new WaitForSeconds(timeToDisappear);
            
            Disappear();
        }

        private void Disappear()
        {
            Sequence sequence = DOTween.Sequence().Append(this.transform.DOScale(0f, 1f).SetEase(Ease.InOutSine));

            sequence.OnComplete(() =>
            {
                model.gameObject.SetActive(false);
            
                var transform1 = this.transform;
                transform1.parent = parentTransform;
                transform1.localPosition = Vector3.zero;
                transform1.localRotation = quaternion.identity;

                ReadyToBeDeployed = true;
                cratesController.CrateReturned();
                this.gameObject.SetActive(false);
            });
        }
        
    }
}
