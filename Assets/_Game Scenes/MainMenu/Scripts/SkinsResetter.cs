using System;
using _Game_Scenes.SkinChanger.Scripts;
using UnityEngine;

namespace _Game_Scenes.MainMenu.Scripts
{
    public class SkinsResetter : MonoBehaviour
    {
        [SerializeField] private HatManagerSO hatManagerSo;
        [SerializeField] private SkinColorValuesSO skinColorValuesSo;

        private void Start()
        {
            skinColorValuesSo.Reset();
            hatManagerSo.Reset();
        }
    }
}
