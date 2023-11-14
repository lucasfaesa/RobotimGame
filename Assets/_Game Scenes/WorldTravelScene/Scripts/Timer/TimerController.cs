using System;
using System.Collections;
using System.Collections.Generic;
using _WorldTravelScene.Scripts.Objective;
using _WorldTravelScene.Scripts.Questions;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _WorldTravelScene.Scripts.Timer
{
    public class TimerController : MonoBehaviour
    {
        [SerializeField] private TimerScoreDataSO timerScoreData;
        [SerializeField] private ObjectivesManagerSO objectivesManager;
        [Space] 
        [SerializeField] private TextMeshProUGUI txtTimer;
        
        private float timer;
        private bool countTime;
        
        private void OnEnable()
        {
            objectivesManager.objectiveStarted += StartTimer;
            objectivesManager.objectiveCompleted += StopTimer;
        }

        private void OnDisable()
        {
            objectivesManager.objectiveStarted -= StartTimer;
            objectivesManager.objectiveCompleted -= StopTimer;
        }

        private void Start()
        {
            timerScoreData.Reset();
            txtTimer.text = "";
            txtTimer.maxVisibleCharacters = 8;
        }

        private void StartTimer(ObjectiveInfo n)
        {
            StartCoroutine(StartTimerDelayed());
        }

        private void StopTimer(ObjectiveInfo n)
        {
            Sequence sequence = DOTween.Sequence().Append(txtTimer.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutSine).SetLoops(15, LoopType.Yoyo));
            sequence.Append(txtTimer.transform.DOScale(1f,0.2f));
            
            timerScoreData.AddTimer(timer);
            countTime = false;
            timer = 0;
        }

        private void Update()
        {
            if (countTime)
            {
                timer += Time.deltaTime;
                txtTimer.text = ToCorrectTimeString(timer);
            }
        }

        private IEnumerator StartTimerDelayed()
        {
            yield return new WaitForSeconds(1f);
            
            txtTimer.gameObject.SetActive(true);
            timer = 0;
            txtTimer.text = ToCorrectTimeString(timer);
            countTime = true;
        }

        private string ToCorrectTimeString(float time)
        {
            var intTime = time;
            var minutes = intTime / 60;
            var seconds = intTime % 60;
            var fraction = time * 1000;
            fraction = fraction % 1000;
            var timeText = $"{minutes:00}:{seconds:00}:{fraction:00}";
            return timeText;
        }
    }
}
