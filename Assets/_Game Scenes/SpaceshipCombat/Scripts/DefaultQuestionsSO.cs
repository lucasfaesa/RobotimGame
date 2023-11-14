using System;
using System.Collections.Generic;
using System.Linq;
using API_Mestrado_Lucas;
using Mapster;
using UnityEngine;

namespace _Game_Scenes.SpaceshipCombat.Scripts
{
    [CreateAssetMenu(fileName = "DefaultQuestions",
        menuName = "ScriptableObjects/SpaceshipCombatScene/DefaultQuestionsSO")]
    public class DefaultQuestionsSO : ScriptableObject
    {
        [field: SerializeField] public List<DefaultQuestions> DefaultQuestions { get; set; } = new();

        public List<QuestionDTO> GetQuestionsBySubjectId(int subjectId)
        {
            return DefaultQuestions.First(x => x.SubjectId == subjectId).Questions.Adapt<List<QuestionDTO>>();
        }
    }
}

    [System.Serializable]
    public record DefaultQuestions
    {
        [field:SerializeField] public int SubjectId { get; set; }
        [field:SerializeField] public List<QuestionHelper> Questions { get; set; }
        
        public DefaultQuestions(int id, List<QuestionHelper> quest) => (SubjectId, Questions) = (id, quest);
    }

    [System.Serializable]
    public record QuestionHelper
    {
        [field:SerializeField] public string QuestionTitle { get; set; }
        [field:SerializeField] public float QuestionTimeLimit { get; set; }
        [field:SerializeField] public List<QuestionAnswerHelper> QuestionAnswers { get; set; }
    }

    [System.Serializable]
    public record QuestionAnswerHelper
    {
        [field:SerializeField] public string AnswerString { get; set; }
        [field:SerializeField] public bool IsCorrectAnswer { get; set; }
    }

