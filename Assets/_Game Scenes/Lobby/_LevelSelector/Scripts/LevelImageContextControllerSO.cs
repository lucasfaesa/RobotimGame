using System.Collections.Generic;
using UnityEngine;

namespace _Lobby._LevelSelector.Scripts
{
    [CreateAssetMenu(fileName = "LevelImageContextController", menuName = "ScriptableObjects/LobbyScene/LevelSelector/LevelImageContextController")]
    public class LevelImageContextControllerSO : ScriptableObject
    {
        [SerializeField] private List<ContextMenuPreviewDataSO> subjectsThemesIdAndPreviews = new();
        [SerializeField] private ContextMenuPreviewDataSO quizContext;
        public ContextMenuPreviewDataSO GetContextMenuPreview(LevelInfoAndScore data)
        {
            ContextMenuPreviewDataSO context = null;
            
            if (data.level != null)
            {
                context = subjectsThemesIdAndPreviews.Find(x => x.SubjectThemeId == data.level.SubjectThemeId);
            }

            if (data.quiz != null)
            {
                context = quizContext;
            }

            return context;
        }
        
        public ContextMenuPreviewDataSO GetContextMenuPreviewBySceneName(string sceneName)
        {
            ContextMenuPreviewDataSO context = null;
            
            context = subjectsThemesIdAndPreviews.Find(x => x.SceneName == sceneName);

            if (context == null)
                context = quizContext;
            
            return context;
        }
    }
}
