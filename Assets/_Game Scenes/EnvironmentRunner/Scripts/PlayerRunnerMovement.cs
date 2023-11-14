using System;
using System.Collections;
using _MeteorShower.Scripts;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class PlayerRunnerMovement : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManagerSo;
        [Space]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Animator playerAnimator;
        [Header("Collision")]
        [SerializeField] private CapsuleCollider playerCollider;
        [SerializeField] private float defaultColliderYPos;
        [SerializeField] private float slidingColliderYPos;
        [Space] 
        [SerializeField] private float leftXPos;
        [SerializeField] private float centerXPos;
        [SerializeField] private float rightXPos;
        [SerializeField] private float timeToMove = 1f;
        [SerializeField] private float slideDuration = 1f;
        [SerializeField] private float playerDefaultYPos = 0.47f;
        [SerializeField] private float playerHighestJumpYPos = 3.34f;
        [SerializeField] private float jumpDuration = 1f;
        
        private PlayerInputActions playerInputActions;

        private Sequence slideSequence;
        private Sequence jumpUpSequence;
        private Sequence jumpDownSequence;
        float valueForSlideReference = 0;
        
        private bool sliding;
        private bool jumping;
        private bool grounded;
        private bool wantToSlideAfterDescent;
        private bool lerpingToLane;

        private Sequence laneLeftSequence;
        private Sequence laneRightSequence;
        
        //0 left, 1 middle, 2 right
        private int currentLane = 1;
        private void OnEnable()
        {
            gameManagerSo.gameGoingToEnd += DisablePlayerActions;
            
            playerInputActions = new PlayerInputActions();
            playerInputActions.Enable();
        }

        private void OnDisable()
        {
            gameManagerSo.gameGoingToEnd -= DisablePlayerActions;
            DisablePlayerActions();
        }

        private void DisablePlayerActions()
        {
            playerInputActions.Disable();
        }

        private void Start()
        {
            slideSequence = DOTween.Sequence().Append((DOTween.To(x => valueForSlideReference = x, 
                                                0, 1, slideDuration))).Pause().SetAutoKill(false);
            
            jumpUpSequence = DOTween.Sequence().Append(playerTransform.DOMoveY(playerHighestJumpYPos,
                jumpDuration / 2).SetEase(Ease.OutSine)).Pause().SetAutoKill(false);
            
            jumpDownSequence = DOTween.Sequence().Append(playerTransform.DOMoveY(playerDefaultYPos,
                jumpDuration / 2).SetEase(Ease.InOutSine)).Pause().SetAutoKill(false);

            laneLeftSequence = DOTween.Sequence()
                .Append(playerTransform
                    .DORotate(new Vector3(playerTransform.rotation.x, -40, playerTransform.rotation.z), timeToMove/2)
                    .SetEase(Ease.InOutSine)).Pause();
            laneLeftSequence.Append(playerTransform
                .DORotate(new Vector3(playerTransform.rotation.x, 0, playerTransform.rotation.z), timeToMove/2)
                .SetEase(Ease.InOutSine)).Pause().SetAutoKill(false);
            
            laneRightSequence = DOTween.Sequence()
                .Append(playerTransform
                    .DORotate(new Vector3(playerTransform.rotation.x, 40, playerTransform.rotation.z), timeToMove/2)
                    .SetEase(Ease.InOutSine)).Pause();
            laneRightSequence.Append(playerTransform
                .DORotate(new Vector3(playerTransform.rotation.x, 0, playerTransform.rotation.z), timeToMove/2)
                .SetEase(Ease.InOutSine)).Pause().SetAutoKill(false);
        }

        private void Update()
        {
            //Debug.Log(playerInputActions.Player.UpDownMovement.ReadValue<Vector2>().y);
            //Debug.Log(playerInputActions.Player.Movement.ReadValue<Vector2>().x);
            CheckInput();
        }


        private void CheckInput()
        {
            playerInputActions.Player.LateralMovement.performed += ctx =>
            {
                if (lerpingToLane) return;
                
                ChangeLanes(playerInputActions.Player.LateralMovement.ReadValue<Vector2>().x);
            };

            playerInputActions.Player.UpDownMovement.performed += ctx =>
            {
                if (playerInputActions.Player.UpDownMovement.ReadValue<Vector2>().y < 0)
                {
                    if (sliding) return;

                    if (jumping && !wantToSlideAfterDescent)
                    {
                        playerAnimator.SetBool("WantToSlide", true);
                        wantToSlideAfterDescent = true;
                        JumpDownPlayerFaster();
                        return;
                    }

                    if (jumping) return;
                    
                    SlidePlayer();
                }
                else
                {
                    if (jumping) return;
                    
                    if(sliding)
                        StopSliding();
                    
                    JumpUpPlayer();
                }
            };
        }

        private void ChangeLanes(float value)
        {
            if (value > 0)
            {
                switch (currentLane)
                {
                    case 0:
                        currentLane++;
                        AnimateLaneChange(false);
                        LerpToLane(centerXPos);
                        break;
                    case 1:
                        currentLane++;
                        AnimateLaneChange(false);
                        LerpToLane(rightXPos);
                        break;
                }
            }
            else
            {
                switch (currentLane)
                {
                    case 2:
                        currentLane--;
                        AnimateLaneChange(true);
                        LerpToLane(centerXPos);
                        break;
                    case 1:
                        currentLane--;
                        AnimateLaneChange(true);
                        LerpToLane(leftXPos);
                        break;
                }
            }
        }

        private void AnimateLaneChange(bool laningLeft)
        {
            if (laningLeft)
                laneLeftSequence.Restart();
            else
                laneRightSequence.Restart();
        }
        
        private void LerpToLane(float xValue)
        {
            if(sliding)
                StopSliding();
            
            lerpingToLane = true;
            Sequence moveSequence = DOTween.Sequence();
            moveSequence.Append(playerTransform.DOMoveX(xValue, timeToMove).SetEase(Ease.OutSine));
            
            moveSequence.OnUpdate(() =>
            {
                if (moveSequence.ElapsedPercentage() >= 0.7f)
                {
                    lerpingToLane = false;
                }
            });
        }

        private void SlidePlayer()
        {
            sliding = true;
            wantToSlideAfterDescent = false;
            playerAnimator.SetBool("WantToSlide", false);
            jumpDownSequence.Pause();
            playerAnimator.SetBool("IsSliding", sliding);
            
            ChangePlayerColliderPos(false);
            
            slideSequence = DOTween.Sequence().Append((DOTween.To(x => valueForSlideReference = x, 0, 1, slideDuration)));
            
            slideSequence.OnComplete(StopSliding);
        }

        private void StopSliding()
        {
            slideSequence.Pause();
            ChangePlayerColliderPos(true);
            sliding = false;
            playerAnimator.SetBool("IsSliding", sliding);
        }

        private void ChangePlayerColliderPos(bool defaultPos)
        {
            if(defaultPos)
                playerCollider.center = new Vector3(playerCollider.center.x, defaultColliderYPos, playerCollider.center.z);
            else
                playerCollider.center = new Vector3(playerCollider.center.x, slidingColliderYPos, playerCollider.center.z);
        }

        private void JumpUpPlayer()
        {
            jumping = true;
            jumpDownSequence.Pause();
            jumpUpSequence.Restart();
            
            playerAnimator.SetBool("IsJumping", true);
            jumpUpSequence.OnComplete(JumpDownPlayer);
        }

        private void JumpDownPlayer()
        {
            playerAnimator.SetBool("IsJumping", false);
            jumpUpSequence.Pause();
            jumpDownSequence.Restart();
            
            playerAnimator.SetBool("IsFalling", true);
            
            jumpDownSequence.OnUpdate(() =>
            {
                if (jumpDownSequence.ElapsedPercentage() >= 0.75f)
                {
                    jumping = false;
                    playerAnimator.SetBool("IsFalling", false);
                    
                    if (wantToSlideAfterDescent)
                    {
                        SlidePlayer();
                    }
                }
            });
        }
        
        private void JumpDownPlayerFaster()
        {
            playerAnimator.SetBool("IsJumping", false);
            jumpUpSequence.Pause();
            
            Sequence jumpDownFasterSequence = DOTween.Sequence().Append(playerTransform.DOMoveY(playerDefaultYPos,
                0.2f).SetEase(Ease.InOutSine));
            
            playerAnimator.SetBool("IsFalling", true);
            
            jumpDownFasterSequence.OnUpdate(() =>
            {
                if (jumpDownFasterSequence.ElapsedPercentage() >= 0.8f)
                {
                    jumping = false;
                    playerAnimator.SetBool("IsFalling", false);
                    
                    if (wantToSlideAfterDescent)
                    {
                        SlidePlayer();
                    }
                }
            });
        }
    }
}
