using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class MissileBehavior : MonoBehaviour
{
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private ParticleSystem missileParticle;
    [Space]
    [SerializeField] private float journeyTime = 1.0f;
    [SerializeField] private Vector3 targetScale;

    private Vector3 defaultPos;
    private Vector3 defaultScale;
    private Transform defaultParent;

    private float elapsedTime;
    
    private Coroutine shootMissileRoutine;
    private Vector3 startPos;
    private Vector3 startScale;

    private void Start()
    {
        var transform1 = projectileTransform.transform;
        defaultParent = transform1.parent;
        defaultPos = transform1.localPosition;
        defaultScale = transform1.localScale;
        startScale = defaultScale;
    }

    public void Shoot(GameObject targetGameObject)
    {
        FollowTarget(targetGameObject);
    }

    private void FollowTarget(GameObject targetGameObject)
    {
        if(shootMissileRoutine != null)
            StopCoroutine(shootMissileRoutine);

        shootMissileRoutine = StartCoroutine(LerpPosition(targetGameObject));
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shoot(targetTransform.gameObject);
        }
    }

    private IEnumerator LerpPosition(GameObject targetGameObject)
    {
        missileParticle.Play();
        var missile = missileParticle.emission;
        missile.rateOverTime = 150f;
        
        var transform1 = projectileTransform.transform;
        
        transform1.parent = defaultParent;
        transform1.localPosition = defaultPos;
        transform1.localScale = Vector3.one;
        
        transform1.parent = null;
        startScale = defaultScale;
        startPos = projectileTransform.transform.position;
        
        var targetPos = targetGameObject.transform.position;

        elapsedTime = 0;

        while (elapsedTime < journeyTime)
        {
            elapsedTime += Time.deltaTime;
            
            projectileTransform.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime/journeyTime);
            projectileTransform.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / journeyTime);
            
            yield return null;
        }

        
        missile.rateOverTime = 0f;
    }

    public void SlowDown()
    {
        startPos = projectileTransform.transform.position;
        startScale = projectileTransform.transform.localScale;
        
        elapsedTime = journeyTime;
        journeyTime = 30f;

        var main = missileParticle.main;
        main.simulationSpeed = 0.1f;
    }

    public void NormalSpeed()
    {
        startPos = projectileTransform.transform.position;
        startScale = projectileTransform.transform.localScale;

        elapsedTime = elapsedTime / journeyTime;
        
        journeyTime = 1f;
        //elapsedTime = journeyTime * time;
        
        var main = missileParticle.main;
        main.simulationSpeed = 1f;
    }
    

}
