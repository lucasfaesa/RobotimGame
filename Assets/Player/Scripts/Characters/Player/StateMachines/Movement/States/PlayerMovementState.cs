using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovementState : IState
{
    protected PlayerMovementStateMachine stateMachine;

    protected PlayerGroundedData movementData;

    protected PlayerAirborneData airborneData;
    
    public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        stateMachine = playerMovementStateMachine;

        movementData = stateMachine.Player.Data.GroundedData;
        airborneData = stateMachine.Player.Data.AirborneData;
        
        InitializeData();
    }

    private void InitializeData()
    {
        SetBaseRotationData();
    }



    #region IState Methods
    public virtual void Enter()
    {
        //Debug.Log("State: " + GetType().Name);

        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }


    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void Update()
    {
       
    }

    public virtual void PhysicsUpdate()
    {
        Move();
    }

    public virtual void OnAnimationEnterEvent()
    {
     
    }

    public virtual void OnAnimationExitEvent()
    {
     
    }

    public virtual void OnAnimationTransitionEvent()
    {
    
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGround(collider);
            return;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGroundExited(collider);
            return;
        }
    }

    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
    }

    public void OnTriggerExit()
    {
       
    }

    #endregion
    
    #region Main Methods
    private void ReadMovementInput()
    {
        stateMachine.ReusableData.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
    }
    
    private void Move()
    {
        if (stateMachine.ReusableData.MovementInput == Vector2.zero || stateMachine.ReusableData.MovementSpeedModifier == 0f) return;

        Vector3 movementDirection = GetMovementDirection();

        float targetRotationYAngle = Rotate(movementDirection);

        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
        
        float movementSpeed = GetMovementSpeed();

        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
        
        stateMachine.Player.Rigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
    }
    

    private float Rotate(Vector3 direction)
    {
        var directionAngle = UpdateTargetRotation(direction);

        RootateTowardsTargetRotation();
        
        return directionAngle;
    }
    
    private void UpdateTargetRotationData(float targetAngle)
    {
        stateMachine.ReusableData.CurrentTargetRotation.y = targetAngle;

        stateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
    }

    private float AddCameraRotationToAngle(float angle)
    {
        angle += stateMachine.Player.MainCameraTransform.eulerAngles.y;

        if (angle > 360f)
            angle -= 360f;
        
        return angle;
    }

    private float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (directionAngle < 0f)
            directionAngle += 360f;
        
        return directionAngle;
    }

    #endregion
    
    #region Reusable Methods

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.animator.SetBool(animationHash,true);
    }
    
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.animator.SetBool(animationHash,false);
    }
    
    private Vector3 GetMovementDirection()
    {
        return new Vector3(stateMachine.ReusableData.MovementInput.x,0f, stateMachine.ReusableData.MovementInput.y);
    }
    
    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 playerHorizontalVelocity = stateMachine.Player.Rigidbody.velocity;
        playerHorizontalVelocity.y = 0f;
        return playerHorizontalVelocity;
        
    }

    protected Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0f, stateMachine.Player.Rigidbody.velocity.y, 0f);
    }
    
    protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
    {
        float movementSpeed = movementData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier;

        if (shouldConsiderSlopes)
        {
            movementSpeed *= stateMachine.ReusableData.MovementOnSlopesSpeedModifier;
        }
        return movementSpeed;
    }
    
    protected private void RootateTowardsTargetRotation()
    {
        float currentYAngle = stateMachine.Player.Rigidbody.rotation.eulerAngles.y;

        if (currentYAngle == stateMachine.ReusableData.CurrentTargetRotation.y)
            return;

        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.ReusableData.CurrentTargetRotation.y,
            ref stateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y, stateMachine.ReusableData.TimeToReachTargetRotation.y - stateMachine.ReusableData.DampedTargetRotationPassedTime.y);

        stateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;
        
        Quaternion targetRotation = Quaternion.Euler(0f,smoothedYAngle, 0f);
        
        stateMachine.Player.Rigidbody.MoveRotation(targetRotation);
    }
    
    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        var directionAngle = GetDirectionAngle(direction);

        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }

        if (directionAngle != stateMachine.ReusableData.CurrentTargetRotation.y)
            UpdateTargetRotationData(directionAngle);

        return directionAngle;
    }

    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    protected void ResetVelocity()
    {
        stateMachine.Player.Rigidbody.velocity = Vector3.zero;
    }

    protected void ResetVerticalVelocity()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        stateMachine.Player.Rigidbody.velocity = playerHorizontalVelocity;
    }
    
    protected virtual void AddInputActionsCallbacks()
    {
        //stateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        //stateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;
    }

    protected void DecelerateHorizontally()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        
        stateMachine.Player.Rigidbody.AddForce(-playerHorizontalVelocity * stateMachine.ReusableData.MovementDecelarationForce, ForceMode.Acceleration);
    }
    
    protected void DecelerateVertically()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
        
        stateMachine.Player.Rigidbody.AddForce(-playerVerticalVelocity * stateMachine.ReusableData.MovementDecelarationForce, ForceMode.Acceleration);
    }

    protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);

        return playerHorizontalMovement.magnitude > minimumMagnitude;
    }

    protected bool IsMovingUp(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y > minimumVelocity;
    }
    
    protected bool IsMovingDown(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y < -minimumVelocity;
    }
    
    protected void SetBaseRotationData()
    {
        stateMachine.ReusableData.RotationData = movementData.BaseRotationData;
        stateMachine.ReusableData.TimeToReachTargetRotation = stateMachine.ReusableData.RotationData.TargetRotationReachTime;
    }
    
    protected virtual void OnContactWithGround(Collider collider)
    {
    }
    
    protected virtual void OnContactWithGroundExited(Collider collider)
    {
       
    }
    #endregion
    
    #region InputMethods
    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        stateMachine.ReusableData.ShouldWalk = !stateMachine.ReusableData.ShouldWalk;
    }
    #endregion
}
