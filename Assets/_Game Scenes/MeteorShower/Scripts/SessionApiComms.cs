using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using _Lobby._LevelSelector.Scripts;
using _Village.Scripts;
using API_Mestrado_Lucas;
using APIComms;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

namespace _MeteorShower.Scripts
{
    public class SessionApiComms : MonoBehaviour
    {
        [Header("SO")] 
        [SerializeField] private ApiCommsControllerSO apiCommsController;
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
        [SerializeField] private PointsManagerSO pointsManager;
        [SerializeField] private ElapsedTimeManagerSO elapsedTimeManager;
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private LevelCompletionSO levelCompletion;
        
        private bool finishedLevel;
        private SessionDTO initialSentSession;
        private Coroutine startSessionRoutine;
        
        private void OnEnable()
        {
            gameManager.gameEnded += SendCompleteSession;
            gameManager.gameEnded += SaveDebugScore;
        }

        private void OnDisable()
        {
            gameManager.gameEnded -= SendCompleteSession;
            gameManager.gameEnded -= SaveDebugScore;
        }

        private void Start()
        {
            SendStartSession();
        }

        private void SendStartSession()
        {
            if (apiCommsController.UseComms && !playerData.GuestMode)
                startSessionRoutine = StartCoroutine(SendStartSessionToDatabase());
        }

        private void SendCompleteSession()
        {
            if (apiCommsController.UseComms && !playerData.GuestMode)
                StartCoroutine(UpdateSessionOnDatabase());
        }

        private void SaveDebugScore()
        {
            #if UNITY_EDITOR
                if (!apiCommsController.SaveScoreOnTxt) return;

                string filePath = "";
                
                if(levelSelectedManager.CurrentLevelInfo.level != null)
                    filePath = Application.persistentDataPath + @"\" + levelSelectedManager.CurrentLevelInfo.subjectThemeDto.Name + levelSelectedManager.CurrentLevelInfo.level.Id + ".json";
                if(levelSelectedManager.CurrentLevelInfo.quiz != null)
                    filePath = Application.persistentDataPath + @"\" + "Quizes" + @"\" + levelSelectedManager.CurrentLevelInfo.quiz.Name + ".json";
                
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath,"[]");
                }
                
                string scoresText = File.ReadAllText(filePath);

                if (scoresText != "")
                {
                    List<int> scoresList = JsonConvert.DeserializeObject<List<int>>(scoresText);
                    
                    scoresList.Add(pointsManager.GetCurrentScore(false));

                    var serializedScore = JsonConvert.SerializeObject(scoresList);
                    
                    File.WriteAllText(filePath,serializedScore);
                    
                    Debug.Log("<color=yellow>Salvo com sucesso no persistent data path</color>");
                }
            #endif
        }

        private IEnumerator SendStartSessionToDatabase()
        {
            SessionDTO sessionDto = new SessionDTO
            {
                StudentId = playerData.StudentData.Id,
                LevelId = levelSelectedManager.CurrentLevelInfo.level?.Id,
                QuizId = levelSelectedManager.CurrentLevelInfo.quiz?.Id,
                ElapsedTime = 0,
                FinishedDate = null,
                Finished = null,
                Score = 0,
                PlayedDate = DateTime.Now,
            };
            
            apiCommsController.StartedDatabaseGameStartCommunication();
            
            using (UnityWebRequest www = UnityWebRequest.Post(ApiPaths.POST_SESSION_URL(apiCommsController.UseCloudPath), "POST"))
            {
                www.disposeDownloadHandlerOnDispose = true;
                www.disposeUploadHandlerOnDispose = true;
                
                byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sessionDto));
                www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.Log(www.error);
                    apiCommsController.GameStartDataSentError();
                    www.downloadHandler.Dispose();
                    www.uploadHandler.Dispose();
                    Debug.Log("<color=yellow>Session sent error</color>");
                }
                else
                {
                    initialSentSession = JsonConvert.DeserializeObject<SessionDTO>(www.downloadHandler.text);
                    apiCommsController.GameStartDataSentSuccessfully();
                    www.downloadHandler.Dispose();
                    www.uploadHandler.Dispose();
                    Debug.Log("<color=yellow>Session sent successfully</color>");
                }
                www.Dispose();
                Debug.Log("<color=yellow>disposed</color>");
                
            }
            
            apiCommsController.FinishedDatabaseGameStartCommunication();
            
            StopCoroutine(startSessionRoutine);
        }
        
        private IEnumerator UpdateSessionOnDatabase()
        {
            if (initialSentSession == null)
            {
                Debug.Log("<color=yellow>Could not update existent session because start session was not sent</color>");
                apiCommsController.FinishedDatabaseEndGameCommunication();
                yield break;
            }
            
            initialSentSession.ElapsedTime = elapsedTimeManager.GetGameElapsedTime();
            initialSentSession.Finished = levelCompletion.LevelCompleted;
            initialSentSession.FinishedDate = levelCompletion.LevelCompleted ? DateTime.Now : null;
            initialSentSession.Score = pointsManager.GetCurrentScore(false);
            
            apiCommsController.StartedDatabaseEndGameCommunication();
            
            using (UnityWebRequest www = UnityWebRequest.Post(ApiPaths.POST_UPDATE_SESSION_URL(apiCommsController.UseCloudPath), "POST"))
            {
                www.disposeDownloadHandlerOnDispose = true;
                www.disposeUploadHandlerOnDispose = true;
                
                byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(initialSentSession));
                www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.Log(www.error);
                    apiCommsController.GameEndDataSentError();
                    Debug.Log("<color=yellow>Session sent error</color>");
                }
                else
                {
                    apiCommsController.GameEndDataSentSuccessfully();
                    Debug.Log("<color=yellow>Session sent successfully</color>");
                }
                
                www.downloadHandler.Dispose();
                www.uploadHandler.Dispose();
                www.Dispose();
            }

            
            apiCommsController.FinishedDatabaseEndGameCommunication();
        }
    }
}

