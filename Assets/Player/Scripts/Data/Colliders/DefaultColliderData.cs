using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DefaultColliderData
{
    [field: SerializeField] public float Height { get; private set; } = 2.21f;
    [field: SerializeField] public float CenterY { get; private set; } = 1.12f;
    [field: SerializeField] public float Radius { get; private set; } = 0.34f;


}
