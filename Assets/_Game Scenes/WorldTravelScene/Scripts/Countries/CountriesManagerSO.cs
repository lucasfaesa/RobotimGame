using System;
using System.Collections.Generic;
using UnityEngine;

namespace _WorldTravelScene.Scripts.Countries
{
    [CreateAssetMenu(fileName = "CountriesManagerSO", menuName = "ScriptableObjects/WorldTravelScene/CountriesManagerSO")]
    public class CountriesManagerSO : ScriptableObject
    {
        public event Action<CountrySO, TargetDivision.TargetDivisionType, int> targetHit;
        public event Action<CountrySO, TargetDivision.TargetDivisionType, int> targetHitCorrectly;
        public event Action<CountrySO, TargetDivision.TargetDivisionType, int> targetHitIncorrectly;

        [field:SerializeField] public List<CountrySO> CountriesActive { get; set; } = new();

        public void TargetHit(CountrySO country, TargetDivision.TargetDivisionType division, int points)
        {
            targetHit?.Invoke(country, division, points);
            
            //Debug.Log("Country Hit: " + countryName + " Division" + division );
        }

        public void TargetHitCorrectly(CountrySO country, TargetDivision.TargetDivisionType division, int points)
        {
            targetHitCorrectly?.Invoke(country, division, points);
        }

        public void TargetHitIncorrectly(CountrySO country, TargetDivision.TargetDivisionType division, int points)
        {
            targetHitIncorrectly?.Invoke(country, division, points);
        }

        public void ActivatedCountries(List<CountrySO> countries)
        {
            CountriesActive = new(countries);
        }
    }
}
