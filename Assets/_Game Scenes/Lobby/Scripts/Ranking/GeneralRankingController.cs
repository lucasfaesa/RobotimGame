using System;
using System.Collections;
using System.Collections.Generic;
using _Lobby.Scripts.Ranking;
using API_Mestrado_Lucas;
using APIComms;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace _Game_Scenes.Lobby.Scripts.Ranking
{
    public class GeneralRankingController : MonoBehaviour
    {
        [Header("SO")] 
        [SerializeField] private ApiCommsControllerSO apiCommsController;
        [SerializeField] private PlayerDataSO playerData;
        [Header("Ranking Instantiation")]
        [SerializeField] private List<WorldRankingDisplay> worldRankingDisplayersPool;
        [Header("Feedback")] 
        [SerializeField] private GameObject loaderCircle;
        [SerializeField] private GameObject xIcon;
        
        private List<StudentAndScoreDTO> highScores;

        private Coroutine rankingRoutine;
    
        private UnityWebRequest www;
    
        void Start()
        {
            if (playerData.GuestMode) return;
            
            StartCoroutine(GenerateRanking());
        }

        private IEnumerator GenerateRanking()
        {
            foreach (var rankingUI in worldRankingDisplayersPool)
            {
                rankingUI.gameObject.SetActive(false);
            }

            if (playerData.StudentData.GroupClassId == null)
            {
                throw new Exception("Aluno sem group class");
            }

            loaderCircle.SetActive(true);
            
            using (www = UnityWebRequest.Get(ApiPaths.GENERAL_RANKING_BY_GROUPCLASS(apiCommsController.UseCloudPath) + playerData.StudentData.GroupClassId))
            {
                yield return www.SendWebRequest();

                if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.Log(www.error);
                    Debug.Log("<color=yellow>General Ranking Comms Error</color>");
                    loaderCircle.SetActive(false);
                    xIcon.SetActive(true);
                }
                else
                {
                    highScores = JsonConvert.DeserializeObject<List<StudentAndScoreDTO>>(www.downloadHandler.text);
                    Debug.Log("<color=yellow>General Ranking Comms Success</color>");
                    loaderCircle.SetActive(false);
                }
            }
            www.Dispose();
            
            SetRanking();
        }

        private void SetRanking()
        {
            if (highScores == null) return;
        
            for (int i = 0; i < highScores.Count; i++)
            {
                worldRankingDisplayersPool[i].SetInfos(highScores[i].Student.Name, highScores[i].Score.ToString());
                worldRankingDisplayersPool[i].gameObject.SetActive(true);
            }
        }
    }
}
