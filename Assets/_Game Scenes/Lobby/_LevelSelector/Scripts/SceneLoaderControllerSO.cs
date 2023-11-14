using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Lobby._LevelSelector.Scripts
{
    [CreateAssetMenu(fileName = "LevelsLoaderController", menuName = "ScriptableObjects/LobbyScene/LevelSelector/LevelsLoaderController")]
    public class SceneLoaderControllerSO : ScriptableObject
    {
        [Header("SO")] 
        [SerializeField] private LevelSelectedManagerSO levelSelectedManager;

        public string SceneToBeLoaded { get; private set; }
        
        public void GoToSelectedLevelScene()
        {
            if(levelSelectedManager.CurrentLevelInfo.level != null)
                SceneToBeLoaded = SceneNames.sceneNameAndCodes.Find(x => x.sceneCode == levelSelectedManager.CurrentLevelInfo.subjectThemeDto.Code).sceneName;
            if(levelSelectedManager.CurrentLevelInfo.quiz != null)
                SceneToBeLoaded = SceneNames.spaceshipCombatSceneName;

            SceneManager.LoadScene(SceneNames.loadingScene);
        }

        public void GoToScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;
            
            SceneToBeLoaded = sceneName;
            SceneManager.LoadScene(SceneNames.loadingScene);
        }

        public void ReloadScene()
        {
            SceneToBeLoaded = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(SceneNames.loadingScene);
            
        }
        
        
    }
}
