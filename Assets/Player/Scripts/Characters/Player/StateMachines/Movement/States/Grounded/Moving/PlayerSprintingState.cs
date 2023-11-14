using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintingState : PlayerMovingState
{
    private PlayerSprintData sprintData;

    private bool shouldResetSprintingState;
    
    private float startTime;
    
    private bool keepSprinting;
    
    public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        sprintData = movementData.SprintData;
    }

    #region IState Methpds

    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = sprintData.SpeedModifier;
        base.Enter();
        
        StartAnimation(stateMachine.Player.AnimationData.SprintParameterHash);
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

        shouldResetSprintingState = true;
        startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.SprintParameterHash);
        
        if (shouldResetSprintingState)
        {
            keepSprinting = false;
            stateMachine.ReusableData.ShouldSprint = false;
        }
    }

    public override void Update()
    {
        base.Update();

        if (keepSprinting) return;

        if (Time.time < startTime + sprintData.SprintToRunTime) return;

        StopSprinting();
    }

    #endregion

    #region MainMethods

    private void StopSprinting()
    {
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
            return;
        }
        
        stateMachine.ChangeState(stateMachine.RunningState);
    }

    #endregion
    
    #region Reusable Methods

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        
        stateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
    }
    
    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        
        stateMachine.Player.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
    }

    protected override void OnFall()
    {
        shouldResetSprintingState = false;
        base.OnFall();
    }

    #endregion

    #region InputMethods

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.HardStoppingState);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        shouldResetSprintingState = false;
        base.OnJumpStarted(context);
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        keepSprinting = true;

        stateMachine.ReusableData.ShouldSprint = true;
    }

    #endregion
}
