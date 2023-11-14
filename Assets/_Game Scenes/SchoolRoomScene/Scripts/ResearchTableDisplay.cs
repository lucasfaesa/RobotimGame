using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class ResearchTableDisplay : MonoBehaviour
    {
        [SerializeField] private ResearchTableSO researchTableSo;
        [SerializeField] private PlayerResearchDataSO playerResearchData;
        [SerializeField] private ResearchTableColliderSO colliderData;
        [Space]
        [SerializeField] public TextMeshPro textName;
        [Space] 
        [SerializeField] private FridgeColor fridgeColor;

        public enum FridgeColor { Red, Green, Blue }

        private bool canInteractWithResearchTable;

        public void ResetSos()
        {
            researchTableSo.Reset();
        }

        public void SetData(ResearchTypeSO data)
        {
            researchTableSo.ResearchType = data;
            researchTableSo.FridgeColor = fridgeColor;
            
            textName.text = data.Name;
        }

        public void EnteredCollisionOnTable()
        {
            colliderData.CollisionEnter(researchTableSo);
            canInteractWithResearchTable = playerResearchData.CanResearch;
        }

        public void LeftCollisionOnTable()
        {
            colliderData.CollisionExit(researchTableSo);
            canInteractWithResearchTable = false;
        }

        private void Update()
        {
            if (canInteractWithResearchTable)
            {
                if (Keyboard.current.eKey.wasReleasedThisFrame)
                {
                    playerResearchData.StartedResearching(researchTableSo.ResearchType, researchTableSo);
                    canInteractWithResearchTable = false;
                }
            }
        }
    }
}
