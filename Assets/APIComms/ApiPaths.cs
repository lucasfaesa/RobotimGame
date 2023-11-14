using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ApiPaths
{
    
    public static string API_URL(bool useAzurePath)
    {
        if (useAzurePath)
            return "";
        else
            return "https://localhost:5001/api/";
    }

    public static string STUDENT_URL(bool useAzurePath) => API_URL(useAzurePath) + "";

    public static string STUDENT_COMPLETED_LEVELS(bool useAzurePath) => API_URL(useAzurePath) + "";
    public static string STUDENT_COMPLETED_QUIZES(bool useAzurePath) => API_URL(useAzurePath) + "";
    
    public static string POST_SESSION_URL(bool useAzurePath) => API_URL(useAzurePath) + "";

    public static string POST_UPDATE_SESSION_URL(bool useAzurePath) => API_URL(useAzurePath) + "";

    public static string LEVEL_URL(bool useAzurePath) => API_URL(useAzurePath) +  "";

    public static string LEVEL_BY_CODE_URL(bool useAzurePath) => API_URL(useAzurePath) + "";

    public static string STUDENT_LOGIN(bool useAzurePath) => API_URL(useAzurePath) + "";
    
    public static string STUDENT_TEMP_LOGIN(bool useAzurePath) => API_URL(useAzurePath) + "";
    
    public static string STUDENT_LOGIN_USERNAME_FIELD(bool useAzurePath) => API_URL(useAzurePath) + "";

    public static string STUDENT_LOGIN_PASSWORD_FIELD(bool useAzurePath) => API_URL(useAzurePath) + "";
    
    //Group Class
    public static string GROUP_CLASS_AND_LEVELS(bool useAzurePath) => API_URL(useAzurePath) + "";
    
    //Ranking
    public static string RANKING_BY_GROUPCLASS_AND_LEVELID_OR_QUIZID(bool useAzurePath) => API_URL(useAzurePath) + "";
    
    public static string GENERAL_RANKING_BY_GROUPCLASS(bool useAzurePath) => API_URL(useAzurePath) + "";

    //Scores
    public static string PLAYER_TOP_SCORES_OF_LEVEL(bool useAzurePath) => API_URL(useAzurePath) + "";
    
    public static string PLAYER_TOP_SCORES_OF_QUIZ(bool useAzurePath) => API_URL(useAzurePath) + "";
    
    public static string STUDENT_TOTAL_SCORE(bool useAzurePath) => API_URL(useAzurePath) + "";
    
    //Questions
    public static string GET_QUESTIONS(bool useAzurePath) => API_URL(useAzurePath) + "";
    public static string GET_QUESTIONS_BY_TEACHER(bool useAzurePath) => API_URL(useAzurePath) + "";
    
    public static string POST_STUDENT_WRONG_QUESTIONS(bool useAzurePath) => API_URL(useAzurePath) + "";
    
    public static string GET_TEACHER_ACTIVE_QUIZES(bool useAzurePath) => API_URL(useAzurePath) + "";
    public static string GET_QUIZ_BY_ID(bool useAzurePath) => API_URL(useAzurePath) + "";
}
