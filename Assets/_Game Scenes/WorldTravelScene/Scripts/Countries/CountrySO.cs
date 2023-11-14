using System;
using UnityEngine;

namespace _WorldTravelScene.Scripts.Countries
{
    [CreateAssetMenu(fileName = "CountrySO", menuName = "ScriptableObjects/WorldTravelScene/CountrySO")]
    public class CountrySO : ScriptableObject
    {
        public enum CountryEnum
        { Brasil, Eua, Paraguai, Uruguai, Argentina, Bolivia, Equador, Peru, Colombia, Venezuela, Canada, China, Franca,
            Alemanha, India, Italia, Japao, Mexico, Russia, AfricaDoSul, CoreiaDoSul, Espanha, ReinoUnido, Portugal,
            Irã, Iraque, ArabiaSaudita, Indonesia, NovaZelandia
        }
        
        [field: SerializeField] public string CountryName { get; set; }
        [field:SerializeField] public CountryEnum Country { get; set; }
        
        [field:SerializeField] public int InnerDivisionPointsAmount { get; set; }
        [field:SerializeField] public int MiddleDivisionPointsAmount { get; set; }
        [field:SerializeField] public int OuterDivisionPointsAmount { get; set; }
    }
}
