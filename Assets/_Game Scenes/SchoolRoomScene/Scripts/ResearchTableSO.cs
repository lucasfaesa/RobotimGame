using System;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    
    [CreateAssetMenu(fileName = "ResearchTableData", menuName = "ScriptableObjects/SchoolRoomScene/ResearchTableData")]
    public class ResearchTableSO : ScriptableObject
    {
        [field: SerializeField] [field:ReadOnly] public ResearchTypeSO ResearchType { get; set; }
        public ResearchTableDisplay.FridgeColor FridgeColor { get; set; }

        public void Reset()
        {
            ResearchType = null;
        }
    }
}
