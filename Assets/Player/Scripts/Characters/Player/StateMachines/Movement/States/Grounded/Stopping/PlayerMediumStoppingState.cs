using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMediumStoppingState : PlayerStoppingState
{
    public PlayerMediumStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.MediumStopParameterHash);

        stateMachine.ReusableData.MovementDecelarationForce = movementData.StopData.MediumDecelerationForce;

        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.MediumForce;
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.MediumStopParameterHash);
    }

}
