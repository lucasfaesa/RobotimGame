using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PinsController : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private Transform[] pinsPosition;
    [SerializeField] private PinBehaviour[] pinsPool;
    [SerializeField] private Transform pinsPoolParent;
    
    [Header("Prefab")]
    [SerializeField] private PinBehaviour pinPrefab;
    [SerializeField] private Material[] pinsColor;
    [SerializeField] private Sprite[] imagePreview;
    
    [Header("Debug")] 
    [SerializeField] private int numberOfPins;

    private List<PinBehaviour> activePins = new();
    
    public void Test()
    {
        if (numberOfPins > pinsPosition.Length)
            throw new Exception("Numero de pins > posições de pins");
        
        List<Transform> newPinsPos = new(pinsPosition);
        List<PinBehaviour> newPinsPool = new(pinsPool);

        if (newPinsPool.Count < numberOfPins)
        {
            var neededInstantiations = numberOfPins - newPinsPool.Count;

            for (int i = 0; i < neededInstantiations; i++)
            {
                PinBehaviour newPin = Instantiate(pinPrefab, pinsPoolParent);
                newPin.PinsController = this;
                newPin.gameObject.SetActive(false);
                newPinsPool.Add(newPin);
            }
        }
        
        for (int i = 0; i < numberOfPins; i++)
        {
            int randomNumber = Random.Range(0, newPinsPos.Count - 1);

            newPinsPool[i].transform.parent = newPinsPos[randomNumber];
            newPinsPool[i].transform.localPosition = Vector3.zero;
            newPinsPool[i].transform.localRotation = Quaternion.identity;
            newPinsPool[i].transform.localScale = Vector3.one;
            
            newPinsPos.Remove(newPinsPos[randomNumber]);
            
            activePins.Add(newPinsPool[i]);
            
            //settin' pin info #temporary
            bool randomCompleted = Random.Range(0, 2) == 0 ? false : true; 
            newPinsPool[i].SetPinInfo(new PinInfo("teste objetivo da fase","america do sul", "matematica", 
                                                        Random.Range(1,11).ToString(), randomCompleted, 
                                                            imagePreview[0],newPinsPos[randomNumber]));
            //#
            
            newPinsPool[i].gameObject.SetActive(true);
        }
    }

    
}
