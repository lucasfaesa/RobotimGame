using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace _MeteorShower.Scripts
{
    [CreateAssetMenu(fileName = "AnswersManager", menuName = "ScriptableObjects/MeteorShowerScene/AnswersManager")]
    public class AnswersManagerSO : ScriptableObject
    {
        [field:SerializeField] [field:ReadOnly] public List<MeteorStats> UnsolvedMeteors { get; set; } = new();
                
        public event Action correctAnswer;
        public event Action<MeteorStats> correctAnswerMeteor;
        public event Action wrongAnswer;

        public void CheckAnswer(string answer)
        {
            bool answeredCorrectly = false;

            List<MeteorStats> unsolved = new List<MeteorStats>(UnsolvedMeteors);
            
            foreach (var meteor in unsolved)
            {
                if (meteor.GetMathQuestion.questionResult == int.Parse(answer))
                {
                    if (!meteor.BeingChasedBy)
                    {
                        meteor.BeingChasedByCrystalPower();
                        correctAnswerMeteor?.Invoke(meteor);
                        answeredCorrectly = true;
                    }
                }
            }
        
            if(answeredCorrectly)
                correctAnswer?.Invoke();
            else
                wrongAnswer?.Invoke();
        }
        
        public void RemoveFromUnsolvedList(MeteorStats stats)
        {
            UnsolvedMeteors.Remove(stats);
        }

        private void OnEnable()
        {
            Reset();
        }

        private void OnDisable()
        {
            Reset();
        }

        public void Reset()
        {
            UnsolvedMeteors = new();
        }
    }
}
