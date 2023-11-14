using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using _Lobby._LevelSelector.Scripts;
using API_Mestrado_Lucas;
using APIComms;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class RankingInstantiator : MonoBehaviour
{
    [Header("SO")] 
    [SerializeField] private ApiCommsControllerSO apiCommsController;
    [SerializeField] private PlayerDataSO playerData;
    [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
    [Header("Ranking Instantiation")]
    [SerializeField] private List<RankingUIDisplayer> rankingUIDisplayersPool;
    [Header("Animation")] 
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float shownAnchoredXPos;
    [SerializeField] private float hiddenAnchoredXPos;
    [Header("Feedback")] 
    [SerializeField] private GameObject loaderCircle;
    [SerializeField] private GameObject xIcon;
    
    
    private List<StudentAndScoreDTO> highScores;

    private Coroutine rankingRoutine;
    
    private UnityWebRequest www;

    private bool shown;
    
    private void OnEnable()
    {
        levelSelectedManager.levelSelected += InstantiateRanking;
    }

    private void OnDisable()
    {
        levelSelectedManager.levelSelected -= InstantiateRanking;
        shown = false;
        rectTransform.anchoredPosition = new Vector2(hiddenAnchoredXPos, rectTransform.anchoredPosition.y);
    }

    private void InstantiateRanking(LevelInfoAndScore levelInfoAndScore)
    {
        if (!apiCommsController.UseComms || playerData.GuestMode) return;
        
        if (rankingRoutine != null)
        {
            highScores = null;
            www.Dispose();
            StopCoroutine(rankingRoutine);
        }
            
        if (!shown)
        {
            float newXPos = 0;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(x => newXPos = x, rectTransform.anchoredPosition.x, shownAnchoredXPos, 0.3f).SetEase(Ease.InOutSine));
            sequence.OnUpdate(() =>
            {
                rectTransform.anchoredPosition = new Vector2(newXPos, rectTransform.anchoredPosition.y);
            });
        }
        
        rankingRoutine = StartCoroutine(GenerateRanking(levelInfoAndScore, playerData.StudentData));
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
        
        loaderCircle.SetActive(true);
        
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
                loaderCircle.SetActive(false);
                xIcon.SetActive(true);
            }
            else
            {
                highScores = JsonConvert.DeserializeObject<List<StudentAndScoreDTO>>(www.downloadHandler.text);
                Debug.Log("<color=yellow>Ranking Comms Success</color>");
                loaderCircle.SetActive(false);
            }
            
            
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
