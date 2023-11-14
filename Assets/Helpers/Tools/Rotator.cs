using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 angularVelocity;
    [SerializeField] private Space space = Space.Self;
    void Update ()
    {
        transform.Rotate(angularVelocity * Time.deltaTime, space);
    }
}
