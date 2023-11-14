using UnityEngine;

namespace _WorldTravelScene.Scripts.Countries
{
    public class TargetDivision : MonoBehaviour
    {
        [SerializeField] private CountryBehavior countryBehavior;
        [Space]
        [SerializeField] private TargetDivisionType targetDivisionDivision;
        public enum TargetDivisionType {Inner, Middle, Outer}

        public void CrateCollided()
        {
            countryBehavior.TargetDivisionHit(targetDivisionDivision);
        }
    }
}
