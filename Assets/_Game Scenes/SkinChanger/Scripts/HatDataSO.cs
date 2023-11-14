using UnityEngine;

namespace _Game_Scenes.SkinChanger.Scripts
{
    [CreateAssetMenu(fileName = "HatDataSO", menuName = "ScriptableObjects/SkinChanger/HatDataSO")]
    public class HatDataSO : ScriptableObject
    {
        [field:SerializeField] public string HatName { get; private set; }
        [field:SerializeField] public float HatValue { get; private set; }

        public GameObject hatPrefab;
        
        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }
    }
}
