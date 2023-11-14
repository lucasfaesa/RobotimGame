using System;
using UnityEngine;

namespace _Game_Scenes.SkinChanger.Scripts
{
    public class SkinChangeController : MonoBehaviour
    {
        [SerializeField] private SkinColorValuesSO skinColorValues;
        [Space]
        [SerializeField] private Material primaryColorMaterial;
        [SerializeField] private Material secondaryColorMaterial;
        [SerializeField] private Material detailsColorMaterial;
        [Space]
        [SerializeField] private FlexibleColorPicker primaryColorPicker;
        [SerializeField] private FlexibleColorPicker secondaryColorPicker;
        [SerializeField] private FlexibleColorPicker tertiaryColorPicker;
        
        private void OnEnable()
        {
            primaryColorPicker.onColorChange.AddListener(ChangePrimaryColor);
            secondaryColorPicker.onColorChange.AddListener(ChangeSecondaryColor);
            tertiaryColorPicker.onColorChange.AddListener(ChangeDetailsColor);
        }

        private void OnDisable()
        {
            primaryColorPicker.onColorChange.RemoveListener(ChangePrimaryColor);
            secondaryColorPicker.onColorChange.RemoveListener(ChangeSecondaryColor);
            tertiaryColorPicker.onColorChange.RemoveListener(ChangeDetailsColor);
        }
        
        private void Start()
        {
            AdaptColorPickerToPlayerColor();
        }

        private void AdaptColorPickerToPlayerColor()
        {
            primaryColorPicker.SetColor(skinColorValues.PrimaryColor);
            secondaryColorPicker.SetColor(skinColorValues.SecondaryColor);
            tertiaryColorPicker.SetColor(skinColorValues.DetailsColor);
        }

        private void ChangePrimaryColor(Color color)
        {
            primaryColorMaterial.color = color;
        }
    
        private void ChangeSecondaryColor(Color color)
        {
            secondaryColorMaterial.color = color;
        }
    
        private void ChangeDetailsColor(Color color)
        {
            detailsColorMaterial.color = color;
        }

        public void SaveChanges()
        {
            skinColorValues.PrimaryColor = primaryColorMaterial.color;
            skinColorValues.SecondaryColor = secondaryColorMaterial.color;
            skinColorValues.DetailsColor = detailsColorMaterial.color;
        }
    
    
    }
}
