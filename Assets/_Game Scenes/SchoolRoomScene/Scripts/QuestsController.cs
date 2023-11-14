using System;
using System.Collections;
using System.Collections.Generic;
using _Lobby._LevelSelector.Scripts;
using _MeteorShower.Scripts;
using _Village.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class QuestsController : MonoBehaviour
    {
        [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
        [SerializeField] private LevelCompletionSO levelCompletion;
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private PointsManagerSO pointsManager;
        [SerializeField] private QuestsManagerSO questsManager;

        private bool questsCompleted;
        private float questStartedTime;

        private int basePointsByQuestCompleted = 31;
        
        private void OnEnable()
        {
            gameManager.gameStarted += GetRandomQuestionDelayed;
            questsManager.deliveredResearchSuccess += DeliverySuccessful;
            questsManager.finishedNpcDeliverDialog += GetRandomQuestionDelayed;
            questsManager.allQuestsCompleted += AllQuestsCompleted;
        }

        private void OnDisable()
        {
            gameManager.gameStarted -= GetRandomQuestionDelayed;
            questsManager.deliveredResearchSuccess -= DeliverySuccessful;
            questsManager.finishedNpcDeliverDialog -= GetRandomQuestionDelayed;
            questsManager.allQuestsCompleted -= AllQuestsCompleted;
        }

        void Start()
        {
            questsManager.Reset();
            questsManager.SetAllQuests();
        }

        private void AllQuestsCompleted() { questsCompleted = true; }

        private void DeliverySuccessful(ResearchTypeSO _)
        {
            var elapsedTime = Time.time - questStartedTime;
            
            Debug.Log($"Time to complete quest: {elapsedTime}");
            
            var extraPointsToAdd = elapsedTime switch
            {
                < 15 => Random.Range(8,11),
                < 22 => Random.Range(5,9),
                < 36 => Random.Range(3,6),
                < 50 => Random.Range(1,4),
                _ => 0
            };
            
            pointsManager.AddPointsWithoutUpdatingScore(basePointsByQuestCompleted+ extraPointsToAdd);
            
        }
        
        private void GetRandomQuestionDelayed()
        {
            pointsManager.UpdateScore();
            
            if (questsCompleted)
            {
                levelSelectedManager.SetupNextLevel();
                levelCompletion.LevelCompleted = true;
                gameManager.GameEnded();
                return;
            }
            
            StartCoroutine(GetQuestionDelayedCoroutine());
        }
        
        private IEnumerator GetQuestionDelayedCoroutine()
        {
            yield return new WaitForSeconds(3f);
            GetRandomQuest();
            questStartedTime = Time.time;
        }

        void GetRandomQuest()
        {
            questsManager.GetRandomUnusedQuest();
        }
        
        [ContextMenu("Get Random Quest")]
        void DoSomething()
        {
            GetRandomQuest();
        }
    }
}
