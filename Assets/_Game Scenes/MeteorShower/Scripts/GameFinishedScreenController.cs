using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using _Lobby._LevelSelector.Scripts;
using API_Mestrado_Lucas;
using APIComms;
using DG.Tweening;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace _MeteorShower.Scripts
{
    public class GameFinishedScreenController : MonoBehaviour
    {
        [Header("SO")]
        [SerializeField] private ApiCommsControllerSO apiCommsController;
        [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
        [SerializeField] private PointsManagerSO pointsManager;
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private ActionsTogglerSO actionsToggler;
        [Header("General")] 
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameObject content;
        [Header("Buttons")] 
        [SerializeField] private CanvasGroup buttonsCanvasGroup;
        [SerializeField] private GameObject continueButton;
        [Header("Score")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [Header("Stars")] 
        [SerializeField] private Image[] starsImages;
        [SerializeField] private Color starDefaultColor;
        [SerializeField] private Color starMissingColor;
        [Header("Ranking Instantiation")]
        [SerializeField] private List<RankingUIDisplayer> rankingUIDisplayersPool;
    
        private List<StudentAndScoreDTO> highScores;

        private Coroutine rankingRoutine;
        private UnityWebRequest www;

        private void OnEnable()
        {
            if (apiCommsController.UseComms && !playerData.GuestMode)
            {
                apiCommsController.finishedDatabaseGameEndCommunication += FadeInButtons;
                apiCommsController.finishedDatabaseGameEndCommunication += GetRanking;
            }
        }

        private void OnDisable()
        {
            if (apiCommsController.UseComms && !playerData.GuestMode)
            {
                apiCommsController.finishedDatabaseGameEndCommunication -= FadeInButtons;
                apiCommsController.finishedDatabaseGameEndCommunication -= GetRanking;
            }
                
            if (rankingRoutine != null)
            {
                www.Dispose();
                StopCoroutine(rankingRoutine);
            }
        }

        public void ShowGameFinishedScreen()
        {
            actionsToggler.EnableCursor(true);
            scoreText.text = "";
            canvasGroup.alpha = 0;
            content.SetActive(true);
            continueButton.SetActive(levelSelectedManager.CheckIfNextLevelExists());
        
            FadeInLevelCompletion();
        }

        private void FadeInLevelCompletion()
        {
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.Append(canvasGroup.DOFade(1f, 1.3f).SetEase(Ease.InOutSine));
            fadeSequence.OnComplete(() =>
            {
                float currentScore = 0;
                Sequence lerpSequence = DOTween.Sequence();
                lerpSequence.Append(DOTween.To(x => currentScore = x, currentScore, pointsManager.GetCurrentScore(false), 1f).SetEase(Ease.InOutSine));
                lerpSequence.OnUpdate(() =>
                {
                    scoreText.text = Convert.ToInt32(currentScore).ToString();
                });

                lerpSequence.OnComplete(() =>
                {
                    if(!apiCommsController.UseComms || playerData.GuestMode)
                        FadeInButtons();
                });
            });
            
            StartCoroutine(ColorStars());
        }

        private IEnumerator ColorStars()
        {
            yield return new WaitForSeconds(1.1f);
            
            int starsToBeColored = 0;

            if (levelSelectedManager.CurrentLevelInfo == null)
            {
                Debug.Log("current mid score null");
                yield break;
            }
            if(levelSelectedManager.CurrentLevelInfo.level == null)
                Debug.Log("current level info null");

            if (levelSelectedManager.CurrentLevelInfo.quiz != null)
            {
                foreach (var image in starsImages)
                {
                    image.gameObject.SetActive(false);
                }
                yield break;
            }
            
            if (levelSelectedManager.CurrentLevelInfo.level.MidScoreThreshold == 0 &&
                levelSelectedManager.CurrentLevelInfo.level.HighScoreThreshold == 0)
            {
                foreach (var image in starsImages)
                {
                    image.gameObject.SetActive(false);
                }
                yield break;
            }
            
            if (pointsManager.GetCurrentScore(false) < levelSelectedManager.CurrentLevelInfo.level.MidScoreThreshold &&
                pointsManager.GetCurrentScore(false) > 0)
                starsToBeColored = 1;

            if (pointsManager.GetCurrentScore(false) > levelSelectedManager.CurrentLevelInfo.level.MidScoreThreshold &&
                pointsManager.GetCurrentScore(false) < levelSelectedManager.CurrentLevelInfo.level.HighScoreThreshold)
                starsToBeColored = 2;
        
            if (pointsManager.GetCurrentScore(false) > levelSelectedManager.CurrentLevelInfo.level.HighScoreThreshold)
                starsToBeColored = 3;

            foreach (var star in starsImages)
            {
                star.color = starMissingColor;
            }

            for (int i = 0; i < starsToBeColored; i++)
            {
                starsImages[i].color = starDefaultColor;
            }
        }

        private void FadeInButtons()
        {
            buttonsCanvasGroup.alpha = 0;
            buttonsCanvasGroup.gameObject.SetActive(true);
        
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.Append(buttonsCanvasGroup.DOFade(1f, 1.3f).SetEase(Ease.InOutSine));
        }

        private void GetRanking()
        {
            rankingRoutine = StartCoroutine(GenerateRanking(levelSelectedManager.CurrentLevelInfo, playerData.StudentData));
        }
        
        private IEnumerator GenerateRanking(LevelInfoAndScore levelInfo, StudentDTO student)
        {
            foreach (var rankingUI in rankingUIDisplayersPool)
            {
                rankingUI.gameObject.SetActive(false);
            }

            if (student.GroupClassId == null)
            {
                throw new Exception("Aluno sem group class");
            }
        
            var groupClassAndLevel = new GroupClassLevelIdAndQuizIdDTO() { GroupClassId = student.GroupClassId.Value, LevelId = levelInfo.level?.Id, QuizId = levelInfo.quiz?.Id};
        
            using (www = UnityWebRequest.Post(ApiPaths.RANKING_BY_GROUPCLASS_AND_LEVELID_OR_QUIZID(apiCommsController.UseCloudPath), "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(groupClassAndLevel));
                www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.Log(www.error);
                    Debug.Log("<color=yellow>Ranking Comms Error</color>");
                }
                else
                {
                    highScores = JsonConvert.DeserializeObject<List<StudentAndScoreDTO>>(www.downloadHandler.text);
                    Debug.Log("<color=yellow>Ranking Comms Success</color>");
                }
            
                www.Dispose();
            }
        
            SetRanking();
        }

        private void SetRanking()
        {
            if (highScores == null) return;
        
            for (int i = 0; i < highScores.Count; i++)
            {
                rankingUIDisplayersPool[i].SetInfos(highScores[i].Student.Name, highScores[i].Score.ToString());
                rankingUIDisplayersPool[i].gameObject.SetActive(true);
            }
        }
    }
}
