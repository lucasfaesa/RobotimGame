using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game_Scenes.SkinChanger.Scripts
{
    [CreateAssetMenu(fileName = "HatManagerSO", menuName = "ScriptableObjects/SkinChanger/HatManagerSO")]
    public class HatManagerSO : ScriptableObject
    {
        [SerializeField] private HatDataSO noHatSo;
        [SerializeField] public List<HatDataSO> hatDataSoList;

        public HatDataSO CurrentSelectedHat { get; set; }

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        
        public void Reset()
        {
            CurrentSelectedHat = noHatSo;
        }

    }
}
