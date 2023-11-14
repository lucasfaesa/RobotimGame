using System;
using System.Collections.Generic;
using System.Linq;
using API_Mestrado_Lucas;
using UnityEngine;

namespace _Game_Scenes.SpaceshipCombat.Scripts
{
    [CreateAssetMenu(fileName = "QuestionsData", menuName = "ScriptableObjects/SpaceshipCombatScene/QuestionsDataSO")]
    public class QuestionsManagerSO : ScriptableObject
    {
        public List<QuestionDTO> SpaceshipQuestions { get; set; }
    
        private static System.Random rng = new System.Random();

        public event Action questionAnswered;
        public event Action<int> correctAnswer;
        public event Action<int> wrongAnswer;
        public event Action questionShown;
        public event Action beginToShowQuestion;
        public event Action timerStarted;

        public void ShuffleQuestions()
        {
            SpaceshipQuestions = SpaceshipQuestions.OrderBy(a => rng.Next()).ToList();
        }
    
        public void QuestionAnswered()
        {
            questionAnswered?.Invoke();
        }

        public void CorrectAnswer(int totalCorrectAnswers)
        {
            correctAnswer?.Invoke(totalCorrectAnswers);
        }

        public void WrongAnswer(int totalWrongAnswers)
        {
            wrongAnswer?.Invoke(totalWrongAnswers);
        }

        public void QuestionShown()
        {
            questionShown?.Invoke();
        }

        public void BeginToShowQuestion()
        {
            beginToShowQuestion?.Invoke();
        }

        public void QuestionTimeStarted()
        {
            timerStarted?.Invoke();
        }

        public void Reset()
        {
            SpaceshipQuestions = new();
        }
    }
}
