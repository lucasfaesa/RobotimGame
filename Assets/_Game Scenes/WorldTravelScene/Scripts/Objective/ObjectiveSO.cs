using System.Collections.Generic;
using _Lobby._LevelSelector.Scripts;
using _WorldTravelScene.Scripts.Countries;
using UnityEngine;

namespace _WorldTravelScene.Scripts.Questions
{
    [CreateAssetMenu(fileName = "ObjectiveSO", menuName = "ScriptableObjects/WorldTravelScene/ObjectiveSO")]
    public class ObjectiveSO : ScriptableObject
    {
        [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
        
        [field:Space]
        [field:TextArea(7,15)] [field:SerializeField] public string Question { get; set; }
        [field:TextArea(2,4)] [field:SerializeField] public string Objective { get; set; }
        
        [field:Space]
        [field:SerializeField] public List<CountrySO> CorrectCountries { get; set; }
        /// <summary>
        /// If the suggested list is empty or does not have the correct amount, a random country will be get instead to complement
        /// </summary>
        [field:SerializeField] public List<CountrySO> SuggestedIncorrectCountries { get; set; }

        private int countriesQuantityToCompleteMission = 1;

        [SerializeField] private int useDebugAmount = 0;
        
        public ObjectiveInfo GetObjectiveInfos()
        {
            if (useDebugAmount != 0)
                countriesQuantityToCompleteMission = useDebugAmount;
            else
            {
                this.countriesQuantityToCompleteMission = levelSelectedManager.CurrentLevelInfo.level.Difficulty;
            }
            countriesQuantityToCompleteMission = Mathf.Clamp(countriesQuantityToCompleteMission, 1, CorrectCountries.Count);

            var replacedObjectiveStrings = Objective.Replace("#N#", countriesQuantityToCompleteMission.ToString());
            replacedObjectiveStrings =
                replacedObjectiveStrings.Replace("#PAIS#", countriesQuantityToCompleteMission > 1 ? "países" : "país");
            replacedObjectiveStrings =
                replacedObjectiveStrings.Replace("#MEMBRO#", countriesQuantityToCompleteMission > 1 ? "MEMBROS" : "MEMBRO");

            return new ObjectiveInfo(Question, replacedObjectiveStrings, CorrectCountries, SuggestedIncorrectCountries,
                countriesQuantityToCompleteMission);
        }
        
    }
    
    public class ObjectiveInfo
    {
        public string Question { get; set; }
        public string Objective { get; set; }
        public List<CountrySO> CorrectCountries { get; set; }
        public List<CountrySO> SuggestedIncorrectCountries { get; set; }
        public int CountriesQuantityToCompleteMission { get; set; }
        public int CountriesDelivered { get; set; } = 0; //países na qual foram entregues caixas, na criação do objeto sempre irá ser 0 mesmo

        public ObjectiveInfo(string quest, string objec, List<CountrySO> corrCountries,
            List<CountrySO> suggIncCountries, int neededToComplete)
        {
            Question = quest;
            Objective = objec;
            CorrectCountries = corrCountries;
            SuggestedIncorrectCountries = suggIncCountries;
            CountriesQuantityToCompleteMission = neededToComplete;
        }
    }
}
