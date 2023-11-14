using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using API_Mestrado_Lucas;
using APIComms;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace _Lobby._LevelSelector.Scripts
{
    public class LevelsSelector : MonoBehaviour
    {
        [Header("SO")] 
        [SerializeField] private ApiCommsControllerSO apiCommsController;
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private ActionsTogglerSO actionsToggler;
        [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
        [Header("Level Instantiation")]
        [SerializeField] private SubjectThemeUIDisplayer subjectThemeUIDisplayerPrefab;
        [SerializeField] private Transform levelUIContent;
        [SerializeField] private List<SubjectThemeUIDisplayer> subjectThemeUIDisplayersPool;
        [Header("General UI")] 
        [SerializeField] private GameObject UiContent;
        [Header("Feedback")] 
        [SerializeField] private GameObject loaderCicle;
        [SerializeField] private GameObject xIcon;
    
        private GroupClassDTO groupClass;
        private List<QuizDTO> teacherActiveQuizes =new();
        private List<SessionDTO> levelTopScoresSessions;
        private List<SessionDTO> quizTopScoresSessions;
        private List<LevelDTO> completedLevels;
        private List<QuizDTO> completedQuizes = new();
        
        private List<LevelInfoAndScore> allLevelsOrQuizesAndScores = new();

        private LevelInfoAndScore previousSelectedLevel;

        private bool availableLevelsRoutineFinished;
        private bool teacherQuizesRoutineFinished;
        private bool playerCompletedLevelsRoutineFinished;
        private bool playerCompletedQuizesRoutineFinished; 
        private bool playerLevelTopScoresRoutineFinished;
        private bool playerQuizesTopScoresRoutineFinished;

        private Coroutine watcherRoutine;
        
        private void OnEnable()
        {
            levelSelectedManager.levelSelected += PassFullListOfSubjectThemeLevels;
        }

        private void OnDisable()
        {
            levelSelectedManager.levelSelected -= PassFullListOfSubjectThemeLevels;
        }
        
        //passing list of corresponding subjectTheme of selected level
        private void PassFullListOfSubjectThemeLevels(LevelInfoAndScore lvlInfoAndScore)
        {
            if (previousSelectedLevel != null && previousSelectedLevel.level == null) previousSelectedLevel = null; //some gambiarra because it was bugging somehow

            if (previousSelectedLevel != null)
            {
                if(lvlInfoAndScore.level != null)
                    if (previousSelectedLevel.level != null && previousSelectedLevel.level.SubjectThemeId == lvlInfoAndScore.level.SubjectThemeId) return;
                if(lvlInfoAndScore.quiz != null)
                    if (previousSelectedLevel.quiz != null && previousSelectedLevel.quiz == lvlInfoAndScore.quiz) return;
            }
            
            previousSelectedLevel = lvlInfoAndScore;

            if (lvlInfoAndScore.level != null)
            {
                var completeSubjectThemeLevels =
                    allLevelsOrQuizesAndScores.FindAll(x => x.level != null && x.level.SubjectThemeId == lvlInfoAndScore.level.SubjectThemeId);

                levelSelectedManager.CurrentSubjectThemeAllLevels = completeSubjectThemeLevels;    
            }
            else
            {
                List<LevelInfoAndScore> quizLevel = new() { lvlInfoAndScore }; //too lazy to create a method to pass only a object, so i made a list with only one item
                levelSelectedManager.CurrentSubjectThemeAllLevels = quizLevel;
            }
            
        }

        void Start()
        {
            levelSelectedManager.Reset();

            if (!levelSelectedManager.PlayerData.GuestMode)
                GetEverythingAndGenerateLevels();
            else
                GenerateLevelsList();
        }

        public void RefreshButton()
        {
            if (watcherRoutine != null)
                return;
            
            subjectThemeUIDisplayersPool.ForEach(x=>x.gameObject.SetActive(false));
            GetEverythingAndGenerateLevels();
        }

        private void GetEverythingAndGenerateLevels()
        {
            availableLevelsRoutineFinished = false;
            teacherQuizesRoutineFinished = false;
            playerCompletedLevelsRoutineFinished = false;
            playerCompletedQuizesRoutineFinished = false;
            playerLevelTopScoresRoutineFinished = false;
            playerQuizesTopScoresRoutineFinished = false;
            
            watcherRoutine = StartCoroutine(GetsWatcher());
            
            StartCoroutine(GetPlayerAvailableLevels());
            StartCoroutine(GetTeacherAvailableQuizes());
            StartCoroutine(GetPlayerCompletedLevels());
            StartCoroutine(GetPlayerCompletedQuizes());
            StartCoroutine(GetPlayerTopScoresOfLevels());
            StartCoroutine(GetPlayerTopScoresOfQuizes());
        }

        private IEnumerator GetsWatcher()
        {
            loaderCicle.SetActive(true);
            
            while (!availableLevelsRoutineFinished || !teacherQuizesRoutineFinished ||
                   !playerCompletedLevelsRoutineFinished || !playerCompletedQuizesRoutineFinished || 
                        !playerLevelTopScoresRoutineFinished || !playerQuizesTopScoresRoutineFinished)
            {
                yield return null;
            }
            loaderCicle.SetActive(false);
            
            GenerateLevelsList();

            watcherRoutine = null;
        }

        private void ErrorOccurred()
        {
            if(watcherRoutine != null)
                StopCoroutine(watcherRoutine);
            
            xIcon.SetActive(true);
            loaderCicle.SetActive(false);
        }
        
        private IEnumerator GetPlayerAvailableLevels()
        {
            var playerGroupClassId = playerData.StudentData.GroupClassId;

            if (playerGroupClassId == null)
                throw new Exception("Aluno sem Group Class");
            
            loaderCicle.SetActive(true);
            
            using UnityWebRequest webRequest = UnityWebRequest.Get(ApiPaths.GROUP_CLASS_AND_LEVELS(apiCommsController.UseCloudPath) + playerGroupClassId);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
       
            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(webRequest.error);
                Debug.LogError("GroupClass get error");
                ErrorOccurred();
            }
            else
            {
                groupClass = JsonConvert.DeserializeObject<GroupClassDTO>(webRequest.downloadHandler.text);
                Debug.Log("GroupClass get success");
                availableLevelsRoutineFinished = true;
            }
            
            webRequest.Dispose();
            
        }

        private IEnumerator GetTeacherAvailableQuizes()
        {
            var playerTeacherId = playerData.StudentData.TeacherId;

            loaderCicle.SetActive(true);
            
            using UnityWebRequest webRequest = UnityWebRequest.Get(ApiPaths.GET_TEACHER_ACTIVE_QUIZES(apiCommsController.UseCloudPath) + playerTeacherId);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
       
            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(webRequest.error);
                Debug.LogError("Teacher Active Quizes get error");
                ErrorOccurred();
            }
            else
            {
                teacherActiveQuizes = JsonConvert.DeserializeObject<List<QuizDTO>>(webRequest.downloadHandler.text);
                Debug.Log("Teacher Active Quizes get success");
                teacherQuizesRoutineFinished = true;
            }
       
            webRequest.Dispose();
            
        }
        
        private IEnumerator GetPlayerCompletedLevels()
        {
            var playerId = playerData.StudentData.Id;

            using UnityWebRequest webRequest = UnityWebRequest.Get(ApiPaths.STUDENT_COMPLETED_LEVELS(apiCommsController.UseCloudPath) + playerId);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
       
            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(webRequest.error);
                Debug.LogError("completed levels get error");
                ErrorOccurred();
            }
            else
            {
                completedLevels = JsonConvert.DeserializeObject<List<LevelDTO>>(webRequest.downloadHandler.text);
                Debug.Log("completed levels get success");
                playerCompletedLevelsRoutineFinished = true;
            }
       
            webRequest.Dispose();
            
        }
        
        private IEnumerator GetPlayerCompletedQuizes()
        {
            var playerId = playerData.StudentData.Id;

            using UnityWebRequest webRequest = UnityWebRequest.Get(ApiPaths.STUDENT_COMPLETED_QUIZES(apiCommsController.UseCloudPath) + playerId);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
       
            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(webRequest.error);
                Debug.LogError("completed quizes get error");
                ErrorOccurred();
            }
            else
            {
                completedQuizes = JsonConvert.DeserializeObject<List<QuizDTO>>(webRequest.downloadHandler.text);
                Debug.Log("completed quizes get success");
                playerCompletedQuizesRoutineFinished = true;
            }
       
            webRequest.Dispose();
            
        }

        private IEnumerator GetPlayerTopScoresOfLevels()
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(ApiPaths.PLAYER_TOP_SCORES_OF_LEVEL(apiCommsController.UseCloudPath) + playerData.StudentData.Id);
        
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
       
            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(webRequest.error);
                Debug.LogError("TopScores Level get error");
                ErrorOccurred();
            }
            else
            {
                levelTopScoresSessions = JsonConvert.DeserializeObject<List<SessionDTO>>(webRequest.downloadHandler.text);
            
                if(levelTopScoresSessions != null)
                    levelTopScoresSessions = levelTopScoresSessions.OrderBy(x => x.LevelId).ToList();

                playerLevelTopScoresRoutineFinished = true;
                
                Debug.Log("TopScores LEvel get success");
            }

            webRequest.Dispose();
            
        }
        
        private IEnumerator GetPlayerTopScoresOfQuizes()
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(ApiPaths.PLAYER_TOP_SCORES_OF_QUIZ(apiCommsController.UseCloudPath) + playerData.StudentData.Id);
        
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
       
            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(webRequest.error);
                Debug.LogError("TopScores Quiz get error");
                ErrorOccurred();
            }
            else
            {
                quizTopScoresSessions = JsonConvert.DeserializeObject<List<SessionDTO>>(webRequest.downloadHandler.text);
            
                if(quizTopScoresSessions != null)
                    quizTopScoresSessions = quizTopScoresSessions.OrderBy(x => x.QuizId).ToList();

                playerQuizesTopScoresRoutineFinished = true;
                Debug.Log("TopScores Quiz get success");
            }

            webRequest.Dispose();
            
        }
        

        private void GenerateLevelsList()
        {
            if (!levelSelectedManager.PlayerData.GuestMode)
            {
                if (groupClass.GroupClassSubjectThemes.Count + teacherActiveQuizes.Count > subjectThemeUIDisplayersPool.Count)
                {
                    InstantiateNewLevelsUIPrefab(groupClass.GroupClassSubjectThemes.Count + teacherActiveQuizes.Count - subjectThemeUIDisplayersPool.Count);
                }

                List<SubjectThemeUIDisplayer> availableSubjectThemeUIs = new(subjectThemeUIDisplayersPool);
                allLevelsOrQuizesAndScores = new();
                
                foreach (var groupClassSubjectThemeDto in groupClass.GroupClassSubjectThemes)
                {
                    string subjectThemeName = "";
                    int levelHighScore = 0;
                    List<LevelInfoAndScore> levelsAndScores = new();

                    foreach (var level in groupClassSubjectThemeDto.SubjectTheme.Levels)
                    {
                        subjectThemeName = groupClassSubjectThemeDto.SubjectTheme.Name;
                        
                        if(levelTopScoresSessions != null)
                        {
                            var topScoreSession = levelTopScoresSessions.FirstOrDefault(x => x.LevelId == level.Id);
                            
                            if (topScoreSession != null)
                                levelHighScore = topScoreSession.Score;
                            else
                                levelHighScore = 0;
                        }

                        bool levelIsCompleted = completedLevels.Any(x=>x.Id == level.Id);
                        
                        levelsAndScores.Add( new LevelInfoAndScore(groupClassSubjectThemeDto.SubjectTheme, level, null, levelHighScore, levelIsCompleted));
                        
                    }
                    
                    levelsAndScores = levelsAndScores.OrderBy(x => x.level.Difficulty).ToList(); //só garantindo
                    allLevelsOrQuizesAndScores.AddRange(levelsAndScores);
                    
                    availableSubjectThemeUIs[0].SetInformations(subjectThemeName, levelsAndScores);
                    availableSubjectThemeUIs[0].gameObject.SetActive(true);
                    availableSubjectThemeUIs.RemoveAt(0);
                }
                
                foreach (var quiz in teacherActiveQuizes)
                {
                    string quizName = quiz.Name;
                    int quizHighScore = 0;
                    
                    
                    if(quizTopScoresSessions != null)
                    {
                        var topScoreSession = quizTopScoresSessions.FirstOrDefault(x => x.QuizId == quiz.Id);
                        
                        if (topScoreSession != null)
                            quizHighScore = topScoreSession.Score;
                        else
                            quizHighScore = 0;
                    }

                    bool quizIsCompleted = completedQuizes.Any(x=>x.Id == quiz.Id);

                    LevelInfoAndScore quizInfoAndScore = new LevelInfoAndScore(null, null, quiz, quizHighScore, quizIsCompleted);
                    
                    allLevelsOrQuizesAndScores.Add( quizInfoAndScore);
                    
                    availableSubjectThemeUIs[0].SetInformations(quizName, quizInfoAndScore);
                    availableSubjectThemeUIs[0].gameObject.SetActive(true);
                    availableSubjectThemeUIs.RemoveAt(0);
                }
                
            }
            else
            {
                
                var levelsGroupedBySubject = levelSelectedManager.ConvertGuestModeLevelsToLevels().GroupBy(x => x.SubjectThemeId).ToList();
                
                if (levelsGroupedBySubject.Count > subjectThemeUIDisplayersPool.Count)
                {
                    InstantiateNewLevelsUIPrefab(levelsGroupedBySubject.Count - subjectThemeUIDisplayersPool.Count);
                }
                
                var availableLevelUIs = subjectThemeUIDisplayersPool;
                allLevelsOrQuizesAndScores = new();
                
                foreach (var groupClasses in levelsGroupedBySubject)
                {
                    string levelName = "";
                    int levelHighScore = 0;
                    List<LevelInfoAndScore>  levelsAndScores = new();
                    
                    foreach (var groupClassLevel in groupClasses)
                    {
                        levelName = groupClassLevel.SubjectTheme.Name;

                        /*var topScoreSession = topScoresSession.FirstOrDefault(x => x.LevelId == groupClassLevel.Id);
                        if (topScoreSession != null)
                            levelHighScore = topScoreSession.Score;
                        else
                            levelHighScore = 0;*/
                
                        levelsAndScores.Add( new LevelInfoAndScore(groupClassLevel.SubjectTheme, groupClassLevel, null, levelHighScore, false));
                    }
            
                    levelsAndScores = levelsAndScores.OrderBy(x => x.level.Difficulty).ToList(); //só garantindo
                    allLevelsOrQuizesAndScores.AddRange(levelsAndScores);
                    
                    availableLevelUIs[0].SetInformations(levelName, levelsAndScores);
                    availableLevelUIs[0].gameObject.SetActive(true);
                    availableLevelUIs.RemoveAt(0);
                }
            }
            
        }

        private void InstantiateNewLevelsUIPrefab(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                SubjectThemeUIDisplayer subjectThemeUIDisplayer = Instantiate(subjectThemeUIDisplayerPrefab, levelUIContent);
                subjectThemeUIDisplayer.gameObject.SetActive(false);
                subjectThemeUIDisplayersPool.Add(subjectThemeUIDisplayer);
            }
        }
    
        public void ToggleContent(bool status)
        {
            UiContent.SetActive(status);    
            actionsToggler.EnableCursor(status);
            actionsToggler.MovementToggle(!status);
            actionsToggler.OrbitToggle(!status);
            
            if (!status)
            {
                levelSelectedManager.CurrentLevelInfo = null;
            }
        }
    
    }

    [Serializable]
    public class LevelInfoAndScore
    {
        public SubjectThemeDTO subjectThemeDto;
        public LevelDTO level;
        public QuizDTO quiz;
        public int score;
        public bool completed;

        public LevelInfoAndScore(SubjectThemeDTO subjectThmDto, LevelDTO lev, QuizDTO quizDto, int sco, bool comp)
        {
            this.subjectThemeDto = subjectThmDto;
            this.quiz = quizDto;
            this.level = lev;
            this.score = sco;
            this.completed = comp;
        }
    }
}