using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorToggle : MonoBehaviour
{
    [Header("SO")]
    [SerializeField] private ActionsTogglerSO actionsToggler; 
    [Space]
    [SerializeField] private bool lockOnStart = true;
    [SerializeField] private bool unlockOnStart = false;
    private void OnEnable()
    {
        actionsToggler.ToggleCursor += EnableCursor;
    }

    private void OnDisable()
    {
        actionsToggler.ToggleCursor -= EnableCursor;
    }

    void Start()
    {
        if (lockOnStart)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (unlockOnStart)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void EnableCursor(bool status) 
    {
        if (status)
        {
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
