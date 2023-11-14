using System;
using System.Collections;
using _WorldTravelScene.Scripts.Objective;
using _WorldTravelScene.Scripts.Questions;
using TMPro;
using UnityEngine;

namespace _WorldTravelScene.Scripts.General_UI
{
    public class DeliveredInfo : MonoBehaviour
    {
        [SerializeField] private ObjectivesManagerSO objectivesManager;
        [Space] 
        [SerializeField] private TextMeshProUGUI deliveredText;
        [SerializeField] private float delayBetweenWords = 0.1f;
        
        private void OnEnable() { objectivesManager.objectiveStarted += ShowDeliverText; objectivesManager.correctCountryDelivered += UpdateDeliveryQuantity;}

        private void OnDisable() { objectivesManager.objectiveStarted -= ShowDeliverText; objectivesManager.correctCountryDelivered -= UpdateDeliveryQuantity;}

        private void Start()
        {
            deliveredText.text = "";
        }

        private void UpdateDeliveryQuantity(ObjectiveInfo info)
        {
            deliveredText.text = "Suprimentos entregues: " + info.CountriesDelivered + "/" + info.CountriesQuantityToCompleteMission;
        }
        
          private void ShowDeliverText(ObjectiveInfo currentObjective)
        {
            deliveredText.text = "Suprimentos entregues: " +  currentObjective.CountriesDelivered + "/" + currentObjective.CountriesQuantityToCompleteMission;
            
            deliveredText.maxVisibleCharacters = 0;
            
            StartCoroutine(TypewriterEffect(deliveredText.text));
        }

        private IEnumerator TypewriterEffect(string delivered)
        {
            deliveredText.ForceMeshUpdate();
            
            TMP_TextInfo deliveredTextInfo = deliveredText.textInfo;

            int totalVisibleCharacters = deliveredTextInfo.characterCount +1;
            
            int visibleCount = 1;

            while (visibleCount != totalVisibleCharacters)
            {
                yield return new WaitForSeconds(delayBetweenWords);
                
                deliveredText.maxVisibleCharacters = visibleCount;
                
                visibleCount += 1;
            }
        }
    }
}
