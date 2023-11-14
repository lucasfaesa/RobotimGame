using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Lobby._LevelSelector.Scripts;
using _MeteorShower.Scripts;
using _Village.Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public enum MeteorDifficulty {Easy, Medium, Hard }
public enum MeteorDistance {Close, Medium, Far};

public class MeteorsSpawnController : MonoBehaviour
{
    [Header("SO")]
    [SerializeField] private AnswersManagerSO answersManager;
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private TimeManagerSO timeManager;
    [SerializeField] private LevelSelectedManagerSO levelSelectedManager;
    [SerializeField] private ApiCommsControllerSO apiCommsController;
    [SerializeField] private LevelCompletionSO levelCompletion;
    [Space]
    [SerializeField] private MathsGenerator mathsGenerator;
    [Space]
    [SerializeField] private MeteorStats[] pooledMeteors;
    [SerializeField] private MeteorStats[] meteorsPrefabs;
    
    [Header("Debug Purposes")]
    [SerializeField] private MeteorDifficulty nextMeteorDifficulty = MeteorDifficulty.Easy;
    [SerializeField] private float delayBetweenMeteors = 3f;

    [SerializeField] private List<MeteorStats> activeMeteors = new List<MeteorStats>();
    [SerializeField] private List<MeteorStats> unsolvedMeteors = new List<MeteorStats>();
    
    private MeteorSpeed meteorSpeed = new MeteorSpeed(5, 5, 5);
    private MeteorSize meteorSize = new MeteorSize(new Vector3(0.9f,0.9f,0.9f), 
                                                    new Vector3(1.3f,1.3f,1.3f), 
                                                        new Vector3(1.8f,1.8f,1.8f));

    private Coroutine spawnRoutine;
    
    private bool timeEnded;
    
    public List<MeteorStats> GetActiveMeteors => activeMeteors;
    public List<MathQuestion> GetActiveQuestions => mathsGenerator.ActiveQuestions;

    private void OnEnable()
    {
        gameManager.gameStarted += StartSpawningMeteors;
        gameManager.gameGoingToEnd += StopSpawningMeteors;
        timeManager.timeEnded += TimeEnded;
    }

    private void OnDisable()
    {
        gameManager.gameStarted -= StartSpawningMeteors;
        gameManager.gameGoingToEnd -= StopSpawningMeteors;
        timeManager.timeEnded -= TimeEnded;
    }

    private void TimeEnded() => timeEnded = true;

    private void StartSpawningMeteors()
    {
        if(spawnRoutine != null)
            StopCoroutine(spawnRoutine);
        
        spawnRoutine = StartCoroutine(SpawnMeteors());
    }

    private void StopSpawningMeteors()
    {
        StopCoroutine(spawnRoutine);
    }
    
