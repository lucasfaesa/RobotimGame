using System;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class UICanvasController : MonoBehaviour
    {
        [SerializeField] private ResearchTableColliderSO researchTableCollider;
        [SerializeField] private NpcColliderSO npcColliderSo;
        [Space] 
        [SerializeField] private GameObject interactDisplay;
        
        private void OnEnable()
        {
            researchTableCollider.EnteredCollision += ShowInteractDisplay;
            researchTableCollider.LeftCollision += HideInteractDisplay;
            npcColliderSo.EnteredCollision += ShowInteractDisplay;
            npcColliderSo.LeftCollision += HideInteractDisplay;
        }
        
        private void OnDisable()
        {
            researchTableCollider.EnteredCollision -= ShowInteractDisplay;
            researchTableCollider.LeftCollision -= HideInteractDisplay;
            npcColliderSo.EnteredCollision -= ShowInteractDisplay;
            npcColliderSo.LeftCollision -= HideInteractDisplay;
        }

        private void ShowInteractDisplay(ResearchTableSO _)
        {
            interactDisplay.SetActive(true);
        }
        private void ShowInteractDisplay()
        {
            interactDisplay.SetActive(true);
        }

        private void HideInteractDisplay(ResearchTableSO _)
        {
            interactDisplay.SetActive(false);
        }
        
        private void HideInteractDisplay()
        {
            interactDisplay.SetActive(false);
        }
        
    }
}
