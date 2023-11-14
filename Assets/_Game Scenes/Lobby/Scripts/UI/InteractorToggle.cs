using System;
using UnityEngine;

namespace _Lobby.Scripts.UI
{
    public class InteractorToggle : MonoBehaviour
    {
        [Header("SO")] 
        [SerializeField] private CollisionManagerSO collisionManager;
        [Space] 
        [SerializeField] private GameObject interactGameObject; 
            
        private void OnEnable()
        {
            collisionManager.collisionWithMissionBoard += ToggleInteractSprite;
            collisionManager.collisionWithSkinChanger += ToggleInteractSprite;
        }

        private void OnDisable()
        {
            collisionManager.collisionWithMissionBoard -= ToggleInteractSprite;
            collisionManager.collisionWithSkinChanger -= ToggleInteractSprite;
        }

        private void ToggleInteractSprite(bool status)
        {
            interactGameObject.gameObject.SetActive(status);
        }
    }
}
