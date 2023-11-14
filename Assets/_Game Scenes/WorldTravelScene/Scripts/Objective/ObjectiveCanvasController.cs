using System;
using System.Collections;
using System.Linq;
using System.Text;
using _WorldTravelScene.Scripts.Countries;
using _WorldTravelScene.Scripts.Objective;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

namespace _WorldTravelScene.Scripts.Questions
{
    public class ObjectiveCanvasController : MonoBehaviour
    {
        [SerializeField] private CountriesManagerSO countriesManager;
        [SerializeField] private ObjectivesManagerSO objectivesManager;
        [Space] 
        [SerializeField] private GameObject questionCanvas;
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private TextMeshProUGUI objectiveText;
        [SerializeField] private GameObject availableCountriesCanvas;
        [SerializeField] private TextMeshProUGUI availableCountriesTxt;
        [Space]
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
        [Space]
        [SerializeField] private TypeText textTypeQuestion;
        [SerializeField] private TypeText textTypeObjective;

        private string questionString;
        private string objectiveString;
        
        private void OnEnable()
        {
            objectivesManager.newObjectiveGot += ShowObjectiveCanvas;
            objectivesManager.objectiveCompleted += HideObjectiveCanvas;
        }

        private void OnDisable()
        {
            objectivesManager.newObjectiveGot -= ShowObjectiveCanvas;
            objectivesManager.objectiveCompleted -= HideObjectiveCanvas;
        }
        

        private void ShowObjectiveCanvas(ObjectiveInfo currentObjective, ObjectivesController.LevelDifficulty n)
        {
            questionString = currentObjective.Question;
            objectiveString = currentObjective.Objective;

            //questionText.maxVisibleCharacters = 0;
            //objectiveText.maxVisibleCharacters = 0;

            UpdateCanvas();
            
            
            questionCanvas.SetActive(true);
            questionCanvas.transform.localScale = Vector3.zero;
            questionCanvas.transform.DOScale(1f, 0.3f).SetEase(Ease.InOutSine);
            
            float currentChars = 0;
            int ok = questionString.Length;
            
            /*Sequence typeSeq = DOTween.Sequence();
            typeSeq.Append(DOTween.To(x => currentChars = x, 0, ok, 20f).SetEase(Ease.Linear));
            typeSeq.OnUpdate(() =>
            {
                Debug.Log(currentChars);
                questionText.text = questionString[..(int)currentChars];
            });*/
            
            textTypeQuestion.SetText(questionString);
        }

        public void QuestionEndedTyping()
        {
            textTypeObjective.SetText(objectiveString);
        }

        public void ObjectiveEndedTyping()
        {
            StartCoroutine(StartObjective());
        }
        
        private void HideObjectiveCanvas(ObjectiveInfo currentObjective)
        {
            Sequence hideSequence = DOTween.Sequence().Append( questionCanvas.transform.DOScale(0f, 0.3f).SetEase(Ease.InOutSine));
            hideSequence.OnComplete(() =>
            {
                questionText.text = "";
                objectiveText.text = "";
                availableCountriesTxt.text = "";
                questionCanvas.SetActive(false);
                availableCountriesCanvas.SetActive(false);
            });
        }

        private IEnumerator StartObjective()
        {
            
            
            
            
            
            yield return new WaitForSeconds(0.5f);
            objectivesManager.ObjectiveStarted();

            availableCountriesTxt.text = "Países disponíveis essa rodada:";
            Random rng = new Random();
            var shuffledCountryList = countriesManager.CountriesActive.OrderBy(a => rng.Next()).ToList();
            
            foreach (var country in shuffledCountryList)
            {
                availableCountriesTxt.text += "\n" + "- " + country.CountryName;
            }
            yield return null;
            availableCountriesCanvas.SetActive(true);
        }

        private void UpdateCanvas()
        {
            StartCoroutine(UpdateCanvasRoutine());
        }
        
        private IEnumerator UpdateCanvasRoutine()
        {
            Canvas.ForceUpdateCanvases();
            verticalLayoutGroup.enabled = false;
            yield return new WaitForEndOfFrame();
            
            Canvas.ForceUpdateCanvases();
            verticalLayoutGroup.enabled = true;
            Canvas.ForceUpdateCanvases();
        }
    }
}
