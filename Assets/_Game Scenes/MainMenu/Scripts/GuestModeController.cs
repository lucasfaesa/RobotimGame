using System;
using APIComms;
using UnityEngine;

namespace _Game_Scenes.MainMenu.Scripts
{
    public class GuestModeController : MonoBehaviour
    {
        [SerializeField] private PlayerDataSO playerDataSo;
        
        private void Awake()
        {
            SetGuestMode(false);
        }

        public void SetGuestMode(bool status)
        {
            playerDataSo.GuestMode = status;
        }
    }
}
