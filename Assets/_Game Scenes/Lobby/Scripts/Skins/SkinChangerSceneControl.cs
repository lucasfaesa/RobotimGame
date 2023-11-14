using _Lobby._LevelSelector.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game_Scenes.Lobby.Scripts.Skins
{
    public class SkinChangerSceneControl : MonoBehaviour
    {
        [SerializeField] private SceneLoaderControllerSO sceneLoaderController;
        private bool insideSkinChangeArea;
        
        public void PlayerInsideSkinChangeArea(bool status)
        {
            insideSkinChangeArea = status;
        }
        
        void Update()
        {
            if (insideSkinChangeArea)
            {
                if(Keyboard.current.eKey.wasPressedThisFrame)
                    sceneLoaderController.GoToScene("SkinChangerScene");
            }
        }
    }
}
