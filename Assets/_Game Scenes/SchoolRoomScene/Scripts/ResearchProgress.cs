using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class ResearchProgress : MonoBehaviour
    {
        [SerializeField] private ActionsTogglerSO actionsToggler;
        [SerializeField] private QuestsManagerSO questsManager;
        [SerializeField] private PlayerResearchDataSO playerResearchData;
        [Space] 
        [SerializeField] private GameObject progressBarCanvas;
        [SerializeField] private Image progressBarImage;
        [SerializeField] private float researchTime = 2f;
        
        private void OnEnable()
        {
            questsManager.deliveredResearchError += ResetSo;
            questsManager.deliveredResearchSuccess += ResetSo;
            playerResearchData.startedResearch += StartResearching;
            playerResearchData.Reset();
        }

        private void OnDisable()
        {
            questsManager.deliveredResearchError -= ResetSo;
            questsManager.deliveredResearchSuccess -= ResetSo;
            playerResearchData.deliveredResearch -= ResetSo;
            playerResearchData.startedResearch -= StartResearching;
        }

        private void StartResearching(ResearchTypeSO _, ResearchTableSO __)
        {
            actionsToggler.MovementToggle(false);
            progressBarImage.fillAmount = 0;
            progressBarCanvas.SetActive(true);

            Sequence researchSequence = DOTween.Sequence().Append(DOTween.To(x => progressBarImage.fillAmount = x,
                progressBarImage.fillAmount, 1, researchTime).SetEase(Ease.Linear));
            
            researchSequence.OnComplete(()=>
            {
                progressBarCanvas.SetActive(false);
                actionsToggler.MovementToggle(true);
                playerResearchData.EndedResearching();
            });
        }

        private void ResetSo(ResearchTypeSO _)
        {
            playerResearchData.Reset();
        }
        
    }
}
