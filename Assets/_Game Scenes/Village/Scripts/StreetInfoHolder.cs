using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StreetInfoHolder : MonoBehaviour
{
    [SerializeField] private StreetInfoSO streetInfo;
    [SerializeField] private StreetsManager streetsManager;
    //[SerializeField] private List<StreetInfoSO> perpendicularStreets;

    public void SetStreetName(string streetName) => streetInfo.StreetName = streetName;
    public string GetStreetName() => streetInfo.StreetName;

    //public List<StreetInfoSO> GetPerpendicularStreets() => perpendicularStreets;
    
    
    #region Trigger Detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            streetsManager.PlayerEnteredStreet(streetInfo.StreetName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            streetsManager.PlayerLeftStreet(streetInfo.StreetName);
        }
    }
    #endregion
}
