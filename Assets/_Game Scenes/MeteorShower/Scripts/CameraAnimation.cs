using System;
using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] private GameManagerSO gameManager;
    [Space]
    [SerializeField] private Camera mainCamera;
    [Space] 
    [SerializeField] private float targetFov;
    [SerializeField] private Vector3 targetPos;
    [Space]
    [SerializeField] private float fovLerpTime = 1f;
    [SerializeField] private float moveLerpTime = 1f;

    private Coroutine animCameraRoutine;
    [Space] 
    [SerializeField] private UnityEvent cameraAnimationEnded;

    private void OnEnable()
    {
        gameManager.preparingToStartGame += Animate;
    }

    private void OnDisable()
    {
        gameManager.preparingToStartGame -= Animate;
    }
    
    private void Animate()
    {
        if(animCameraRoutine != null)
            StopCoroutine(animCameraRoutine);

        animCameraRoutine = StartCoroutine(AnimateCamera());
    }

    private IEnumerator AnimateCamera()
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0;
        float initialFov = mainCamera.fieldOfView;
        
        while (elapsedTime < fovLerpTime)
        {
            elapsedTime += Time.deltaTime;

            mainCamera.fieldOfView = Mathf.Lerp(initialFov, targetFov,Mathf.SmoothStep(0.0f, 1.0f, elapsedTime/fovLerpTime));

            yield return null;
        }

        elapsedTime = 0;
        Vector3 initialPos = mainCamera.transform.localPosition;
        while (elapsedTime < moveLerpTime)
        {
            elapsedTime += Time.deltaTime;

            mainCamera.transform.localPosition = Vector3.Lerp(initialPos, targetPos,Mathf.SmoothStep(0.0f, 1.0f, elapsedTime/moveLerpTime));

            yield return null;
        }

        gameManager.GameStarted();
        cameraAnimationEnded?.Invoke();
        animCameraRoutine = null;
    }
}
