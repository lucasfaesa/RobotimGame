using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CinemachineInputProvider inputProvider;
    [Space]
    [SerializeField] [Range(0f,10f)] private float defaultDistance = 6f;
    [SerializeField] [Range(0f,10f)] private float minimumDistance = 1f;
    [SerializeField] [Range(0f,10f)] private float maximumDistance = 6f;
    [Space]
    [SerializeField] [Range(0f,10f)] private float smoothing = 4f;
    [SerializeField] [Range(0f,10f)] private float zoomSensitivity = 1f;

    private float currentTargetDistance;
    private CinemachineFramingTransposer framingTransposer;
    
    private void Awake()
    {
        framingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        currentTargetDistance = defaultDistance;
    }

    private void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        float zoomValue = inputProvider.GetAxisValue(2) * zoomSensitivity;
        currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minimumDistance, maximumDistance);

        float currentDistance = framingTransposer.m_CameraDistance;

        if (currentDistance == currentTargetDistance) return;

        float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);

        framingTransposer.m_CameraDistance = lerpedZoomValue;
    }
}