    private IEnumerator SpawnMeteors()
    {
        float elapsedTime = 0;
        
        while (true)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > delayBetweenMeteors)
            {
                ActivateMeteor();
                elapsedTime = 0;
            }
            else
                yield return null;
        }
    }
    
    private void ActivateMeteor()
    {
        foreach (var meteor in pooledMeteors)
        {
            if (!meteor.IsMeteorActive())
            {
                meteor.InjectMeteorsSpawnController(this);
                
                meteor.ActivateMeteor(GenerateMeteorData(levelSelectedManager.CurrentLevelInfo.level.Difficulty));
                
                break;
            }
        }
    }

    private MeteorData GenerateMeteorData(int difficulty)
    {
        nextMeteorDifficulty = RandomMeteorDifficulty(difficulty);
        
        MeteorDistance meteorDistanceEnum = MeteorDistance.Medium;
        
        float meteorDistanceValue = GenerateMeteorDistance(ref meteorDistanceEnum);
        float meteorHeightValue = GenerateMeteorHeight(meteorDistanceEnum);
        bool rightToLeft = true;
        float meteorSideValue = MeteorSideGenerator(meteorDistanceEnum, ref rightToLeft);
        
        switch (nextMeteorDifficulty)
        {
            case MeteorDifficulty.Easy:
                return new MeteorData(meteorSpeed.slowSpeed, meteorSize.smallSize, meteorDistanceValue,
                    meteorHeightValue, meteorSideValue, rightToLeft, nextMeteorDifficulty, meteorDistanceEnum,
                    mathsGenerator.GenerateMathQuestion(nextMeteorDifficulty), Random.Range(21, 26), Time.time);
            
            case MeteorDifficulty.Medium:
                return new MeteorData(meteorSpeed.mediumSpeed, meteorSize.mediumSize, meteorDistanceValue,
                    meteorHeightValue, meteorSideValue, rightToLeft, nextMeteorDifficulty, meteorDistanceEnum,
                                                        mathsGenerator.GenerateMathQuestion(nextMeteorDifficulty),Random.Range(29, 37), Time.time);
            
            case MeteorDifficulty.Hard:
                return new MeteorData(meteorSpeed.fastSpeed, meteorSize.bigSize, meteorDistanceValue,
                    meteorHeightValue, meteorSideValue, rightToLeft, nextMeteorDifficulty, meteorDistanceEnum,
                                                        mathsGenerator.GenerateMathQuestion(nextMeteorDifficulty),Random.Range(42, 54), Time.time);
            
            default:
                Debug.Log("Entered Default");
                return new MeteorData(meteorSpeed.mediumSpeed, meteorSize.mediumSize, meteorDistanceValue,
                    meteorHeightValue, meteorSideValue, rightToLeft, nextMeteorDifficulty, meteorDistanceEnum,
                                                        mathsGenerator.GenerateMathQuestion(nextMeteorDifficulty),Random.Range(21, 26), Time.time);
        }
    }

    private float MeteorSideGenerator(MeteorDistance meteorDistance, ref bool rightToLeft)
    {
        float posCloseLeft = -86f;
        float posCloseRight = 86f;

        float posMediumLeft = -134f;
        float posMediumRight = 134f;

        float posFarLeft = -178.4f;
        float posFarRight = 178.4f;
        
        int randomNumber = Random.Range(1, 3);
        if (randomNumber == 1)
            rightToLeft = true;
        else
            rightToLeft = false;
        
        switch (meteorDistance)
        {
            case MeteorDistance.Close:
                if(rightToLeft)
                    return posCloseRight;
                return posCloseLeft;
            case MeteorDistance.Medium:
                if(rightToLeft)
                    return posMediumRight;
                return posMediumLeft;
            case MeteorDistance.Far:
                if(rightToLeft)
                    return posFarRight;
                return posFarLeft;
            
            default:
                Debug.Log("Entered Default");
                if(rightToLeft)
                    return posMediumRight;
                return posMediumLeft;
        }
    }

    private float GenerateMeteorDistance(ref MeteorDistance distance)
    {
        /*int random = Random.Range(1, 4);

        switch (random)
        {
            case 1:
                distance = MeteorDistance.Close;
                return -72f;
            case 2:
                distance = MeteorDistance.Medium;
                return -115.3f;
            case 3:
                distance = MeteorDistance.Far;
                return -158f;
            default:
                Debug.Log("Entered Default");
                distance = MeteorDistance.Medium;
                return -115.3f;
        }*/
        
        distance = MeteorDistance.Far;
        return -158f;
    }

    private float GenerateMeteorHeight(MeteorDistance distance)
    {
        
        /*
        float totalCloseHeight = 60f;
        float totalMediumHeight = 98f;
        float totalFarHeight = 140.5f;
        
        switch (distance)
        {
            case MeteorDistance.Close:
                return ((totalCloseHeight / maxNumberOfRows) * multiplier) + -27f; //encontrando a posição em Y do meteoro
            
            case MeteorDistance.Medium:
                return ((totalMediumHeight / maxNumberOfRows) * multiplier) + -44.1f; //encontrando a posição em Y do meteoro
            
            case MeteorDistance.Far:
                return ((totalFarHeight / maxNumberOfRows) * multiplier) + -60.9f; //encontrando a posição em Y do meteoro
                
            default:
                Debug.Log("Entered Default");
                return (totalMediumHeight / maxNumberOfRows) * multiplier; //encontrando a posição em Y do meteoro
        }*/
        
        //se já tiver meteoros na altura que ele escolher E o algum meteoro desta altura tiver sido spawnado a menos de
        //3 segundos, então ele procura outra altura pra spawnar, até encontrar uma altura válida.
        while (true)
        {
            int multiplier = Random.Range(1, 8);
            int maxNumberOfRows = 7;
            float totalFarHeight = 124f;
            float randomHeight = ((totalFarHeight / maxNumberOfRows) * multiplier) + -60.9f;
            
            List<MeteorStats> sameHeightMeteors = activeMeteors.FindAll(x => x.MeteorData.meteorHeightY == randomHeight);
            
            if (sameHeightMeteors.Count > 0)
            {
                if (sameHeightMeteors.Any(x => Time.time - x.MeteorData.meteorSpawnTime <= 3f))
                {
                    continue;
                }
            }
            
            return randomHeight;
        }
        
        
        
        //return ((totalFarHeight / maxNumberOfRows) * multiplier) + -60.9f; //encontrando a posição em Y do meteoro
    }
    
    private MeteorDifficulty RandomMeteorDifficulty(int difficulty)
    {
        int randomNumber = Random.Range(1, 6);

        if (difficulty <= 2)
        {
            //Debug.Log("easy");
                return MeteorDifficulty.Easy;
        }

        if (difficulty > 2 && difficulty <= 6)
        {
            if (randomNumber >= 1 && randomNumber <= 2)
            {
                //Debug.Log("easy");
                return MeteorDifficulty.Easy;
            }

            if (randomNumber >= 3 && randomNumber <= 4)
            {
                //Debug.Log("medium");
                return MeteorDifficulty.Medium;
            }
            //Debug.Log("hard");
            return MeteorDifficulty.Hard;
        }

        if (difficulty > 6)
        {
            if (randomNumber >= 1 && randomNumber <= 3)
            {
               // Debug.Log("medium");
                return MeteorDifficulty.Medium;
            }
            // Debug.Log("hard");
            return MeteorDifficulty.Hard;
        }
        
        //if it doesnt enter in any stages, will return here as a easy meteor
        return MeteorDifficulty.Easy;
    }

    public void MeteorActivated(MeteorStats meteorStats)
    {
        activeMeteors.Add(meteorStats);
        answersManager.UnsolvedMeteors.Add(meteorStats);
    }
    public void MeteorDeactivated(MeteorStats meteorStats)
    {
        activeMeteors.Remove(meteorStats);

        if (activeMeteors.Count == 0 && timeEnded)
        {
            levelSelectedManager.SetupNextLevel();
            levelCompletion.LevelCompleted = true;
            gameManager.GameEnded();
        }
        
    }

    public void RemoveQuestionFromList(MathQuestion mathQuestion)
    {
        mathsGenerator.RemoveQuestionFromList(mathQuestion);
    }

}

