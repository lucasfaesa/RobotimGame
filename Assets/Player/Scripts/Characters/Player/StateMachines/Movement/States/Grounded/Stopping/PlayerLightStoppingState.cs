using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightStoppingState : PlayerStoppingState
{
    public PlayerLightStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    
    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementDecelarationForce = movementData.StopData.LightDecelerationForce;
        //stateMachine.ReusableData.MovementDecelerationForce = groundedData.StopData.LightDecelerationForce;

        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;
    }
    
    #endregion
}
