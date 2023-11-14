using System;
using UnityEngine;

namespace _Game_Scenes.SkinChanger.Scripts
{
    [CreateAssetMenu(fileName = "SkinColorValues", menuName = "ScriptableObjects/SkinChanger/SkinColorValues")]
    public class SkinColorValuesSO : ScriptableObject
    {
        [field:SerializeField] public Color32 PrimaryColor { get; set; } = new Color32((byte)137, (byte)137, (byte)137, (byte)255);
        [field:SerializeField] public Color32 SecondaryColor { get; set; } = new Color32((byte)47, (byte)47, (byte)47, (byte)255);
        [field:SerializeField] public Color32 DetailsColor { get; set; } = new Color32((byte)227, (byte)227, (byte)227, (byte)255);
        
        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        public void Reset()
        { 
            PrimaryColor = new Color32((byte)137, (byte)137, (byte)137, (byte)255);
            SecondaryColor = new Color32((byte)47, (byte)47, (byte)47, (byte)255);
            DetailsColor  = new Color32((byte)227, (byte)227, (byte)227, (byte)255);
        }
    }
}
