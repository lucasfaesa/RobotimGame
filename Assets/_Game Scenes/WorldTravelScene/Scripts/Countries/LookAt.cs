using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    void Update()
    {
        this.transform.LookAt( 2 * this.transform.position - target.position ) ;
    }
}
