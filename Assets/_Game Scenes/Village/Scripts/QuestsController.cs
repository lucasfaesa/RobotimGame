using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Game_Scenes.Village.Scripts
{
    public class QuestsController : MonoBehaviour
    {
        [SerializeField] private QuestManagerSO questManager;
        [SerializeField] [ReadOnly] private int completedQuests;
        [SerializeField] [ReadOnly] private int totalQuests;
        [SerializeField] [ReadOnly] private string currentTextDescription;

        private void OnEnable()
        {
            UpdateTotalQuests(questManager.TotalQuestsQuantity);
            UpdateCompletedQuests();

            questManager.QuestEnded += UpdateCompletedQuests;
        }

        private void OnDisable()
        {
            questManager.QuestEnded -= UpdateCompletedQuests;
        }

        private void Start()
        {
            questManager.SetRandomNpcQuestGiverAndQuestType();
        }

        private void UpdateTotalQuests(int total)
        {
            totalQuests = total;
        }

        private void UpdateCompletedQuests()
        {
            completedQuests = questManager.CompletedQuests;
        }


    }
}
