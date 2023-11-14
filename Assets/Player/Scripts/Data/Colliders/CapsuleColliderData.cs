using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CapsuleColliderData
{
    [field: SerializeField] public CapsuleCollider Collider { get; private set; }
    public Vector3 ColliderCenterInLocalSpace { get; private set; }

    public Vector3 ColliderVerticalExtents { get; private set; }
    
    public void UpdateColliderData()
    {
        ColliderCenterInLocalSpace = Collider.center;

        ColliderVerticalExtents = new Vector3(0f, Collider.bounds.extents.y, 0f);
    }
}
