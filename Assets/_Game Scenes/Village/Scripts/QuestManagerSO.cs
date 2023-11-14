using System;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using _Village.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Scenes.Village.Scripts
{
    [CreateAssetMenu(fileName = "QuestManager", menuName = "ScriptableObjects/VillageScene/QuestManager", order = 9)]
    public class QuestManagerSO : ScriptableObject
    {
        [Header("SO")] 
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private LevelCompletionSO levelCompletion;
        [Header("Quests SOs")] 
        [SerializeField] private FindObjectQuestSO findObjectQuest;
        [SerializeField] private int totalQuestsQuantity;
        [Space] 
        [SerializeField] [ReadOnly] private VillageNpcDataSO currentQuestGiverNpc;
        [SerializeField] [ReadOnly] private VillageNpcDataSO currentQuestInteractorNpc;

        [Header("List of NPCs")] 
        [SerializeField] private List<VillageNpcDataSO> AllNpcsData;

        public bool InQuest { get; private set; }
        public bool InOverlayQuest { get; private set; }
    
        public int CompletedQuests { get; private set; }
        public enum QuesType {None, Planification, FindObject, IdentifyObject, StreetsMap, ObjectDelivery}

        public event Action<bool> InOverlay;
        public event Action<QuesType> QuestStarted;
        public event Action QuestEnded;
        public event Action<VillageNpcDataSO> questGiverNpcChanged;
        public event Action<VillageNpcDataSO> questInteractorNpcChanged;
        public event Action startedFindObjectQuestObjectSelection;
        public int TotalQuestsQuantity => totalQuestsQuantity;

        public VillageNpcDataSO CurrentQuestGiverNpc => currentQuestGiverNpc;
        public VillageNpcDataSO CurrentQuestInteractorNpc => currentQuestInteractorNpc;

        private void OnEnable()
        {
            //findObjectQuest.correctObjectChoice += EndQuest;
        }
        private void OnDisable()
        {
            //findObjectQuest.correctObjectChoice -= EndQuest;
            ResetSO();
        }

        public void SetRandomNpcQuestGiverAndQuestType()
        {
            int randomNumber = Random.Range(0, AllNpcsData.Count);
        
            AllNpcsData[randomNumber].SetNpcType(VillageNpcDataSO.NpcType.QuestGiver, QuestManagerSO.QuesType.FindObject);
        
            //TODO to make random quest type

        }
    
        public void InOverlayToggle(bool status)
        {
            InOverlayQuest = status;
            InOverlay?.Invoke(status);
        }
    
        public void ChangeCurrentQuestGiverNpc(VillageNpcDataSO data)
        {
            currentQuestGiverNpc = data;
            questGiverNpcChanged?.Invoke(data);
        }
        public void ChangeCurrentQuestInteractorNpc(VillageNpcDataSO data)
        {
            currentQuestInteractorNpc = data;
            questInteractorNpcChanged?.Invoke(data);
        }
    
        public void SetCurrentQuest(QuesType qType)
        {
            switch (qType)
            {
                case QuesType.FindObject:
                    findObjectQuest.SetRandomQuestParameters();
                    break;
            }
        
            StartQuest(qType);
        }

        public void ActivateFindObjectQuestObjectSelection()
        {
            startedFindObjectQuestObjectSelection?.Invoke();
            currentQuestInteractorNpc.CanDialog = false;
        }
    
        public void StartQuest(QuesType qType)
        {
            InQuest = true;
            QuestStarted?.Invoke(qType);
        }

        public void EndQuest()
        {
            ResetNpcs();
        
            InQuest = false;
            CompletedQuests++;
            if (CompletedQuests == totalQuestsQuantity)
            {
                levelCompletion.LevelCompleted = true;
                gameManager.GameEnded();
            }
            else
                SetRandomNpcQuestGiverAndQuestType();

            QuestEnded?.Invoke();
        }

        private void ResetNpcs()
        {
            currentQuestGiverNpc.CanDialog = true;
            currentQuestInteractorNpc.CanDialog = true;
            currentQuestGiverNpc.SetNpcType(VillageNpcDataSO.NpcType.Default,QuesType.None);
            currentQuestInteractorNpc.SetNpcType(VillageNpcDataSO.NpcType.Default,QuesType.None);
        }

        private void ResetSO()
        {
            CompletedQuests = 0;
            currentQuestGiverNpc = null;
            currentQuestInteractorNpc = null;
        }


    }
}


