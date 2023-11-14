using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAnimation : MonoBehaviour
{
    [SerializeField] private GameObject crystalGameObject;
    [Space]
    [SerializeField] private float scaleLerpTime = 1f;
    [SerializeField] private Vector3 maxCrystalSize;
    [SerializeField] private Vector3 minCrystalSize;
    [Space]
    [SerializeField] private float moveLerpTime = 1f;
    [SerializeField] private Vector3 maxPosition;
    [SerializeField] private Vector3 minPosition;
    [Space]
    [SerializeField] private float rotationsPerMinute = 10f;
    
    private Coroutine lerpSizeRoutine;
    private Coroutine unlerpSizeRoutine;
    private Coroutine lerpMovementRoutine;
    private Coroutine unlerpMovementRoutine;
    
    private void Start()
    {
        if(lerpSizeRoutine != null)
            StopCoroutine(lerpSizeRoutine);
        
        lerpSizeRoutine = StartCoroutine(LerpSize(crystalGameObject.transform.localScale, maxCrystalSize));
        
        if(lerpMovementRoutine != null)
            StopCoroutine(lerpMovementRoutine);
        
        lerpMovementRoutine = StartCoroutine(LerpPos(crystalGameObject.transform.localPosition, minPosition));
    }

    void Update()
    {
        transform.Rotate(0,6f * rotationsPerMinute * Time.deltaTime,0);
    }
    
    private IEnumerator LerpSize(Vector3 currentScale, Vector3 targetScale)
    {
        float elapsedTime = 0;

        while (elapsedTime < scaleLerpTime)
        {
            elapsedTime += Time.deltaTime;

            crystalGameObject.transform.localScale = Vector3.Lerp(currentScale, targetScale,Mathf.SmoothStep(0.0f, 1.0f, elapsedTime/scaleLerpTime));
            
            yield return null;
        }

        if(unlerpSizeRoutine != null)
            StopCoroutine(unlerpSizeRoutine);
        
        unlerpSizeRoutine = StartCoroutine(UnlerpSize(crystalGameObject.transform.localScale, minCrystalSize));
    }
    
    private IEnumerator UnlerpSize(Vector3 currentScale, Vector3 targetScale)
    {
        float elapsedTime = 0;

        while (elapsedTime < scaleLerpTime)
        {
            elapsedTime += Time.deltaTime;

            crystalGameObject.transform.localScale = Vector3.Lerp(currentScale, targetScale,Mathf.SmoothStep(0.0f, 1.0f, elapsedTime/scaleLerpTime));

            yield return null;
        }

        if(lerpSizeRoutine != null)
            StopCoroutine(lerpSizeRoutine);
            
        lerpSizeRoutine = StartCoroutine(LerpSize(crystalGameObject.transform.localScale, maxCrystalSize));
    }
    
    private IEnumerator LerpPos(Vector3 currentPos, Vector3 targetPos)
    {
        float elapsedTime = 0;

        while (elapsedTime < moveLerpTime)
        {
            elapsedTime += Time.deltaTime;

            crystalGameObject.transform.localPosition = Vector3.Lerp(currentPos, targetPos,Mathf.SmoothStep(0.0f, 1.0f, elapsedTime/moveLerpTime));
            
            yield return null;
        }

        if(unlerpMovementRoutine != null)
            StopCoroutine(unlerpMovementRoutine);
        
        unlerpMovementRoutine = StartCoroutine(UnlerpPos(crystalGameObject.transform.localPosition, maxPosition));
    }
    
    private IEnumerator UnlerpPos(Vector3 currentPos, Vector3 targetPos)
    {
        float elapsedTime = 0;

        while (elapsedTime < moveLerpTime)
        {
            elapsedTime += Time.deltaTime;

            crystalGameObject.transform.localPosition = Vector3.Lerp(currentPos, targetPos,Mathf.SmoothStep(0.0f, 1.0f, elapsedTime/scaleLerpTime));

            yield return null;
        }

        if(lerpMovementRoutine != null)
            StopCoroutine(lerpMovementRoutine);
            
        lerpMovementRoutine = StartCoroutine(LerpPos(crystalGameObject.transform.localPosition, minPosition));
    }
}
