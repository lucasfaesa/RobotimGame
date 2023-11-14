using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class NpcResearchDeliveryController : MonoBehaviour
    {
        [SerializeField] private QuestsManagerSO questsManager;
        [SerializeField] private NpcColliderSO npcColliderSo;
        [SerializeField] private PlayerResearchDataSO playerResearchData;

        private bool canInteractWithNpc;
        
        
        void Update()
        {
            if(canInteractWithNpc)
                if (Keyboard.current.eKey.wasReleasedThisFrame)
                {
                    playerResearchData.DeliveredResearch(playerResearchData.CurrentResearch);
                    questsManager.ResearchDelivered(playerResearchData.CurrentResearch);
                    Delivered();
                }
        }

        public void TriggerEnter()
        {
            if (playerResearchData.CurrentResearch != null)
            {
                npcColliderSo.CollisionEnter();
                canInteractWithNpc = true;
            }
        }

        public void TriggerExit()
        {
            if (playerResearchData.CurrentResearch != null)
            {
                npcColliderSo.CollisionExit();
                canInteractWithNpc = false;
            }
        }

        private void Delivered()
        {
            npcColliderSo.CollisionExit();
            canInteractWithNpc = false;
        }
    }
}
