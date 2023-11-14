using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    [CreateAssetMenu(fileName = "QuestsManager", menuName = "ScriptableObjects/SchoolRoomScene/QuestsManagerSO")]
    public class QuestsManagerSO : ScriptableObject
    {
        [SerializeField] private int questsToComplete = 3;
        [SerializeField] private List<SchoolRoomQuestsSO> quests;
        
        public SchoolQuest CurrentQuest { get; private set; }

        private List<SchoolQuest> usedQuests;
        private List<SchoolQuest> allQuests;

        public event Action<SchoolQuest> newQuestGot;
        public event Action questDialogEnded;
        
        public event Action<ResearchTypeSO> deliveredResearchSuccess;
        public event Action<ResearchTypeSO> deliveredResearchError;

        public event Action finishedNpcDeliverAnimation;
        public event Action finishedNpcDeliverDialog;

        public event Action allQuestsCompleted;
        
        private int questsCompletedCount;
        
        
        public void SetAllQuests()
        {
            foreach (var quest in quests)
            {
                foreach (var desc in quest.Description)
                {
                    allQuests.Add(new SchoolQuest(desc, quest.ResearchTypeCorrectAnswer));
                }
            }
            
            allQuests.Shuffle();
        }
        
        public SchoolQuest GetRandomUnusedQuest()
        {
            CurrentQuest = null;
            
            int randomIndex;

            if (usedQuests.Count == allQuests.Count)
                usedQuests = new();
            
            while (CurrentQuest == null)
            {
                randomIndex = Random.Range(0, allQuests.Count);
                
                if (usedQuests.Contains(allQuests[randomIndex]))
                    continue;
                
                CurrentQuest = allQuests[randomIndex];
                
                usedQuests.Add(CurrentQuest);

            }
            
            newQuestGot?.Invoke(CurrentQuest);
            //Debug.Log($"Current Quest: {CurrentQuest.Description}");
            return CurrentQuest;
        }

        public void ResearchDelivered(ResearchTypeSO research)
        {
            if(research == CurrentQuest.ResearchTypeCorrectAnswer)
                DeliveredResearchSuccess(research);
            else
                DeliveredResearchError(research);
        }
        
        private void DeliveredResearchSuccess(ResearchTypeSO research)
        {
            deliveredResearchSuccess?.Invoke(research);
            questsCompletedCount++;
            
            if(questsCompletedCount == questsToComplete)
                allQuestsCompleted?.Invoke();
        }

        private void DeliveredResearchError(ResearchTypeSO research)
        {
            deliveredResearchError?.Invoke(research);
            questsCompletedCount++;
            
            if(questsCompletedCount == questsToComplete)
                allQuestsCompleted?.Invoke();
        }

        public void FinishedNpcDeliverAnimation()
        {
            finishedNpcDeliverAnimation?.Invoke();
        }

        public void FinishedNpcDeliverDialog()
        {
            finishedNpcDeliverDialog?.Invoke();
        }
        
        public void Reset()
        {
            CurrentQuest = null;
            questsCompletedCount = 0;
            usedQuests = new();
            allQuests = new();
        }

        public void QuestDialogEnded()
        {
            questDialogEnded?.Invoke();
        }
    }

    public class SchoolQuest
    {
        public string Description { get; set; }
        public ResearchTypeSO ResearchTypeCorrectAnswer { get; set; }

        public SchoolQuest(string desc, ResearchTypeSO research)
        {
            this.Description = desc;
            this.ResearchTypeCorrectAnswer = research;
        }
    }
}
