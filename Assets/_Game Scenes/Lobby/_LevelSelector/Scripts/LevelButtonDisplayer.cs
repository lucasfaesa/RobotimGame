using System;
using System.Collections;
using System.Collections.Generic;
using _Lobby._LevelSelector.Scripts;
using API_Mestrado_Lucas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonDisplayer : MonoBehaviour
{
    [Header("SO")] 
    [SerializeField] private LevelSelectedManagerSO levelSelectedManager; 
    [Space]
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private Image innerCircle;
    [Header("Circle Background")]
    [SerializeField] private Color incompleteColor;
    [SerializeField] private Color completeColor;
    [Header("Stars")] 
    [SerializeField] private Image[] starsImages;
    [SerializeField] private Color starDefaultColor;
    [SerializeField] private Color starMissingColor;

    public LevelInfoAndScore LevelInfoAndScore;
    
    public void SetInfos(string number, LevelInfoAndScore levelInfoAndScore)
    {
        numberText.text = number;
        LevelInfoAndScore = levelInfoAndScore;

        innerCircle.color = levelInfoAndScore.completed ? completeColor : incompleteColor;

        if (levelInfoAndScore.level != null)
        {
            if (levelInfoAndScore.level.MidScoreThreshold == 0 &&
                levelInfoAndScore.level.HighScoreThreshold == 0)
            {
                foreach (var images in starsImages)
                {
                    images.gameObject.SetActive(false);
                }

                return;
            }
            
            foreach (var images in starsImages)
            {
                images.gameObject.SetActive(true);
            }
            
            int starsToBeColored = 0;
        
            if (levelInfoAndScore.score < levelInfoAndScore.level.MidScoreThreshold &&
                levelInfoAndScore.score > 0)
                starsToBeColored = 1;

            if (levelInfoAndScore.score > levelInfoAndScore.level.MidScoreThreshold &&
                levelInfoAndScore.score < levelInfoAndScore.level.HighScoreThreshold)
                starsToBeColored = 2;
        
            if (levelInfoAndScore.score > levelInfoAndScore.level.HighScoreThreshold)
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
        else
        {
            foreach (var images in starsImages)
            {
                images.gameObject.SetActive(false);
            }
        }
    }

    public void SetCurrentLevel() //usado no botão
    {
        if (levelSelectedManager.CurrentLevelInfo != null)
        {
            if (levelSelectedManager.CurrentLevelInfo.level != null &&
                levelSelectedManager.CurrentLevelInfo.level == LevelInfoAndScore.level)
                return;
            
            if (levelSelectedManager.CurrentLevelInfo.quiz != null &&
                levelSelectedManager.CurrentLevelInfo.quiz == LevelInfoAndScore.quiz)
                return;
        }
        
        levelSelectedManager.SelectLevel(LevelInfoAndScore);
    }
}
