using System;
using System.Collections.Generic;
using System.Linq;
using _MeteorShower.Scripts;
using _WorldTravelScene.Scripts.Objective;
using _WorldTravelScene.Scripts.Questions;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _WorldTravelScene.Scripts.Countries
{
    public class CountriesController : MonoBehaviour
    {
        [SerializeField] private ObjectivesManagerSO objectivesManagerSo;
        [SerializeField] private PointsManagerSO pointsManager;
        [SerializeField] private CountriesManagerSO countriesManager;
        [Space] 
        [SerializeField] private List<CountryBehavior> countriesObjects;

        private List<CountrySO> correctCountriesToShow = new();
        private List<CountrySO> countriesIncorrectToShow = new();

        private void OnEnable()
        {
            countriesManager.targetHit += TargetHit;
            objectivesManagerSo.newObjectiveGot += SetCountriesForTheObjective;
            objectivesManagerSo.objectiveStarted += ShowCountries;
            objectivesManagerSo.objectiveCompleted += HideAllCountries;
        }

        private void OnDisable()
        {
            countriesManager.targetHit -= TargetHit;
            objectivesManagerSo.newObjectiveGot -= SetCountriesForTheObjective;
            objectivesManagerSo.objectiveStarted -= ShowCountries;
            objectivesManagerSo.objectiveCompleted -= HideAllCountries;
        }

        private void TargetHit(CountrySO country, TargetDivision.TargetDivisionType targetDivType, int points)
        {
            if (objectivesManagerSo.CurrentObjective.CorrectCountries.Contains(country))
            {
                countriesManager.TargetHitCorrectly(country, targetDivType, points);
                objectivesManagerSo.DeliveredToCorrectCountry();
                pointsManager.AddPoints(points);
            }
            else
            {
                countriesManager.TargetHitIncorrectly(country, targetDivType, points);
            }
        }

        private void SetCountriesForTheObjective(ObjectiveInfo objectiveInfo, ObjectivesController.LevelDifficulty levelDifficulty)
        {
            correctCountriesToShow = new();
            countriesIncorrectToShow = new();
                                       
            if (objectiveInfo.CorrectCountries.Count == 0)
                throw new Exception("Sem países corretos");

            List<CountrySO> allCountries = new();
            foreach (var country in countriesObjects) { allCountries.Add(country.GetCountry); } //adding allCountries to list

            List<CountrySO> suggestedIncorrectCountries = new(objectiveInfo.SuggestedIncorrectCountries); //adding suggestedIncorrectCountries

            int correctCountriesAmountToDisplay = Mathf.Clamp(objectiveInfo.CountriesQuantityToCompleteMission + (int)levelDifficulty, 
                                                                                    1, objectiveInfo.CorrectCountries.Count);
            int countriesIncorrectAmountToDisplay = correctCountriesAmountToDisplay;
            
            //adicionando países errados
            List<CountrySO> remainingCountries = new(allCountries);
            
            foreach (var correctCountries in objectiveInfo.CorrectCountries)
            {
                remainingCountries.Remove(correctCountries);
            }

            if (suggestedIncorrectCountries.Count > 0)
            {
                foreach (var incorrectCountry in suggestedIncorrectCountries)
                {
                    remainingCountries.Remove(incorrectCountry);
                }   
            }
            
            if (suggestedIncorrectCountries.Count < countriesIncorrectAmountToDisplay)
            {
                int remainingIncorrectCountriesQuantity = countriesIncorrectAmountToDisplay - suggestedIncorrectCountries.Count;
                
                if(suggestedIncorrectCountries.Count != 0)
                    countriesIncorrectToShow.AddRange(suggestedIncorrectCountries);
                
                for (int i = 0; i < remainingIncorrectCountriesQuantity; i++)
                {
                    int random = Random.Range(0, remainingCountries.Count);
                
                    countriesIncorrectToShow.Add(remainingCountries[random]);
//                    Debug.Log("wrong Country Added: " + remainingCountries[random].CountryName);
                    remainingCountries.RemoveAt(random);
                }
            }
            else
            {
                countriesIncorrectToShow.AddRange(suggestedIncorrectCountries.Take(countriesIncorrectAmountToDisplay));
         //       Debug.Log("wrong Country Added: " + countriesIncorrectToShow);
            }
                
            
            //adicionando países certos
            correctCountriesToShow = new();
            if (objectiveInfo.CountriesQuantityToCompleteMission == objectiveInfo.CorrectCountries.Count)
            {
                correctCountriesToShow = new(objectiveInfo.CorrectCountries);
            }
            else
            {
                //get random correct countries
                List<int> randomCountriesIndex = new();

                while (randomCountriesIndex.Count != correctCountriesAmountToDisplay)
                {
                    int randomIndex = Random.Range(0, objectiveInfo.CorrectCountries.Count);

                    if (!randomCountriesIndex.Contains(randomIndex))
                    {
                        randomCountriesIndex.Add(randomIndex);
                        correctCountriesToShow.Add(objectiveInfo.CorrectCountries[randomIndex]);
//                        Debug.Log("Correct Country Added: " + objectiveInfo.CorrectCountries[randomNumber].CountryName);
                    }
                }
            }
            List<CountrySO> countriesActivated = new();
            countriesActivated.AddRange(correctCountriesToShow);
            countriesActivated.AddRange(countriesIncorrectToShow);
            countriesManager.ActivatedCountries(countriesActivated);
        }

        private void ShowCountries(ObjectiveInfo objective)
        {
            //ativando paises corretos
            foreach (var country in correctCountriesToShow)
            {
                countriesObjects.Find(x=>x.GetCountry == country).Toggle(true);
            }
            
            //ativando paises incorretos
            foreach (var country in countriesIncorrectToShow)
            {
                countriesObjects.Find(x=>x.GetCountry == country).Toggle(true);
            }
        }
        
        private void HideAllCountries()
        {
            foreach (var country in countriesObjects) { country.Toggle(false); }
        }
        
        private void HideAllCountries(ObjectiveInfo n)
        {
            foreach (var country in countriesObjects) { country.Toggle(false); }
        }
    }
}
