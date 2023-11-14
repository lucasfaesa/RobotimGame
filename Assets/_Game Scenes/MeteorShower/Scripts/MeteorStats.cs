using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using _MeteorShower.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class MeteorStats : MonoBehaviour
{
    [SerializeField] private AnswersManagerSO answersManager;
    [SerializeField] private PointsManagerSO pointsManager;
    [Space]
    [SerializeField] private GameObject meteorModelObject;
    [SerializeField] private Renderer meteorRenderer;
    [SerializeField] private TextMeshPro meteorText;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private TextMeshPro pointsValueText;
    
    [SerializeField] private Color32[] meteorColorsDifficulty;

    private MeteorsSpawnController meteorsSpawnController = null;

    private Vector3 rotationsPerMinute;
    private bool isMeteorReady;
    public MeteorData MeteorData { get; private set; }

    private float timeActive = 0;

    private float maxCloseDistanceLeft = -86f;
    private float maxCloseDistanceRight = 86f;

    private float maxMediumDistanceLeft = -134f;
    private float maxMediumDistanceRight = 134f;

    private float maxFarDistanceLeft = -178.4f;
    private float maxFarDistanceRight = 178.4f;
        
    /*private const float closeDistance =  -72f;
    private const float mediumDistance = -115.3f;
    private const float farDistance = -158f;*/

    private readonly float fontSizeClose = 37f;
    private readonly float fontSizeMedium  = 60f;
    private readonly float fontSizeFar = 83f;
    
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private Coroutine explodeRoutine;
    private Coroutine waitForCrystalPowerJourney;
    
    public bool BeingChasedBy { get; private set; }

    public MathQuestion GetMathQuestion => MeteorData.mathQuestion;
    
    public void InjectMeteorsSpawnController(MeteorsSpawnController mSc)
    {
        if(meteorsSpawnController == null)
            meteorsSpawnController = mSc;
    }

    public void ActivateMeteor(MeteorData data)
    {
        rotationsPerMinute.x = Random.Range(5, 18);
        rotationsPerMinute.y = Random.Range(5, 18);
        rotationsPerMinute.z = Random.Range(5, 18);

        AdaptCanvasSize(data.meteorDistance);
        TintMeteor(data.meteorDifficulty);
        
        MeteorData = data;
        meteorText.text = MeteorData.mathQuestion.questionText + " = ?";
        
        var localPosition = this.transform.localPosition;
        
        meteorModelObject.transform.localScale = data.meteorSize;
        this.transform.localPosition = new Vector3(data.meteorDistanceX, MeteorData.meteorHeightY, data.meteorSideZAxis);
        
        SetMeteorActive();
        
        timeActive = Time.time;
        isMeteorReady = true;
    }

    private void Update()
    {
        if (!isMeteorReady) return;

        Translate();
        Rotate();
    }

    public void BeingChasedByCrystalPower()
    {
        float journeyTime = 1f;
        BeingChasedBy = true;

        meteorsSpawnController.RemoveQuestionFromList(MeteorData.mathQuestion);
        answersManager.RemoveFromUnsolvedList(this);

        if(waitForCrystalPowerJourney != null)
            StopCoroutine(waitForCrystalPowerJourney);

        waitForCrystalPowerJourney = StartCoroutine(WaitForCrystalPowerJourneyTime(journeyTime));
    }

    private IEnumerator WaitForCrystalPowerJourneyTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        ExplodeMeteor();
    }
    
    private void ExplodeMeteor()
    { 
        if(explodeRoutine != null)
            StopCoroutine(explodeRoutine);

        explodeRoutine = StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        //Debug.Log("Meteor Question: " + meteorData.mathQuestion.questionText + " Result: " + meteorData.mathQuestion.questionResult);

        var timePenalty = Mathf.Clamp(((int)Time.time - ((int)timeActive + 10)/*Esse 10 é um threshold, como se demorasse 10 segundos para começar a contar o tempo ativo do meteoro*/),
                                                            0, MeteorData.meteorPointsValue/3); //valor máximo de punição é um terço do valor do meteoro 
        
        Debug.Log(MeteorData.meteorPointsValue - timePenalty);
        pointsManager.AddPoints(MeteorData.meteorPointsValue - timePenalty);
        
        isMeteorReady = false;
        
        Vector3 defaultTextPos = pointsValueText.transform.localPosition;
        
        pointsValueText.gameObject.SetActive(true);
        
        meteorModelObject.SetActive(false);
        meteorText.gameObject.SetActive(false);
        
        explosionParticle.transform.parent = null;
        explosionParticle.Play();

        pointsValueText.transform.SetParent(null);
        pointsValueText.transform.position = this.transform.position;
        pointsValueText.text = "+ " + (MeteorData.meteorPointsValue - timePenalty);
        
        bool sequencePlaying = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(pointsValueText.DOFade(0f, 1.7f).SetEase(Ease.InOutSine));
        sequence.Insert(0, pointsValueText.transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 1.7f).SetEase(Ease.InOutSine));
        sequence.Insert(0, pointsValueText.transform.DOMoveY(pointsValueText.transform.position.y + 1.2f, 1.7f).SetEase(Ease.Linear));
        sequence.OnComplete(() =>
        {
            sequencePlaying = false;
        });
        
        while (explosionParticle.isPlaying || sequencePlaying)
        {
            yield return null;
        }

        pointsValueText.transform.SetParent(this.transform);
        pointsValueText.transform.localPosition = defaultTextPos;
        
        pointsValueText.gameObject.SetActive(false);
        pointsValueText.alpha = 1f;
        
        explosionParticle.transform.parent = this.transform;
        explosionParticle.transform.localPosition = Vector3.zero;
        
        BeingChasedBy = false;

        meteorsSpawnController.MeteorDeactivated(this);
    }

    private void TintMeteor(MeteorDifficulty meteorDifficulty)
    {
        switch (meteorDifficulty)
        {
            case MeteorDifficulty.Easy:
                meteorRenderer.material.SetColor(BaseColor, meteorColorsDifficulty[0]);
                break;
            case MeteorDifficulty.Medium:
                meteorRenderer.material.SetColor(BaseColor, meteorColorsDifficulty[1]);
                break;
            case MeteorDifficulty.Hard:
                meteorRenderer.material.SetColor(BaseColor, meteorColorsDifficulty[2]);
                break;
        }
    }
    
    private void AdaptCanvasSize(MeteorDistance meteorDistance)
    {
        switch (meteorDistance)
        {
            case MeteorDistance.Close:
                meteorText.fontSize = fontSizeClose;
                pointsValueText.fontSize = fontSizeClose;
                break;
            case MeteorDistance.Medium:
                meteorText.fontSize = fontSizeMedium;
                pointsValueText.fontSize = fontSizeMedium;
                break;
            case MeteorDistance.Far:
                meteorText.fontSize = fontSizeFar;
                pointsValueText.fontSize = fontSizeFar;
                break;
        }
    }
    private void Translate()
    {
        if (MeteorData.rightToLeftMovement)
        {
            this.transform.Translate(Vector3.back * (MeteorData.meteorSpeed * Time.deltaTime));
            
            switch (MeteorData.meteorDistance)
            {
                case MeteorDistance.Close:
                    if(this.transform.localPosition.z <= maxCloseDistanceLeft)
                        DeactivateMeteor();
                    break;
                
                case MeteorDistance.Medium:
                    if(this.transform.localPosition.z <= maxMediumDistanceLeft)
                        DeactivateMeteor();
                    break;
                
                case MeteorDistance.Far:
                    if(this.transform.localPosition.z <= maxFarDistanceLeft)
                        DeactivateMeteor();
                    break;
            }
        }
        else
        {
            this.transform.Translate(Vector3.forward * (MeteorData.meteorSpeed * Time.deltaTime));
            
            switch (MeteorData.meteorDistance)
            {
                case MeteorDistance.Close:
                    if(this.transform.localPosition.z >= maxCloseDistanceRight)
                        DeactivateMeteor();
                    break;
                
                case MeteorDistance.Medium:
                    if(this.transform.localPosition.z >= maxMediumDistanceRight)
                        DeactivateMeteor();
                    break;
                
                case MeteorDistance.Far:
                    if(this.transform.localPosition.z >= maxFarDistanceRight)
                        DeactivateMeteor();
                    break;
            }
        }
    }

    private void Rotate()
    {
        meteorModelObject.transform.Rotate(rotationsPerMinute.x * Time.deltaTime,rotationsPerMinute.y * Time.deltaTime,rotationsPerMinute.z * Time.deltaTime);
    }
    
    private void SetMeteorActive()
    {
        meteorModelObject.SetActive(true);
        meteorText.gameObject.SetActive(true);
        meteorsSpawnController.MeteorActivated(this);
    }
    
    private void DeactivateMeteor()
    {
        if (BeingChasedBy) return;
        
        isMeteorReady = false;
        meteorsSpawnController.MeteorDeactivated(this);
        meteorsSpawnController.RemoveQuestionFromList(MeteorData.mathQuestion);
        answersManager.RemoveFromUnsolvedList(this);
        
        meteorModelObject.SetActive(false);
        meteorText.gameObject.SetActive(false);
    }

    public bool IsMeteorActive()
    {
        return meteorModelObject.activeSelf || explosionParticle.isPlaying;
    }

}
