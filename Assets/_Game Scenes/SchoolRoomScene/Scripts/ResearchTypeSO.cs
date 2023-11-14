using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    [CreateAssetMenu(fileName = "ResearchTypeSO", menuName = "ScriptableObjects/SchoolRoomScene/ResearchTypeSO")]
    public class ResearchTypeSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public BioCategoryEnum BioCategory { get; set; }
        [field: SerializeField] public BioTypeEnum BioType { get; set; }
        
        public enum BioCategoryEnum { Disease, Cure }
        public enum BioTypeEnum {Virus, Bacteria, Protozoa, Vaccine, Antibiotic, Serum}
    }
}
