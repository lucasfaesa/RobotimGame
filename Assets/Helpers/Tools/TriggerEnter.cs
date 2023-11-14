using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEnter : MonoBehaviour
{
    [SerializeField] private string tagToCompare;
    [SerializeField] private bool debugCollision;
    
    [SerializeField] private UnityEvent triggerEntered;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToCompare))
        {
            if(debugCollision)
                Debug.Log("Collision entered with: " + other.gameObject.name);
            triggerEntered?.Invoke();
        }
    }
}
