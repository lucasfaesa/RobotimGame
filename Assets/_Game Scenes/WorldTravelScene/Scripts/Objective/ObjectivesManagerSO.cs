using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using _Lobby._LevelSelector.Scripts;
using _WorldTravelScene.Scripts.Questions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _WorldTravelScene.Scripts.Objective
{
    [CreateAssetMenu(fileName = "ObjectivesManagerSO", menuName = "ScriptableObjects/WorldTravelScene/ObjectivesManagerSO")]
    public class ObjectivesManagerSO : ScriptableObject
    {
        [field:SerializeField] public List<ObjectiveSO> Objectives { get; set; }
        [field:SerializeField] public int ObjectivesAmountToFinishLevel { get; set; } = 3;
        [field:SerializeField] [field:ReadOnly] public int CompletedObjectivesAmount { get; private set; }
        
        public ObjectiveInfo CurrentObjective { get; set; }
        
        public event Action<ObjectiveInfo, ObjectivesController.LevelDifficulty> newObjectiveGot;
        public event Action<ObjectiveInfo> objectiveStarted;
        public event Action<ObjectiveInfo> correctCountryDelivered;
        public event Action<ObjectiveInfo> objectiveCompleted;
        public event Action allObjectivesCompleted;

        private List<ObjectiveSO> notUsedObjectives;

        public void GetRandomUnusedQuestion(ObjectivesController.LevelDifficulty difficulty)
        {
            if (notUsedObjectives.Count == 0)
            {
                Debug.Log("Refreshing Questions");
                notUsedObjectives = new(Objectives);
            }
            
            int randomNumber = Random.Range(0, notUsedObjectives.Count);

            CurrentObjective = notUsedObjectives[randomNumber].GetObjectiveInfos();
            
            notUsedObjectives.RemoveAt(randomNumber);
            
            StartNewQuestion(difficulty);
        }
        
        private void StartNewQuestion(ObjectivesController.LevelDifficulty difficulty)
        {
            newObjectiveGot?.Invoke(CurrentObjective, difficulty);
        }

        public void ObjectiveStarted()
        {
            objectiveStarted?.Invoke(CurrentObjective);
        }

        public void DeliveredToCorrectCountry()
        {
            CurrentObjective.CountriesDelivered++;
            
            correctCountryDelivered?.Invoke(CurrentObjective);
            
            if (CurrentObjective.CountriesQuantityToCompleteMission == CurrentObjective.CountriesDelivered)
            {
                CompletedObjectivesAmount++;
                objectiveCompleted?.Invoke(CurrentObjective);
                CurrentObjective = null;
            }
            
            if(CompletedObjectivesAmount == ObjectivesAmountToFinishLevel)
                allObjectivesCompleted?.Invoke();
        }

        public void Reset()
        {
            notUsedObjectives = new(Objectives);
            CurrentObjective = null;
            CompletedObjectivesAmount = 0;
        }
    }
}
