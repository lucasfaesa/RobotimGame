using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _Lobby._LevelSelector.Scripts;
using _MeteorShower.Scripts;
using _SpaceshipCombat.Scripts;
using _Village.Scripts;
using API_Mestrado_Lucas;
using APIComms;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace _Game_Scenes.SpaceshipCombat.Scripts
{
    public class QuestionsController : MonoBehaviour
    {
        [Header("SO")] 
        [SerializeField] private ApiCommsControllerSO apiCommsController;
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
        [SerializeField] private QuestionsManagerSO questionsManager;
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private PointsManagerSO pointsManager;
        [SerializeField] private LevelCompletionSO levelCompletion;
        [SerializeField] private DefaultQuestionsSO defaultQuestions;
        [Space]
        [SerializeField] private PlayerSpaceshipTriggerInteractor playerSpaceshipTriggerInteractor;
        [Header("Question Canvas")] 
        [SerializeField] private QuestionsDisplay questionsDisplay;
        [Header("Desks Answers")] 
        [SerializeField] private List<DeskAnswerHolder> deskAnswersHolders;

        private QuestionDTO currentQuestion;
        private List<StudentWrongAnswersDTO> studentWrongAnswers = new();
        
        private DeskAnswerHolder currentAnswerOption;
        private bool canAnswer;
        private bool questionShown;

        private int contQuestions = 0;
        private int answersNeededToFinish = 5;
        private int maxErrors = 5;

        private int totalErrors = 0;
        private int totalCorrectAnswers = 0;

        private static System.Random rng = new System.Random();

        private Coroutine questionsRoutine;
        
        //score calculation variables
        private int baseQuestionsPointsValue;
        private int baseQuestionsPenaltyPointsValue;
        private float questionStartTime;
        private float questionAnsweredTime;
        private int wrongQuestionsAnsweredCount;
        //private int rightAnswersStreak;

        private int lastAddedPoints = 0;
        
        private void OnEnable()
        {
            playerSpaceshipTriggerInteractor.playerCollidingWithDesk += InsideAnswerArea;
            questionsDisplay.timesUp += TimesUp;
            questionsManager.timerStarted += QuestionTimerStarted;
            questionsManager.questionShown += QuestionShown;
        }

        private void OnDisable()
        {
            playerSpaceshipTriggerInteractor.playerCollidingWithDesk -= InsideAnswerArea;
            questionsDisplay.timesUp -= TimesUp;
            questionsManager.timerStarted -= QuestionTimerStarted;
            questionsManager.questionShown -= QuestionShown;
        }
        private void Start()
        {
            questionsManager.Reset();

            if (apiCommsController.UseComms && !playerData.GuestMode)
                questionsRoutine = StartCoroutine(GetApiQuestions());
            else
            {
                questionsManager.SpaceshipQuestions = defaultQuestions.GetQuestionsBySubjectId(levelSelectedManager.CurrentLevelInfo.subjectThemeDto.Id);
                questionsManager.ShuffleQuestions();    
                StartCoroutine(GetQuestionAfterDelay(2f));
            }
        }
        
        private void QuestionShown()
        {
            questionShown = true;
        }
    
        private void QuestionTimerStarted()
        {
            questionStartTime = Time.time;
        }

        private IEnumerator GetApiQuestions()
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(ApiPaths.GET_QUIZ_BY_ID(apiCommsController.UseCloudPath) + levelSelectedManager.CurrentLevelInfo.quiz.Id);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
       
            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(webRequest.error);
                Debug.LogError("<color=yellow> Quiz get error </color>");
            }
            else
            {
                var quiz = JsonConvert.DeserializeObject<QuizDTO>(webRequest.downloadHandler.text);

                questionsManager.SpaceshipQuestions = quiz.Questions.ToList();

                Debug.Log("<color=yellow> Quiz get success </color>");
            }
            questionsManager.ShuffleQuestions(); 
            StartCoroutine(GetQuestionAfterDelay(2f));
            webRequest.Dispose();
        }

        private IEnumerator GetQuestionAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SetAndShowQuestion(); 
        }
    
        private void SetAndShowQuestion()
        {
            if (contQuestions > questionsManager.SpaceshipQuestions.Count - 1)
            {
                contQuestions = 0;
                questionsManager.ShuffleQuestions(); //this will randomize questions again
            }
            
            currentQuestion = questionsManager.SpaceshipQuestions[contQuestions];
        
            //Debug.Log(currentQuestion);
        
            var randomizedAnswers = currentQuestion.QuestionAnswers.OrderBy(a => rng.Next()).ToList();
        
            for (int i = 0; i < deskAnswersHolders.Count; i++)
            {
                deskAnswersHolders[i].AssignAnswerOption(randomizedAnswers[i]);    
            }
        
            contQuestions++;
        
            questionsDisplay.ShowQuestionOnCanvas(currentQuestion, randomizedAnswers);
        
        }

        void Update()
        {
            if (canAnswer && questionShown)
            {
                if(Keyboard.current.eKey.wasPressedThisFrame)
                    AnswerQuestion(currentAnswerOption);
            }
        }

        private void InsideAnswerArea(bool status, DeskAnswerHolder answer)
        {
            canAnswer = status;
            if (status)
                currentAnswerOption = answer;
            else
                currentAnswerOption = null;
        }

        private void AnswerQuestion(DeskAnswerHolder answer)
        {
            questionsManager.QuestionAnswered();
        
            HideQuestionAndAnswers();
        
            if (answer.GetAnswer().IsCorrectAnswer)
            {
                AnsweredCorrectly();
            }
            else
            {
                AnsweredIncorrectly(answer);
            }
        }

        private void AnsweredCorrectly()
        {
            totalCorrectAnswers++;
            questionsManager.CorrectAnswer(totalCorrectAnswers);

            questionAnsweredTime = Time.time;
            CalculateQuestionPoints(true, currentQuestion.QuestionScoreValue);
        
            //rightAnswersStreak++;

            if (totalCorrectAnswers == answersNeededToFinish)
            {
                StartCoroutine(SendPlayerWrongAnswers());
                levelCompletion.LevelCompleted = true;
                gameManager.GameWon();
                gameManager.GameEnded();
            }
            else
            {
                SetupForNextQuestion();
            }
        }
    
        private void AnsweredIncorrectly(DeskAnswerHolder answer)
        {
            totalErrors++;
            questionsManager.WrongAnswer(totalErrors);
            
            studentWrongAnswers.Add(new StudentWrongAnswersDTO
            {
                StudentId = playerData.StudentData.Id,
                QuestionTitle = currentQuestion.QuestionTitle,
                QuestionCorrectAnswer = currentQuestion.QuestionAnswers.First(x=>x.IsCorrectAnswer).AnswerString,
                StudentWrongAnswer = answer is not null ? answer.GetAnswer().AnswerString : "Tempo limite excedido",
                QuizId = currentQuestion.QuizId
            });
            
            CalculateQuestionPoints(false, currentQuestion.QuestionScoreValue);
            
            wrongQuestionsAnsweredCount++;
            
            //rightAnswersStreak = 0;
        
            if (totalErrors == maxErrors)
            {
                StartCoroutine(SendPlayerWrongAnswers());
                levelCompletion.LevelCompleted = false;
                gameManager.GameLost();
                gameManager.GameEnded();
            }
            else
            {
                SetupForNextQuestion();
            }
        }

        private IEnumerator SendPlayerWrongAnswers()
        {
            if (studentWrongAnswers.Count > 0)
            {
                using (UnityWebRequest www = UnityWebRequest.Post(ApiPaths.POST_STUDENT_WRONG_QUESTIONS(apiCommsController.UseCloudPath), "POST"))
                {
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(studentWrongAnswers));
                    www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
                    www.SetRequestHeader("Content-Type", "application/json");

                    yield return www.SendWebRequest();

                    if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
                    {
                        Debug.Log(www.error);
                        Debug.Log("<color=yellow>Student Wrong Answers Comms Error</color>");
                    }
                    else
                    {
                        Debug.Log("<color=yellow>Student Wrong Answers Comms Success</color>");
                    }
            
                    www.Dispose();
                }
            }
        }

        private void CalculateQuestionPoints(bool correctAnswer, float questionScoreValue)
        {
            if (correctAnswer)
            {
                /*if (wrongQuestionsAnsweredCount > 0)
                    rightAnswersStreak = 0;*/
                
                //baseQuestionsPointsValue = Random.Range(30, 36) - wrongQuestionsAnsweredCount + (rightAnswersStreak * 2);
                Debug.Log($"Question Score Value: {currentQuestion.QuestionScoreValue}");
                baseQuestionsPointsValue = (int)questionScoreValue - (int)(wrongQuestionsAnsweredCount/2)/* + (rightAnswersStreak * 2)*/;
                //o calculo funciona da seguinte forma, se o aluno demorar mais que 50% do tempo total para responder
                //ele perde 1 ponto na questão
                //os streaks de resposta correta adicionam mais 2 pontos por resposta correta seguida até que se erre
                //****** streaks removido por hora*********
                
                var timeTakenToAnswer = questionAnsweredTime - questionStartTime;
                
                var points = baseQuestionsPointsValue - (timeTakenToAnswer > currentQuestion.QuestionTimeLimit/2 ? 1 : 0);

                lastAddedPoints = points;
                
                Debug.Log("Points: " + points);
            
                pointsManager.AddPoints(points);
            }
            else
            {
                if (lastAddedPoints == 0)
                {
                    lastAddedPoints = (int)questionsManager.SpaceshipQuestions.Max(x=>x.QuestionScoreValue);
                }
                
                baseQuestionsPenaltyPointsValue = Random.Range(1, 4) + lastAddedPoints + (wrongQuestionsAnsweredCount);
                //errar gera punição por erro, cumulativo e não reseta por acerto (o primeiro erro não gera a punição, só os proximos)
            
                pointsManager.RemovePointsButHasMinimumScore(baseQuestionsPenaltyPointsValue, 8);
            }
        }

        private void HideQuestionAndAnswers()
        {
            questionShown = false;
            canAnswer = false;
            questionsDisplay.HideQuestionCanvas();
        
            playerSpaceshipTriggerInteractor.ForceTriggerExit();
        
            foreach (var desks in deskAnswersHolders)
            {
                desks.HideAnswersOptions();
            }
        }
        
        private void SetupForNextQuestion()
        {
            StartCoroutine(GetQuestionAfterDelay(4f));
        }

        private void TimesUp()
        {
            AnsweredIncorrectly(null);
            HideQuestionAndAnswers();
            questionsManager.QuestionAnswered();
        }
    
    
    
    }
}
