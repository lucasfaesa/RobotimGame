using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _WorldTravelScene.Scripts.Timer
{
    public class TimersAndScoresController : MonoBehaviour
    {
        [SerializeField] private TimerScoreDataSO timerScoreData;
        [Space]
        [SerializeField] private TimerAndScoreDisplay timerAndScoreDisplayPrefab;
        [SerializeField] private List<TimerAndScoreDisplay> timerAndScoreDisplayPool;
        [Header("Game Finished Scren")]
        [SerializeField] private Transform gameFinishedScreenTimerAndScoreTransform;
        [SerializeField] private List<TimerAndScoreDisplay> timerAndScoreGameFinishedScreenPool;
        private void OnEnable() { timerScoreData.timerAndScoreAdded += MarkTimeAndScore; }
        private void OnDisable() { timerScoreData.timerAndScoreAdded -= MarkTimeAndScore; }
        
        private void MarkTimeAndScore(float time, int points)
        {
            int number = timerAndScoreDisplayPool.Count(x => x.isActiveAndEnabled) + 1;
            
            foreach (var timeAndScore in timerAndScoreDisplayPool)
            {
                if (!timeAndScore.isActiveAndEnabled)
                {
                    timeAndScore.gameObject.SetActive(true);
                    timeAndScore.SetData(number, time, points);
                    //return;
                    break;
                }
            }
            
            foreach (var timeAndScore in timerAndScoreGameFinishedScreenPool)
            {
                if (!timeAndScore.gameObject.activeSelf)
                {
                    timeAndScore.gameObject.SetActive(true);
                    timeAndScore.SetData(number, time, points);
                    return;
                }
            }
            
            InstantiateTimeAndScore(number, time, points);
        }

        private void InstantiateTimeAndScore(int number, float time, int points)
        {
            TimerAndScoreDisplay timeAndScore = Instantiate(timerAndScoreDisplayPrefab, this.transform);
            timeAndScore.SetData( number, time, points);
            timeAndScore.gameObject.SetActive(true);
            
            timerAndScoreDisplayPool.Add(timeAndScore);
            
            TimerAndScoreDisplay timeAndScoreGameFinishedScreen = Instantiate(timerAndScoreDisplayPrefab, gameFinishedScreenTimerAndScoreTransform);
            timeAndScoreGameFinishedScreen.SetData( number, time, points);
            timeAndScoreGameFinishedScreen.gameObject.SetActive(true);
            
            timerAndScoreGameFinishedScreenPool.Add(timeAndScoreGameFinishedScreen);
        }
    }
}
