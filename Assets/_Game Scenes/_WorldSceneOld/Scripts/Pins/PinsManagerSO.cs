using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PinsManager", menuName = "ScriptableObjects/LevelSelector/PinsManager", order = 1)]
public class PinsManagerSO : ScriptableObject
{

    public Action<PinInfo> pinSelected;
    public Action pinDeselected;

    public void DeselectPin() //used in context menu hide button
    {
        pinDeselected?.Invoke();
    }

    public void SelectPin(PinInfo info)
    {
        pinSelected?.Invoke(info);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        
    }
}
