using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _WorldTravelScene.Scripts.Countries
{
    public class CountryBehavior : MonoBehaviour
    {
        [SerializeField] private CountrySO countrySo;
        [SerializeField] private CountriesManagerSO countriesManager;
        [Space] 
        [SerializeField] private Transform countryContext;
        [SerializeField] private TextMeshPro countryNameTxt;
        [SerializeField] private List<Collider> targetColliders;

        public CountrySO GetCountry => countrySo;

        private Sequence animateIn;
        private Sequence animateOut;

        private void OnEnable()
        {
            countriesManager.targetHitCorrectly += TargetHitCorrectly;
        }

        private void OnDisable()
        {
            countriesManager.targetHitCorrectly -= TargetHitCorrectly;
        }

        void Start()
        {
            animateIn = DOTween.Sequence().Append(this.countryContext.DOScale(1f, 0.5f).SetEase(Ease.InOutSine)).SetAutoKill(false).Pause();
            animateOut = DOTween.Sequence().Append(this.countryContext.DOScale(0f, 0.5f).SetEase(Ease.InOutSine)).SetAutoKill(false).Pause();
            countryNameTxt.text = countrySo.CountryName;
        }

        private void TargetHitCorrectly(CountrySO country, TargetDivision.TargetDivisionType targetDivisionType, int arg3)
        {
            if (country == countrySo)
            {
                ToggleCollider(false);
                StartCoroutine(ToggleDelayed(1.5f,false));
            }
        }

        private IEnumerator ToggleDelayed(float delay, bool show)
        {
            yield return new WaitForSeconds(delay);
            
            Toggle(show);
        }
        
        public void Toggle(bool show)
        {
            if (show)
            {
                ToggleCollider(true);
                countryContext.localScale = Vector3.zero;
                countryContext.gameObject.SetActive(true);
                animateIn.Restart();
            }
            else
            {
                if (!countryContext.gameObject.activeInHierarchy) return;
                
                countryContext.localScale = Vector3.one;
                animateOut.Restart();
                animateOut.OnComplete(() =>
                {
                    countryContext.gameObject.SetActive(false);
                });
            }
        }

        public void TargetDivisionHit(TargetDivision.TargetDivisionType divisionType)
        {
            int pointsAmount = 0;

            switch (divisionType)
            {
                    case TargetDivision.TargetDivisionType.Inner:
                        pointsAmount = countrySo.InnerDivisionPointsAmount;
                    break;
                    case TargetDivision.TargetDivisionType.Middle:
                        pointsAmount = countrySo.MiddleDivisionPointsAmount;
                    break;
                    case TargetDivision.TargetDivisionType.Outer:
                        pointsAmount = countrySo.OuterDivisionPointsAmount;
                    break;
            }

            countriesManager.TargetHit(countrySo, divisionType, pointsAmount);
        }

        private void ToggleCollider(bool show)
        {
            foreach (var targetCollider in targetColliders) { targetCollider.enabled = show; }
        }
    }
}
