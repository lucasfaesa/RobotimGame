using _Lobby._LevelSelector.Scripts;
using UnityEngine;

namespace _MeteorShower.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
        [SerializeField] private GameManagerSO gameManager;

        void Start()
        {
            if(levelSelectedManager.CurrentLevelInfo.level != null)
                Debug.Log("Level ID: " + levelSelectedManager.CurrentLevelInfo.level.Id);
            
            gameManager.SceneLoaded();
            //gameManager.PreparingToStartGame();
        }
        
    }
}
