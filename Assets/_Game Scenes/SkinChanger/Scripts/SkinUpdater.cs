using System;
using UnityEngine;

namespace _Game_Scenes.SkinChanger.Scripts
{
    public class SkinUpdater : MonoBehaviour
    {
        [Header("Skin Color")]
        [SerializeField] private SkinColorValuesSO skinColorValues;
        [SerializeField] private Material primaryColorMaterial;
        [SerializeField] private Material secondaryColorMaterial;
        [SerializeField] private Material detailsColorMaterial;

        [Header("Hats")] 
        [SerializeField] private bool updateHatOnStart = true;
        [SerializeField] private HatManagerSO hatManagerSo;
        [SerializeField] private Transform hatTransformReference;
        
        
        private void Awake()
        {
            primaryColorMaterial.color = skinColorValues.PrimaryColor;
            secondaryColorMaterial.color = skinColorValues.SecondaryColor;
            detailsColorMaterial.color = skinColorValues.DetailsColor;

            if(updateHatOnStart)
                if (hatManagerSo.CurrentSelectedHat != null)
                    Instantiate(hatManagerSo.CurrentSelectedHat.hatPrefab, hatTransformReference);
        }
    }
}
