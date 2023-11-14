using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MovementToggle : MonoBehaviour
{
    [Header("SO")]
    [SerializeField] private ActionsTogglerSO actionsToggler; 
    [Space]
    [SerializeField] private CinemachineInputProvider cinemachineInputProv;
    [SerializeField] private PlayerInput playerInput;
    [Space] 
    [SerializeField] private bool lockMovementOnStart;
    [SerializeField] private bool lockCameraOnStart;
    
    public static CinemachineInputProvider staticInputProvider;
    public static PlayerInput input;
    
    private void OnEnable()
    {
        actionsToggler.ToggleOrbit += CameraOrbitActive;
        actionsToggler.ToggleMovement += PlayerMovementActive;
    }

    private void OnDisable()
    {
        actionsToggler.ToggleOrbit -= CameraOrbitActive;
        actionsToggler.ToggleMovement -= PlayerMovementActive;
    }
    
    private void Start()
    {
        staticInputProvider = cinemachineInputProv;
        input = playerInput;

        if (lockMovementOnStart)
            PlayerMovementActive(false);

        if (lockCameraOnStart)
            CameraOrbitActive(false);
    }

    public static void CameraOrbitActive(bool status)
    {
        staticInputProvider.enabled = status;
    }

    public static void PlayerMovementActive(bool status)
    {
        input.enabled = status;
    }
}
