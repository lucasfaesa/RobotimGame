using System;
using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrystalPowerController : MonoBehaviour
{
    [SerializeField] private AnswersManagerSO answersManager;
    [SerializeField] private List<CrystalPowerBehavior> pooledCrystalPowerGameObjects = new List<CrystalPowerBehavior>();
    [SerializeField] private CrystalPowerBehavior crystalPowerPrefab;

    private bool activatedCrystalPower;

    private void OnEnable()
    {
        answersManager.correctAnswerMeteor += ActivateHomingCrystalPower;
    }

    private void OnDisable()
    {
        answersManager.correctAnswerMeteor -= ActivateHomingCrystalPower;
    }

    public void ActivateHomingCrystalPower(MeteorStats targetObject)
    {
        activatedCrystalPower = false;
            
        foreach (var crystal in pooledCrystalPowerGameObjects)
        {
            if(crystal.Executing) continue;
                
 
            crystal.ActivateCrystalPower(targetObject.gameObject);
            activatedCrystalPower = true;
            break;
        }

        if (!activatedCrystalPower)
        {
            CrystalPowerBehavior crystalPower = Instantiate(crystalPowerPrefab, Vector3.zero, 
                Quaternion.identity, this.transform);
                
            crystalPower.ActivateCrystalPower(targetObject.gameObject);
                
            pooledCrystalPowerGameObjects.Add(crystalPower);
        }
    }
}
