using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions PlayerActions { get; private set; }

    [SerializeField] private bool canPlayerJump = true;
    
    private void Awake()
    {
        InputActions = new PlayerInputActions();

        PlayerActions = InputActions.Player;
    }

    private void OnEnable()
    {
        InputActions.Enable();
        
        if(!canPlayerJump)
            PlayerActions.Jump.Disable();
        else
            PlayerActions.Jump.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }

    public void DisableActionFor(InputAction action, float seconds)
    {
        StartCoroutine(DisableAction(action, seconds));
    }

    private IEnumerator DisableAction(InputAction action, float seconds)
    {
        action.Disable();

        yield return new WaitForSeconds(seconds);

        action.Enable();
    }
}
