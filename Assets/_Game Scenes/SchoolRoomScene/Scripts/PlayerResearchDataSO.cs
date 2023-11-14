using System;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    [CreateAssetMenu(fileName = "PlayerResearchData", menuName = "ScriptableObjects/SchoolRoomScene/PlayerResearchDataSO")]
    public class PlayerResearchDataSO : ScriptableObject
    {
        public ResearchTypeSO CurrentResearch { get; set; }
        public bool CanResearch { get; set; }
        
        public event Action<ResearchTypeSO, ResearchTableSO> startedResearch;
        public event Action endedResearch;
        public event Action<ResearchTypeSO> deliveredResearch;

        public void StartedResearching(ResearchTypeSO researchType, ResearchTableSO researchTableSo)
        {
            startedResearch?.Invoke(researchType, researchTableSo);
            CurrentResearch = researchType;
            CanResearch = false;
        }

        public void EndedResearching()
        {
            endedResearch?.Invoke();
            CanResearch = true;
        }

        public void DeliveredResearch(ResearchTypeSO researchType)
        {
            deliveredResearch?.Invoke(researchType);
        }

        public void Reset()
        {
            CanResearch = true;
            CurrentResearch = null;
        }
    }
}
