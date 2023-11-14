using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrystalPowerBehavior : MonoBehaviour
{
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private GameObject particleGameObject;
    [SerializeField] private GameObject trailGameObject;
    [SerializeField] private GameObject projectileModelObject;
    [Space]
    [SerializeField] private float journeyTime = 1.0f;
    [SerializeField] private Vector3 targetScale;
    public bool Executing { get; private set; }
    private Coroutine followMeteorRoutine;

    public void ActivateCrystalPower(GameObject targetGameObject)
    {
        FollowMeteor(targetGameObject);
    }

    private void FollowMeteor(GameObject targetGameObject)
    {
        if(followMeteorRoutine != null)
            StopCoroutine(followMeteorRoutine);
        
        followMeteorRoutine = StartCoroutine(SlerpPosition(new Vector3(0,GetRandomCurve(),GetRandomCurve()), targetGameObject));
    }

    private int GetRandomCurve()
    {
        int randomNumber = Random.Range(1, 3);

        switch (randomNumber)
        {
            case 0:
                return 30;
            case 2:
                return -30;
            default:
                return 30;
        }
    }
    
    private IEnumerator SlerpPosition(Vector3 centerOffset, GameObject targetGameObject)
    {
        Executing = true;
        projectileTransform.transform.localPosition = Vector3.zero;
        
        ToggleGameObjects(true);
        projectileTransform.gameObject.SetActive(true);
        
        Vector3 center = (this.transform.localPosition + targetGameObject.transform.position) * 0.5F;
        
        center -= centerOffset;
        
        Vector3 riseRelCenter = this.transform.localPosition - center;
        //Vector3 setRelCenter = targetGameObject.transform.position - center;
        
        float elapsedTime = 0;

        while (elapsedTime < journeyTime)
        {
            elapsedTime += Time.deltaTime;
            
            projectileTransform.transform.localPosition = Vector3.Slerp(riseRelCenter, 
                                                            targetGameObject.transform.position - center, 
                                                                elapsedTime/journeyTime);
            
            projectileTransform.transform.localScale = Vector3.Lerp(Vector3.one, targetScale, elapsedTime / journeyTime);
            projectileTransform.transform.localPosition += center;
            
            yield return null;
        }
        
        projectileModelObject.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        ToggleGameObjects(false);
        
        Executing = false;
    }

    private void ToggleGameObjects(bool status)
    {
        particleGameObject.SetActive(status);
        trailGameObject.SetActive(status);
        projectileModelObject.SetActive(status);
    }
}
