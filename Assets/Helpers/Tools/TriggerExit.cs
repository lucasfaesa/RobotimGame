using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class TriggerExit : MonoBehaviour
{
    [SerializeField] private string tagToCompare;
    [SerializeField] private bool debugCollision;
    [SerializeField] private UnityEvent triggerExited;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagToCompare))
        {
            if(debugCollision)
                Debug.Log("Left collision with: " + other.gameObject.name);
            
            triggerExited?.Invoke();
        }
    }
}
