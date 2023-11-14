using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Lobby._LevelSelector.Scripts
{
    public class SubjectThemeUIDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelName;
        [SerializeField] private LevelButtonDisplayer levelButtonPrefab;
        [SerializeField] private List<LevelButtonDisplayer> levelButtonsPool;
    
        public void SetInformations(string name, List<LevelInfoAndScore> levelsAndScores)
        {
            levelName.text = name;

            for (int i = 0; i < levelsAndScores.Count; i++)
            {
                levelButtonsPool[i].SetInfos((i+1).ToString(), levelsAndScores[i]);
                levelButtonsPool[i].gameObject.SetActive(true);
            }
        }
        
        public void SetInformations(string name, LevelInfoAndScore levelsAndScores)
        {
            levelName.text = name;

            levelButtonsPool[0].SetInfos("1", levelsAndScores);
            levelButtonsPool[0].gameObject.SetActive(true);
            
        }

        private void OnDisable()
        {
            levelButtonsPool.ForEach(x=>x.gameObject.SetActive(false));
        }
    }
}
