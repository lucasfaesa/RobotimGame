using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class KeyPressController : MonoBehaviour
{
    [Header("For every key you add, add one item in the 'Key Pressed Events' for it")]
    [SerializeField] private List<KeyCode> keysList = new List<KeyCode>();
    
    [SerializeField] private List<UnityEvent> keyPressedEvents;
    
    
    
    void Update()
    {
        if (keysList.Count == 0) return;

        for (int i = 0; i < keysList.Count; i++)
        {
            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
                if(Input.GetKey(vKey)){
                    if(vKey == keysList[i])
                        keyPressedEvents[i]?.Invoke();
                }
            }
        }

        KeyControl ok = new KeyControl();
        
    }
}