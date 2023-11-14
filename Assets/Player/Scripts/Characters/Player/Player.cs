using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [field: Header("References")]   
    [field: SerializeField] public PlayerSO Data { get; private set; }
    
    [field: Header("Collisions")]
    [field: SerializeField] public PlayerCapsuleColliderUtility ColliderUtility { get; private set; }
    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
    
    [field: Header("Animation Data")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; } 
        
    [Space]
    [SerializeField] private new Rigidbody rigidbody;
    public Animator animator;
    [SerializeField] private PlayerInput input;
    [Space] 
    [SerializeField] private Transform mainCameraTransform;

    public Transform MainCameraTransform => mainCameraTransform;
    public Rigidbody Rigidbody => rigidbody;
    public PlayerInput Input => input;
    
    private PlayerMovementStateMachine _movementStateMachine;

    private void Awake()
    {
        ColliderUtility.CalculateCapsuleColliderDimensions();
        
        AnimationData.Initialize();
        
        _movementStateMachine = new PlayerMovementStateMachine(this);
    }

    private void OnValidate()
    {
        ColliderUtility.CalculateCapsuleColliderDimensions();
    }

    private void Start()
    {
        _movementStateMachine.ChangeState(_movementStateMachine.IdlingState);
    }

    private void OnTriggerEnter(Collider collider)
    {
        _movementStateMachine.OnTriggerEnter(collider);
    }
    
    private void OnTriggerExit(Collider collider)
    {
        _movementStateMachine.OnTriggerExit(collider);
    }

    private void Update()
    {
        //Debug.Log(transform.position);
        _movementStateMachine.HandleInput();
        _movementStateMachine.Update();
    }
    
    private void FixedUpdate()
    {
        _movementStateMachine.PhysicsUpdate();
    }

    public void OnMovementStateAnimationEnterEvent()
    {
        _movementStateMachine.OnAnimationEnterEvent();
    }
    
    public void OnMovementStateAnimationExitEvent()
    {
        _movementStateMachine.OnAnimationExitEvent();
    }
    
    public void OnMovementStateAnimationTransitionEvent()
    {
        _movementStateMachine.OnAnimationTransitionEvent();
    }
}
