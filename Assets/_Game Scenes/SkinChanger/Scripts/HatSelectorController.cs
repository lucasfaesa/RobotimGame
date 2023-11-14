using System;
using System.Collections;
using System.Collections.Generic;
using API_Mestrado_Lucas;
using APIComms;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace _Game_Scenes.SkinChanger.Scripts
{
    public class HatSelectorController : MonoBehaviour
    {
        [SerializeField] private ApiCommsControllerSO apiCommsController;
        [SerializeField] private PlayerDataSO playerDataSo;
        [Space]
        [SerializeField] private TextMeshProUGUI hatNameText;
        [SerializeField] private TextMeshProUGUI hatValueText;
        [SerializeField] private TextMeshProUGUI playerTotalScoreText;
        [Space] 
        [SerializeField] private GameObject lockIcon;
        [Space]
        [SerializeField] private HatManagerSO hatManagerSo;
        [SerializeField] private Transform hatTransformReference;

        private List<GameObject> hatsList = new();
        private int hatIndex;
        private int playerPoints;
        
        void Start()
        {
            StartCoroutine(GetPlayerTotalScore());
            InstantiateHats();
        }

        private IEnumerator GetPlayerTotalScore()
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(ApiPaths.STUDENT_TOTAL_SCORE(apiCommsController.UseCloudPath) + playerDataSo.StudentData.Id);
        
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
       
            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(webRequest.error);
                Debug.LogError("max score get error");
                playerTotalScoreText.text = "Seus pontos: x";
            }
            else
            {
                playerPoints = Convert.ToInt32(JsonConvert.DeserializeObject<string>(webRequest.downloadHandler.text));
                playerTotalScoreText.text = $"Seus pontos: {playerPoints}";
                Debug.Log("max score get success");
            }

            webRequest.Dispose();
        }
        
        private void InstantiateHats()
        {
            int count = 0;
            hatIndex = 0;

            foreach (var hatDataSo in hatManagerSo.hatDataSoList)
            {
                GameObject hat = Instantiate(hatDataSo.hatPrefab, hatTransformReference);
                
                if (hatManagerSo.CurrentSelectedHat != null && hatDataSo.HatName == hatManagerSo.CurrentSelectedHat.HatName)
                {
                    hatNameText.text = $"{hatDataSo.HatName}";
                    hatValueText.text = hatDataSo.HatValue > 0 ? hatDataSo.HatValue + " Pontos" : "";
                    hat.SetActive(true);
                    hatIndex = count;
                }
                else
                {
                    hat.SetActive(false);
                }
                
                hatsList.Add(hat);
                count++;
            }
        }

        public void CicleThroughHats(bool forward)
        {
            if (forward)
                hatIndex++;
            else
                hatIndex--;
            
            if (hatIndex > hatsList.Count - 1)
                hatIndex = 0;

            if (hatIndex < 0)
                hatIndex = hatsList.Count - 1;

            hatsList.ForEach(x=>x.SetActive(false));
            hatsList[hatIndex].SetActive(true);
            
            var currentHat = hatManagerSo.hatDataSoList[hatIndex];
            hatNameText.text = $"{currentHat.HatName}";
            hatValueText.text = currentHat.HatValue > 0 ? currentHat.HatValue + " Pontos" : "";

            if (currentHat.HatValue > playerPoints)
            {
                lockIcon.SetActive(true);
            }
            else
            {
                lockIcon.SetActive(false);
                hatManagerSo.CurrentSelectedHat = currentHat;
            }
                
        }
    }
}
