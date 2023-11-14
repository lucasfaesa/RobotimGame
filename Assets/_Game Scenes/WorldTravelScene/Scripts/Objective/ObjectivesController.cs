using System;
using System.Collections;
using _Lobby._LevelSelector.Scripts;
using _MeteorShower.Scripts;
using _Village.Scripts;
using _WorldTravelScene.Scripts.Objective;
using DG.Tweening;
using UnityEngine;

namespace _WorldTravelScene.Scripts.Questions
{
    public class ObjectivesController : MonoBehaviour
    {
        [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
        [SerializeField] private LevelCompletionSO levelCompletion;
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private ObjectivesManagerSO objectivesManager;
        [Space] 
        [SerializeField] private float newQuestionDelay = 3f;
        
        [Header("debug")] 
        [SerializeField] private int levelDifficultyDebug;
        
        public enum LevelDifficulty { Easy = 2, Normal = 1, Hard = 0 }; //values are for some references used in crates and countries
        
        private void OnEnable()
        {
            gameManager.gameStarted += GetQuestionDelay;
            objectivesManager.objectiveCompleted += ObjectiveCompleted;
        }

        private void OnDisable()
        {
            gameManager.gameStarted -= GetQuestionDelay;
            objectivesManager.objectiveCompleted -= ObjectiveCompleted;
        }
        
        void Start() { objectivesManager.Reset(); }

        private void GetQuestionDelay()
        {
            StartCoroutine(GetQuestionDelayed());            
        }

        private IEnumerator GetQuestionDelayed()
        {
            yield return new WaitForSeconds(newQuestionDelay);
            
            objectivesManager.GetRandomUnusedQuestion(GetLevelDifficulty());
        }

        private LevelDifficulty GetLevelDifficulty()
        {
            if (levelDifficultyDebug != 0)
            {
                return levelDifficultyDebug switch
                {
                    < 3 => LevelDifficulty.Easy,
                    < 5 => LevelDifficulty.Normal,
                    _ => LevelDifficulty.Hard
                };
            }
            else
            {
                return levelSelectedManager.CurrentLevelInfo.level.Difficulty switch
                {
                    < 3 => LevelDifficulty.Easy,
                    < 5 => LevelDifficulty.Normal,
                    _ => LevelDifficulty.Hard
                };
            }
        }
        
        private void ObjectiveCompleted(ObjectiveInfo n)
        {
            if (objectivesManager.CompletedObjectivesAmount == objectivesManager.ObjectivesAmountToFinishLevel)
            {
                float number = 0;
                Sequence waitSequence = DOTween.Sequence().Append(DOTween.To(x => number = x, number, 1, 1.5f)); //instead of creating a ienumerator i created a sequence that changes some value and waits some time
                waitSequence.OnComplete(() =>
                {
                    levelSelectedManager.SetupNextLevel();
                    levelCompletion.LevelCompleted = true;
                    gameManager.GameWon();
                    gameManager.GameEnded();    
                });
                return;
            }

            StartCoroutine(GetQuestionDelayed());
        }

    }
}
