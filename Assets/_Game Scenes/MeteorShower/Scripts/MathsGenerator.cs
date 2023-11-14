using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MathsGenerator : MonoBehaviour
{
    [SerializeField] private List<MathQuestion> mathQuestions = new List<MathQuestion>();

    public List<MathQuestion> ActiveQuestions => mathQuestions;
    
    public MathQuestion GenerateMathQuestion(MeteorDifficulty meteorDifficulty)
    {
        int randomNumber = Random.Range(0, 5);
        MathQuestion generatedQuestion = new MathQuestion();

        switch (randomNumber)
        {
            case 0:
                generatedQuestion = GenerateSumQuestion(meteorDifficulty);
                break;
            case 1:
                generatedQuestion = GenerateSubtractionQuestion(meteorDifficulty);
                break;
            case 2:
                generatedQuestion = GenerateDivisionQuestion(meteorDifficulty);
                break;
            case 3:
                generatedQuestion = GenerateMultiplicationQuestion(meteorDifficulty);
                break;
            default:
                generatedQuestion = GenerateSumQuestion(meteorDifficulty);
                break;
        }

        mathQuestions.Add(generatedQuestion);
        return generatedQuestion;
    }

    public void RemoveQuestionFromList(MathQuestion question)
    {
        mathQuestions.Remove(question);
    }

    private MathQuestion GenerateSumQuestion(MeteorDifficulty difficulty)
    {
        int randomNumber1 = 0;
        int randomNumber2 = 0;
        
        switch (difficulty)
        {
            case MeteorDifficulty.Easy:
                randomNumber1 = Random.Range(0, 11); //0 a 9
                randomNumber2 = Random.Range(0, 101);
                return new MathQuestion((randomNumber1 + " + " + randomNumber2), (randomNumber1 + randomNumber2));
            case MeteorDifficulty.Medium:
                randomNumber1 = Random.Range(4, 11); //4 a 9
                randomNumber2 = Random.Range(0, 101);
                return new MathQuestion((randomNumber1 + " + " + randomNumber2), (randomNumber1 + randomNumber2));
            case MeteorDifficulty.Hard:
                randomNumber1 = Random.Range(0, 101); //0 a 100
                randomNumber2 = Random.Range(0, 101);
                return new MathQuestion((randomNumber1 + " + " + randomNumber2), (randomNumber1 + randomNumber2));
            default:
                randomNumber1 = Random.Range(0, 10); //0 a 9
                randomNumber2 = Random.Range(0, 101);
                Debug.Log("Entered Default in Sum");
                return new MathQuestion((randomNumber1 + " + " + randomNumber2), (randomNumber1 + randomNumber2));
        }
    }
    
    private MathQuestion GenerateSubtractionQuestion(MeteorDifficulty difficulty)
    {
        int randomNumber1 = 0;
        int randomNumber2 = 0;
        
        switch (difficulty)
        {
            case MeteorDifficulty.Easy:
                randomNumber1 = Random.Range(0, 10); //0 a 9
                randomNumber2 = Random.Range(0, randomNumber1 + 1); //não deixa gerar numeros negativos
                return new MathQuestion((randomNumber1 + " - " + randomNumber2), (randomNumber1 - randomNumber2));
            case MeteorDifficulty.Medium:
                randomNumber1 = Random.Range(4, 10); //4 a 9
                randomNumber2 = Random.Range(4, randomNumber1 + 1);
                return new MathQuestion((randomNumber1 + " - " + randomNumber2), (randomNumber1 - randomNumber2));
            case MeteorDifficulty.Hard:
                randomNumber1 = Random.Range(9, 101); //0 a 100
                randomNumber2 = Random.Range(9, randomNumber1 + 1);
                return new MathQuestion((randomNumber1 + " - " + randomNumber2), (randomNumber1 - randomNumber2));
            default:
                randomNumber1 = Random.Range(0, 10); //0 a 9
                randomNumber2 = Random.Range(0, randomNumber1 + 1);
                Debug.Log("Entered Default in Subtraction");
                return new MathQuestion((randomNumber1 + " - " + randomNumber2), (randomNumber1 - randomNumber2));
        }
    }
    
    private MathQuestion GenerateMultiplicationQuestion(MeteorDifficulty difficulty)
    {
        int randomNumber1 = 0;
        int randomNumber2 = 0;
        
        switch (difficulty)
        {
            case MeteorDifficulty.Easy:
                randomNumber1 = Random.Range(0, 5); //0 a 4
                randomNumber2 = Random.Range(0, 6); 
                return new MathQuestion((randomNumber1 + " x " + randomNumber2), (randomNumber1 * randomNumber2));
            case MeteorDifficulty.Medium:
                randomNumber1 = Random.Range(3, 6); //3 a 5
                randomNumber2 = Random.Range(3, 13);
                return new MathQuestion((randomNumber1 + " x " + randomNumber2), (randomNumber1 * randomNumber2));
            case MeteorDifficulty.Hard:
                randomNumber1 = Random.Range(2, 11); //2 a 10
                randomNumber2 = Random.Range(6, 12);
                return new MathQuestion((randomNumber1 + " x " + randomNumber2), (randomNumber1 * randomNumber2));
            default:
                randomNumber1 = Random.Range(0, 5); //0 a 4
                randomNumber2 = Random.Range(0, 6); 
                Debug.Log("Entered Default in Multiplication");
                return new MathQuestion((randomNumber1 + " x " + randomNumber2), (randomNumber1 * randomNumber2));
        }
    }
    
    private MathQuestion GenerateDivisionQuestion(MeteorDifficulty difficulty)
    {
        int randomNumber1 = 0;
        int randomNumber2 = 0;
        
        switch (difficulty)
        {
            case MeteorDifficulty.Easy:
                randomNumber1 = Random.Range(1, 11); //1 a 10
                randomNumber2 = Random.Range(1, 4); //1 a 3 
                while ((randomNumber1 % randomNumber2) != 0) //só deixa gerar divisões inteiras
                {
                    randomNumber1 = Random.Range(1, 11);
                    randomNumber2 = Random.Range(1, 4);
                }
                return new MathQuestion((randomNumber1 + " / " + randomNumber2), (randomNumber1 / randomNumber2));
            case MeteorDifficulty.Medium:
                randomNumber1 = Random.Range(3, 21); 
                randomNumber2 = Random.Range(1, 11);
                while ((randomNumber1 % randomNumber2) != 0) //só deixa gerar divisões inteiras
                {
                    randomNumber1 = Random.Range(3, 21); 
                    randomNumber2 = Random.Range(1, 11);
                }
                return new MathQuestion((randomNumber1 + " / " + randomNumber2), (randomNumber1 / randomNumber2));
            case MeteorDifficulty.Hard:
                randomNumber1 = Random.Range(3, 51); 
                randomNumber2 = Random.Range(3, 51);
                while ((randomNumber1 % randomNumber2) != 0) //só deixa gerar divisões inteiras
                {
                    randomNumber1 = Random.Range(3, 51);
                    randomNumber2 = Random.Range(3, 51);
                }
                return new MathQuestion((randomNumber1 + " / " + randomNumber2), (randomNumber1 / randomNumber2));
            default:
                randomNumber1 = Random.Range(1, 11); //1 a 10
                randomNumber2 = Random.Range(1, 4); //1 a 3 
                while ((randomNumber1 % randomNumber2) != 0) //só deixa gerar divisões inteiras
                {
                    randomNumber1 = Random.Range(1, 11);
                    randomNumber2 = Random.Range(1, 4);
                }
                Debug.Log("Entered Default in Multiplication");
                return new MathQuestion((randomNumber1 + " / " + randomNumber2), (randomNumber1 / randomNumber2));
        }
    }
}

[Serializable]
public struct MathQuestion
{
    public string questionText;
    public int questionResult;

    public MathQuestion(string text, int result)
    {
        questionText = text;
        questionResult = result;
    }
}
