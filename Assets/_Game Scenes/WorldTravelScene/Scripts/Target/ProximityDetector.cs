using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _WorldTravelScene.Scripts.Target
{
    public class ProximityDetector : MonoBehaviour
    {
        [SerializeField] private Transform targetRef;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private float delayToActivateDetector = 2f;
        
        private bool crateInsideTrigger;
        private bool playerInsideTrigger;

        private bool scaledUp;
        private bool someoneInside;
        private float lastTimeSomeoneWasInside;
        private float threshold = 0.5f;

        private Vector3 defaultScale;

        private void OnEnable()
        {
            boxCollider.enabled = false;
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(delayToActivateDetector);
            boxCollider.enabled = true;
        }
        
        private void Start()
        {
            defaultScale = targetRef.localScale;
            targetRef.localScale = Vector3.zero;
        }
        
        private void FixedUpdate()
        {
            if (!someoneInside) return;
            
            if (Time.time - lastTimeSomeoneWasInside > threshold)
            {
                someoneInside = false;
                Scale(0f);
            }
                
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Crate") || other.CompareTag("Player"))
            {
                someoneInside = true;
                lastTimeSomeoneWasInside = Time.time;

                if (!scaledUp)
                    Scale(defaultScale.x);
            }
        }

        private void Scale(float value)
        {
            scaledUp = value > 0f;
            
            targetRef.DOScale(value, 0.2f).SetEase(Ease.InOutSine);
        }
    }
}
