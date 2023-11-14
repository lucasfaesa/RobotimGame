using UnityEngine;

namespace _Lobby._LevelSelector.Scripts
{
    [CreateAssetMenu(fileName = "ContextMenuPreviewData", menuName = "ScriptableObjects/LobbyScene/LevelSelector/ContextMenuPreviewData")]
    public class ContextMenuPreviewDataSO : ScriptableObject
    {
        [field:SerializeField] public int SubjectThemeId { get; set; }
        [field:SerializeField] public string SceneName { get; set; }
        
        [field:SerializeField] public string LevelName { get; set; }
        [field:SerializeField] public Sprite ContextImagePreview { get; set; }
        [field:SerializeField] public Sprite LoadingImagePreview { get; set; }
    }
}
