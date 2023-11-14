using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class ResearchTablesController : MonoBehaviour
    {
        [Header("So's")] 
        [SerializeField] private PlayerResearchDataSO playerResearchData;
        [SerializeField] private QuestsManagerSO questManager;
        [SerializeField] private List<ResearchTypeSO> diseasesData;
        [SerializeField] private List<ResearchTypeSO> curesData;
        [Header("Research Tables")] 
        [SerializeField] private List<ResearchTableDisplay> researchTables;
        [SerializeField] private List<Transform> researchTableSpots;

        private void OnEnable()
        {
            questManager.newQuestGot += SetResearchTables;
            questManager.questDialogEnded += AnimateTablesIn;
            playerResearchData.deliveredResearch += AnimateTablesOut;
        }

        private void OnDisable()
        {
            questManager.newQuestGot -= SetResearchTables;
            questManager.questDialogEnded -= AnimateTablesIn;
            playerResearchData.deliveredResearch -= AnimateTablesOut;
        }

        private void Start()
        {
            researchTables.ForEach(x=>x.ResetSos());
        }

        // Update is called once per frame
        void Update()
        {
            /*if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SetResearchTables();
            }*/
        }

        private void SetResearchTables(SchoolQuest schoolQuest)
        {
            ToggleAllResearchTables(false);
            
            List<Transform> randomSpots = new(researchTableSpots);
            randomSpots.Shuffle();

            for (int i = 0; i < researchTables.Count; i++)
            {
                researchTables[i].transform.position = randomSpots[i].position;
            }
            
            SetTablesData(schoolQuest);
        }

        private void SetTablesData(SchoolQuest quest)
        {
           List<ResearchTypeSO> randomizedTypeList = new();
           
           switch (quest.ResearchTypeCorrectAnswer.BioCategory)
           {
               case ResearchTypeSO.BioCategoryEnum.Cure:
                   randomizedTypeList.AddRange(curesData); break; 
               default:
                   randomizedTypeList.AddRange(diseasesData); break;
           }
           
           randomizedTypeList.Shuffle();

           for (int i = 0; i < researchTables.Count; i++)
           {
               researchTables[i].SetData(randomizedTypeList[i]);
           }
           
        }
        

        private void AnimateTablesIn()
        {
            foreach (var table in researchTables) { table.transform.localScale = Vector3.zero; }
            ToggleAllResearchTables(true);
            
            Sequence firstSequence = DOTween.Sequence().Append(researchTables[0].transform.DOScale(1f, 0.6f).SetEase(Ease.OutBack));
            Sequence secondSequence = DOTween.Sequence().Append(researchTables[1].transform.DOScale(1f, 0.6f).SetEase(Ease.OutBack)).PrependInterval(0.2f);
            Sequence thirdSequence = DOTween.Sequence().Append(researchTables[2].transform.DOScale(1f, 0.6f).SetEase(Ease.OutBack)).PrependInterval(0.4f);
        }

        private void AnimateTablesOut(ResearchTypeSO _)
        {
            Sequence firstSequence = DOTween.Sequence().Append(researchTables[0].transform.DOScale(0f, 0.6f).SetEase(Ease.OutBack));
            Sequence secondSequence = DOTween.Sequence().Append(researchTables[1].transform.DOScale(0f, 0.6f).SetEase(Ease.OutBack)).PrependInterval(0.2f);
            Sequence thirdSequence = DOTween.Sequence().Append(researchTables[2].transform.DOScale(0f, 0.6f).SetEase(Ease.OutBack)).PrependInterval(0.4f);

            thirdSequence.OnComplete(() =>
            {
                ToggleAllResearchTables(false);
            });
        }

        private void ToggleAllResearchTables(bool status)
        {
            foreach (var researchTableDisplay in researchTables)
            {
                researchTableDisplay.gameObject.SetActive(status);
            }
        }
    }
}
