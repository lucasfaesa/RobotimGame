using System;
using System.Collections.Generic;
using System.Linq;
using API_Mestrado_Lucas;
using APIComms;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Lobby._LevelSelector.Scripts
{
    [CreateAssetMenu(fileName = "LevelSelected", menuName = "ScriptableObjects/LobbyScene/LevelSelector/LevelSelected")]
    public class LevelSelectedManagerSO : ScriptableObject
    {
        private LevelInfoAndScore _currentLevelInfo;

        public LevelInfoAndScore CurrentLevelInfo
        {
            get
            {
                if (PlayerData.GuestMode)
                {
                    #if UNITY_EDITOR
                        if (!useGuestModeSelectedLevelInfos)
                        {
                            var currentLevel = guestModeLevels.First(x => x.id == currentLevelId);
                            
                            var level = new LevelDTO
                            {
                                Id = currentLevel.id,
                                Difficulty = currentLevel.difficulty,
                                SubjectThemeId = currentLevel.subjectThemeId,
                                SubjectTheme = new SubjectThemeDTO
                                {
                                  Name  = currentLevel.subjectThemeName,
                                  Code = currentLevel.levelCode,
                                  SubjectId = currentLevel.subjectId
                                },
                                MidScoreThreshold = currentLevel.midScoreThreshold,
                                HighScoreThreshold = currentLevel.highScoreThreshold
                            };
                            return new LevelInfoAndScore(level.SubjectTheme, level, null,0 , false/*_currentLevelInfo.score*/);
                        }
                        else
                        {
                            if (_currentLevelInfo == null) 
                                return null;
                            
                            return new LevelInfoAndScore(_currentLevelInfo.subjectThemeDto, _currentLevelInfo.level,_currentLevelInfo.quiz, 0, false);
                        }
                    #else
                     if (_currentLevelInfo == null) 
                        return null;

                    return new LevelInfoAndScore(_currentLevelInfo.subjectThemeDto, _currentLevelInfo.level,_currentLevelInfo.quiz, 0, false);
                    #endif
                }
                else
                    return _currentLevelInfo;
            }
            
            set => _currentLevelInfo = value;
        }
        
        public List<LevelInfoAndScore> CurrentSubjectThemeAllLevels { get; set; }

        public LevelInfoAndScore NextLevelInfo { get; set; }
        
        public Action<LevelInfoAndScore> levelSelected;
        
        [field: Header("Debug Purposes")] 
        [field:SerializeField] public PlayerDataSO PlayerData { get; set; }
        [SerializeField] private bool useGuestModeSelectedLevelInfos;
        [SerializeField] private int currentLevelId = 0;
        [field:SerializeField] public DebugLevel[] guestModeLevels;

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }
        
        public void SelectLevel(LevelInfoAndScore levelInfoAndScore)
        {
            CurrentLevelInfo = levelInfoAndScore;
            levelSelected?.Invoke(CurrentLevelInfo);
        }

        public List<LevelDTO> ConvertGuestModeLevelsToLevels()
        {
            List<LevelDTO> convertedLevels = new();
            
            foreach (var debugLevel in guestModeLevels)
            {
                var level = new LevelDTO
                {
                    Id = debugLevel.id,
                    Difficulty = debugLevel.difficulty,
                    SubjectThemeId = debugLevel.subjectThemeId,
                    SubjectTheme = new SubjectThemeDTO
                    {
                        Name  = debugLevel.subjectThemeName,
                        Code = debugLevel.levelCode
                    },
                    MidScoreThreshold = debugLevel.midScoreThreshold,
                    HighScoreThreshold = debugLevel.highScoreThreshold
                };
                convertedLevels.Add(level);
            }

            return convertedLevels;
        }
        
        public bool CheckIfNextLevelExists()
        {
            if (PlayerData.GuestMode)
            {
                if (CurrentLevelInfo.level == null) return false;
                
                var nextLevelInfoDebug = guestModeLevels.FirstOrDefault(x => x.id == CurrentLevelInfo.level.Id + 1 && x.subjectThemeId == CurrentLevelInfo.subjectThemeDto.Id);
                if (nextLevelInfoDebug == null) 
                    return false;
                else 
                    return true;
            }
            else
            {
                LevelInfoAndScore nextLevel = null;
                
                if(CurrentLevelInfo.level != null)
                    nextLevel = CurrentSubjectThemeAllLevels.FirstOrDefault(x => x.level.Id == CurrentLevelInfo.level.Id + 1);
                
                if (nextLevel == null) 
                    return false;
                else
                {
                    SetupNextLevel();
                    return true;
                }
                    
            }
        }

        public void SetupNextLevel()
        {
            NextLevelInfo = null;
            
            if (PlayerData.GuestMode)
            {
                var nextLevelInfoDebug = guestModeLevels.FirstOrDefault(x => x.id == CurrentLevelInfo.level.Id + 1);
                
                if (nextLevelInfoDebug == null) return;
                
                var level = new LevelDTO
                {
                    Id = nextLevelInfoDebug.id,
                    Difficulty = nextLevelInfoDebug.difficulty,
                    SubjectTheme = new SubjectThemeDTO
                    {
                        Name  = nextLevelInfoDebug.subjectThemeName,
                        Code = nextLevelInfoDebug.levelCode
                    },
                    SubjectThemeId = nextLevelInfoDebug.subjectThemeId,
                    MidScoreThreshold = nextLevelInfoDebug.midScoreThreshold,
                    HighScoreThreshold = nextLevelInfoDebug.highScoreThreshold
                };

                _currentLevelInfo.subjectThemeDto.Id += 1;
                
                NextLevelInfo = new LevelInfoAndScore(level.SubjectTheme ,level, null, 0, false);
            }
            else
            {
                NextLevelInfo = CurrentSubjectThemeAllLevels.FirstOrDefault(x => x.level.Id == CurrentLevelInfo.level.Id + 1);
            }
        }

        public void LoadNextLevel()
        {
            if(NextLevelInfo == null)
                Debug.Log("next level is null");
            
            CurrentLevelInfo = NextLevelInfo;
            currentLevelId = NextLevelInfo.level.Id;
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Reset()
        {
            CurrentLevelInfo = null;
        }

    }
}

[Serializable]
public class DebugLevel
{
    public int id;
    public int subjectThemeId;
    public string subjectThemeName;
    public string levelCode;
    public int subjectId;
    public int difficulty;
    public int midScoreThreshold;
    public int highScoreThreshold;

    public DebugLevel(int idd, int subjThemeId, string subjName, string lvlCode, int diff, int midScore, int highScore, int subjId)
    {
        this.id = idd;
        this.subjectThemeId = subjThemeId;
        this.subjectThemeName = subjName;
        this.subjectId = subjId;
        this.levelCode = lvlCode;
        this.difficulty = diff;
        this.midScoreThreshold = midScore;
        this.highScoreThreshold = highScore;
    }
}

