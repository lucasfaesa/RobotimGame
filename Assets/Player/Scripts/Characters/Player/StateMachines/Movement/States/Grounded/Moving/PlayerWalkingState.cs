using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerMovingState
{
    private PlayerWalkData walkData;
    public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        walkData = movementData.WalkData;
    }

    #region IStateMethods
    
    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = walkData.SpeedModifier;
        
        base.Enter();
        
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;
        
    }
    #endregion

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    #region InputMethods

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.LightStoppingState);
    }

    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);
        
        stateMachine.ChangeState(stateMachine.RunningState);
    }

    #endregion

}
