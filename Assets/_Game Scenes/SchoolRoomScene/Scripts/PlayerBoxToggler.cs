using System;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class PlayerBoxToggler : MonoBehaviour
    {
        [SerializeField] private PlayerResearchDataSO playerResearchData;
        [SerializeField] private PlayerSO playerSo;
        [Space]
        [SerializeField] private GameObject boxGameObject;
        [Space]
        [SerializeField] private Material[] boxesMaterials;
        [SerializeField] private Renderer[] boxesRenderers;
        
        private readonly float playerDefaultBaseSpeed = 1.41f;
        private readonly float playerSlowBaseSpeed = 0.5f;
        
        private void OnEnable()
        {
            Reset();
            playerResearchData.startedResearch += HideBox;
            playerResearchData.startedResearch += ColorBox;
            playerResearchData.endedResearch += ShowBox;
            playerResearchData.deliveredResearch += HideBox;
        }

        private void OnDisable()
        {
            playerResearchData.startedResearch -= HideBox;
            playerResearchData.startedResearch -= ColorBox;
            playerResearchData.endedResearch -= ShowBox;
            playerResearchData.deliveredResearch -= HideBox;
        }

        

        private void ColorBox(ResearchTypeSO _,ResearchTableSO researchTableSo)
        {
            Material material = researchTableSo.FridgeColor switch
            {
                ResearchTableDisplay.FridgeColor.Red => boxesMaterials[0],
                ResearchTableDisplay.FridgeColor.Green => boxesMaterials[1],
                ResearchTableDisplay.FridgeColor.Blue => boxesMaterials[2]
            };

            foreach (var rend in boxesRenderers)
            {
                rend.material = material;
            }
        }

        private void ShowBox()
        {
            boxGameObject.transform.localScale = Vector3.zero;
            boxGameObject.SetActive(true);
            boxGameObject.transform.DOScale(0.01f, 0.4f).SetEase(Ease.OutBack);
            
            playerSo.GroundedData.BaseSpeed = playerSlowBaseSpeed;
        }

        private void HideBox(ResearchTypeSO __, ResearchTableSO _)
        {
            boxGameObject.SetActive(false);
            playerSo.GroundedData.BaseSpeed = playerDefaultBaseSpeed;
        }
        
        private void HideBox(ResearchTypeSO _)
        {
            boxGameObject.SetActive(false);
            playerSo.GroundedData.BaseSpeed = playerDefaultBaseSpeed;
        }

        private void Reset()
        {
            playerSo.GroundedData.BaseSpeed = playerDefaultBaseSpeed;
        }
    }
}