public struct MeteorData
{
    public float meteorSpeed;
    public Vector3 meteorSize;
    public float meteorDistanceX;
    public float meteorHeightY;
    public float meteorSpawnTime;
    public float meteorSideZAxis;
    public bool rightToLeftMovement;
    public MeteorDifficulty meteorDifficulty;
    public MeteorDistance meteorDistance;
    public MathQuestion mathQuestion;
    public int meteorPointsValue;

    public MeteorData(float speed, Vector3 size, float distanceX, float heightY, float sideZAxis, bool rightMovement, 
                                                        MeteorDifficulty dif, MeteorDistance dist, MathQuestion quest, int points, float spawnTime)
    {
        meteorSpeed = speed;
        meteorSize = size;
        meteorDistanceX = distanceX;
        meteorHeightY = heightY;
        meteorSideZAxis = sideZAxis;
        rightToLeftMovement = rightMovement;
        meteorDifficulty = dif;
        meteorDistance = dist;
        mathQuestion = quest;
        meteorPointsValue = points;
        meteorSpawnTime = spawnTime;
    }
}

public struct MeteorSpeed
{
    public float slowSpeed;
    public float mediumSpeed;
    public float fastSpeed;

    public MeteorSpeed(float slow, float medium, float fast)
    {
        slowSpeed = slow;
        mediumSpeed = medium;
        fastSpeed = fast;
    }
}

public struct MeteorSize
{
    public Vector3 smallSize;
    public Vector3 mediumSize;
    public Vector3 bigSize;
    
    public MeteorSize(Vector3 small, Vector3 medium, Vector3 big)
    {
        smallSize= small;
        mediumSize = medium;
        bigSize = big;
    }
}


