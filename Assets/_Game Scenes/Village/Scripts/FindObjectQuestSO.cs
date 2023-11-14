using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game_Scenes.Village.Scripts;
using _MeteorShower.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "FindObjectQuest", menuName = "ScriptableObjects/VillageScene/FindObjectQuest", order = 10)]
public class FindObjectQuestSO : ScriptableObject
{
    [Header("SO")] 
    [SerializeField] private PointsManagerSO pointsManager;
    [Space]
    [SerializeField] [ReadOnly] private ThreeDObjectInfoSO correctObject;
    [SerializeField] [ReadOnly] private List<ThreeDObjectInfoSO> wrongObjects;
    [SerializeField] [ReadOnly] private VillageNpcDataSO targetNpc;

    [SerializeField] private List<VillageNpcDataSO> AllNpcs;
    [SerializeField] private List<ThreeDObjectInfoSO> AllPolyhedron3dObjects;

    public event Action objectsSet;
    public event Action<int> correctObjectChoice;
    public event Action<int> wrongObjectChoice;
    
    private int numberOfObjectsAlternatives = 2;
    
    private static System.Random rng = new System.Random();
    public List<ThreeDObjectInfoSO> RandomObjectsOptionsList { get; private set; }
    
    public ThreeDObjectInfoSO GetCorrectObject => correctObject;
    public List<ThreeDObjectInfoSO> GetWrongObjects => wrongObjects;
    public VillageNpcDataSO TargetNpc => targetNpc;
    
    public enum ObjectParameterToAnswerCorrectly {Vertices,Edges,Faces}

    public ObjectParameterToAnswerCorrectly CurrentCorrectParameter { get; private set; }
    
    //score settings
    private float startedTime;
    private float endedTime;
    private int wrongAnswersSelected;
    private int calculatedScore;
    private int baseScore = 200;
    private int wrongAnswerPenalty = 9;
    
    public void SetRandomQuestParameters()
    {
        startedTime = Time.time;
        Debug.Log("Quest Started");
        wrongAnswersSelected = 0;
        
        correctObject = null;
        wrongObjects = new List<ThreeDObjectInfoSO>();
        
        correctObject = AllPolyhedron3dObjects[Random.Range(0, AllPolyhedron3dObjects.Count)];
        int randomNumber = Random.Range(0, 3);
        
        switch (randomNumber)
        {
            case 0:
                CurrentCorrectParameter = ObjectParameterToAnswerCorrectly.Edges;
                break;
            case 1:
                CurrentCorrectParameter = ObjectParameterToAnswerCorrectly.Faces;
                break;
            case 2:
                CurrentCorrectParameter = ObjectParameterToAnswerCorrectly.Vertices;
                break;
        }
        
        while (wrongObjects.Count < numberOfObjectsAlternatives)
        {
            var wrongObj = AllPolyhedron3dObjects[Random.Range(0, AllPolyhedron3dObjects.Count)];

            switch (CurrentCorrectParameter)
            {
                case ObjectParameterToAnswerCorrectly.Edges:
                    if (correctObject.NumberOfEdges == wrongObj.NumberOfEdges)
                        continue;
                    break;
                case ObjectParameterToAnswerCorrectly.Faces:
                    if (correctObject.NumberOfFaces == wrongObj.NumberOfFaces)
                        continue;
                    break;
                case ObjectParameterToAnswerCorrectly.Vertices:
                    if(correctObject.NumberOfVertices == wrongObj.NumberOfVertices)
                        continue;
                    break;
            }
            if(wrongObjects.Count > 0)
                foreach (var wObject in wrongObjects)
                {
                    if (wrongObj == wObject)
                        goto continueLoop; //if i use continue here, i will only break this foreach loop, so i jumped no "continueloop" line do make something like continue
                }
            
            if(wrongObj == correctObject)
                continue;
            
            wrongObjects.Add(wrongObj);

            continueLoop:;

        }

        List<VillageNpcDataSO> availableNpcs = new();
        
        foreach (var npc in AllNpcs)
        {
            if(npc.GetNpcType == VillageNpcDataSO.NpcType.Default)
                availableNpcs.Add(npc);
        }

        targetNpc = availableNpcs[Random.Range(0, availableNpcs.Count)];
        targetNpc.SetNpcType(VillageNpcDataSO.NpcType.QuestInteractor,QuestManagerSO.QuesType.FindObject);
        
        SetRandomObjectsList();
        
        objectsSet?.Invoke();
    }

    private void SetRandomObjectsList()
    {
        RandomObjectsOptionsList = new List<ThreeDObjectInfoSO>();
        RandomObjectsOptionsList.AddRange(wrongObjects);
        RandomObjectsOptionsList.Add(correctObject);
        
        RandomObjectsOptionsList = RandomObjectsOptionsList.OrderBy(a => rng.Next()).ToList(); // aleatorizando lista
    }

    public void SelectObjectFromUI(int index)
    {
        if (RandomObjectsOptionsList[index] == correctObject)
        {
            Debug.Log("Objeto Correto");
            CalculateScore();
            correctObjectChoice?.Invoke(index);
        }
        else
        {
            wrongAnswersSelected++;
            wrongObjectChoice?.Invoke(index);    
            Debug.Log("Objeto incorreto");
        }
    }

    private void CalculateScore()
    {
        endedTime = Time.time;

        var timePenalty = Mathf.Clamp(((int)endedTime - (int)startedTime) / 5, 0, baseScore/4); //200 pontos base menos o tempo que ele demorou pra finalizar dividido por 4, sendo no máximo 1/4 possivel de punicao
        calculatedScore = Mathf.Abs(baseScore - timePenalty); 

        calculatedScore = calculatedScore - (wrongAnswersSelected * wrongAnswerPenalty);

        Debug.Log("Started Time: " + startedTime + " End Time: " + endedTime + " Penalty Calculation: " + timePenalty
                  + " wrong Answers Selected: " + wrongAnswersSelected + " calculated Score: " + calculatedScore);
        
        pointsManager.AddPoints(calculatedScore);
    }

    private void ResetSO()
    {
        startedTime = 0;
        endedTime = 0;
        wrongAnswersSelected = 0;

        correctObject = null;
        wrongObjects = new List<ThreeDObjectInfoSO>();
        targetNpc = null;
    }

    private void OnDisable()
    {
        ResetSO();
    }
}
