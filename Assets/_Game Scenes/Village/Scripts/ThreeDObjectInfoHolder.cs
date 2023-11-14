using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ThreeDObjectInfoHolder : MonoBehaviour
{
    [SerializeField] private ThreeDObjectInfoSO threeDObjectInfo;
    [SerializeField] private GameObject model;
    [Space]
    [SerializeField] private bool rotate;
    [SerializeField] private Vector3 rotateAmount;
    public ThreeDObjectInfoSO Get3DObjectInfo() => threeDObjectInfo;

    private void Start()
    {
        model.transform.DOLocalRotate(rotateAmount, 30f, RotateMode.LocalAxisAdd).SetLoops(-1,LoopType.Restart).SetEase(Ease.Linear);
    }
}
