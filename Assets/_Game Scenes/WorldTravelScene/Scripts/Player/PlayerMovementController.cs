using System;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using _WorldTravelScene.Scripts.Countries;
using _WorldTravelScene.Scripts.Questions;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace _WorldTravelScene.Scripts.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private CountriesManagerSO countriesManager;
        [SerializeField] private ActionsTogglerSO actionsToggler;
        [Header("Camera")]
        [SerializeField] private Transform cameraRef;
        [SerializeField] private Camera camera;
        [Header("Player")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform bodyRef;
        [SerializeField] private Transform bodyModel;
        [SerializeField] private Transform thrustersTransform;
        [SerializeField] private Transform world;
        [Header("General Settings")]
        [SerializeField] private float speed;
        [SerializeField] private float boostMultiplier;
        [SerializeField] private float worldRadius;
        [SerializeField] private float flyHeight;
        [SerializeField] private float turnSpeed;
        [SerializeField] private float cameraHeight;
        [SerializeField] private float smoothRollTime;
        [Header("Boost Settings")]
        [SerializeField] private float shakeStrength;
        [SerializeField] private int vibrato;
        [SerializeField] private Image boostCanvasRef;
        [SerializeField] private CanvasGroup boostCanvasGroup;
        [SerializeField] private ParticleSystem boostGainParticle;
        [SerializeField] private float currentBoostQuantity = 5;
        [SerializeField] private float maxBoostQuantity = 5;

        private float smoothedTurnSpeed;
        private float turnSmoothV;
        private float rollSmoothV;
        private float angle;

        private bool turningRight;
        private bool turningLeft;
        private bool goingStraight;

        private float boostDelay = 1f;
        private float lastTimeBoost;
        private bool boosting;

        private PlayerInputActions playerInputActions;

        private Sequence slowThrusterSequence;
        private Sequence fastThrusterSequence;
        private Sequence modelShakeSequence;
        private Sequence showBoostBar;
        private Sequence hideBoostBar;
        private Sequence showAndHideBoost;

        private float cont = 1;

        private bool gamePreparingToStart;
        private bool gameStarted;
        private float defaultSpeed;
        
        private void Awake()
        {
            playerInputActions = new PlayerInputActions();
        }


        void OnEnable()
        {
            playerInputActions.Enable();
            countriesManager.targetHitCorrectly += BoostGain;
            actionsToggler.ToggleMovement += ToggleInputActions;
            gameManager.preparingToStartGame += GameGoingToStart;
        }

        private void OnDisable()
        {
            playerInputActions.Disable();
            countriesManager.targetHitCorrectly -= BoostGain;
            actionsToggler.ToggleMovement -= ToggleInputActions;
            gameManager.preparingToStartGame -= GameGoingToStart;
        }

        private void GameGoingToStart() => gamePreparingToStart = true;

        private void StartGame()
        {
            speed = defaultSpeed;
            gameManager.GameStarted();
            ToggleInputActions(true);
        }
        
        private void ToggleInputActions(bool toggle)
        {
            if(toggle)
                playerInputActions.Enable();
            else
                playerInputActions.Disable();
        }

        private void Start()
        {
            defaultSpeed = speed;
            speed = 3;
            
            CreateAnimations();
            ToggleInputActions(false);
            boostCanvasGroup.gameObject.SetActive(false);
            boostCanvasGroup.alpha = 0f;
        }

        private void CreateAnimations()
        {
            modelShakeSequence = DOTween.Sequence();
            modelShakeSequence.Append(bodyModel.DOShakePosition(99, shakeStrength, vibrato)).Pause();
            
            slowThrusterSequence = DOTween.Sequence();
            slowThrusterSequence.Append(thrustersTransform.DOScaleX(1.2f, 1f).SetEase(Ease.InOutBack, 11));
            slowThrusterSequence.SetLoops(-1, LoopType.Yoyo);
            
            fastThrusterSequence = DOTween.Sequence();
            fastThrusterSequence.Append(thrustersTransform.DOScaleX(3f, 0.2f).SetEase(Ease.InOutBack, 11));
            fastThrusterSequence.SetLoops(-1, LoopType.Yoyo).Pause();

            showBoostBar = DOTween.Sequence().Append(boostCanvasGroup.DOFade(1f, 0.4f).SetEase(Ease.InOutSine)).SetAutoKill(false).Pause();
            hideBoostBar = DOTween.Sequence().Append(boostCanvasGroup.DOFade(0f, 0.4f).SetEase(Ease.InOutSine)).SetAutoKill(false).Pause().PrependInterval(0.5f);
            
            showAndHideBoost = DOTween.Sequence().Append(boostCanvasGroup.DOFade(1f, 0.4f).SetEase(Ease.InOutSine)).AppendInterval(1f);
            showAndHideBoost.Append(boostCanvasGroup.DOFade(0f, 0.4f).SetEase(Ease.InOutSine));
            showAndHideBoost.SetAutoKill(false).Pause();
        }
        
        void Update()
        {
            if (!gamePreparingToStart) return;
            
            UpdatePlayer();
            UpdateCamera();
            TurnAnimation();
            BoostButtonCheck();
            BoostQuantityCheck();
            SlowSpeedButtonCheck();
            /*if (Keyboard.current[Key.C].wasPressedThisFrame) //#temp
            {
                currentBoostQuantity = maxBoostQuantity;
                UpdateBoostCanvasRef();
            }*/
        }

        private void UpdatePlayer()
        {
            //movement
            Vector3 newPos = player.position + player.forward * (speed * Time.deltaTime);
            Vector3 gravityUp = newPos.normalized;
            newPos = Vector3.zero + gravityUp * (worldRadius + flyHeight);
            player.position = newPos;
            
            //rotation
            float turnInput = playerInputActions.Player.Movement.ReadValue<Vector2>().x;
            smoothedTurnSpeed =
                Mathf.SmoothDamp(smoothedTurnSpeed, turnInput * turnSpeed, ref turnSmoothV, smoothRollTime);
            player.RotateAround(newPos,gravityUp,smoothedTurnSpeed * Time.deltaTime);
            player.rotation = Quaternion.FromToRotation(player.up, gravityUp) * player.rotation;
            
        }

        private void UpdateCamera()
        {
            var position = player.position;

            //cameraRef.transform.position = position + player.up * cameraHeight;

            if (cont < 3.31f)
            {
               cameraRef.transform.position =
                    Vector3.Slerp(cameraRef.transform.position, position + player.up * cameraHeight, Time.deltaTime/1.3f);
                cont += Time.deltaTime;

            }
            else
            {
                if (!gameStarted)
                {
                    gameStarted = true;
                    StartGame();
                }
                
                cameraRef.transform.position = position + player.up * cameraHeight;
            }

            cameraRef.transform.LookAt(position);
        }

        private void TurnAnimation()
        {
            if (playerInputActions.Player.Movement.ReadValue<Vector2>().x > 0 && !turningRight)
            {
                ResetBools();
                turningRight = true;
                LerpToRot(new Vector3(0,0,-35));
            }

            if (playerInputActions.Player.Movement.ReadValue<Vector2>().x < 0 && !turningLeft)
            {
                ResetBools();
                turningLeft = true;
                LerpToRot(new Vector3(0,0,35));
            }
            else if (playerInputActions.Player.Movement.ReadValue<Vector2>().x == 0 && !goingStraight)
            {
                ResetBools();
                goingStraight = true;
                LerpToRot(new Vector3(0,0,0));
            }
        }
        
        private void LerpToRot(Vector3 targetRot)
        {
            bodyRef.DOLocalRotate(targetRot, 0.4f);
        }

        private void ResetBools()
        {
            turningRight = false;
            turningLeft = false;
            goingStraight = false;
        }

        private void BoostButtonCheck()
        {
            if (playerInputActions.Player.Dash.WasPressedThisFrame() && !boosting)
            {
                if (Time.time - lastTimeBoost < boostDelay && lastTimeBoost != 0) { return; }
                
                StartBoosting();
            }
            if (playerInputActions.Player.Dash.WasReleasedThisFrame() && boosting)
            {
                StopBoosting();
            }
        }

        private void SlowSpeedButtonCheck()
        {
            if (boosting) return;
            
            if (playerInputActions.Player.Control.WasPressedThisFrame())
            {
                speed = defaultSpeed / 2;
            }
            if (playerInputActions.Player.Control.WasReleasedThisFrame())
            {
                speed = defaultSpeed;
            }
        }

        private void StartBoosting()
        {
            if (hideBoostBar.IsPlaying()) { hideBoostBar.Pause(); }
            if (showAndHideBoost.IsPlaying()){showAndHideBoost.Pause();}
            
            boostCanvasGroup.gameObject.SetActive(true);
            showBoostBar.Restart();
            
            boosting = true;
            speed = defaultSpeed;
            speed *= boostMultiplier;
            DOTween.To(x => camera.fieldOfView = x, camera.fieldOfView, 51, 0.2f).SetEase(Ease.Linear);
            bodyModel.DOLocalRotate(new Vector3(360, 0, 0), 0.8f, RotateMode.FastBeyond360).SetRelative(true)
                .SetEase(Ease.OutBack);
            modelShakeSequence.Play();
            slowThrusterSequence.Pause();
            thrustersTransform.transform.localScale = new Vector3(2f, 1f, 1f);
            fastThrusterSequence.Play();
                
            lastTimeBoost = Time.time;
        }
        private void StopBoosting()
        {
            hideBoostBar.Restart();
            hideBoostBar.OnComplete(() =>
            {
                boostCanvasGroup.gameObject.SetActive(false);
            });
            
            lastTimeBoost = Time.time;
            boosting = false;
            
            speed /= boostMultiplier;
            DOTween.To(x => camera.fieldOfView = x, camera.fieldOfView, 47.6f, 0.5f).SetEase(Ease.InOutSine);
            modelShakeSequence.Pause();
            modelShakeSequence.Rewind();
            thrustersTransform.transform.localScale = new Vector3(1f, 1f, 1f);
            slowThrusterSequence.Play();
            fastThrusterSequence.Pause();
        }

        private void BoostGain(CountrySO countrySo, TargetDivision.TargetDivisionType targetDivisionType, int arg3)
        {
            /*switch (targetDivisionType)
            {
                case TargetDivision.TargetDivisionType.Inner:
                    currentBoostQuantity += 3.7f;
                    break;
                case TargetDivision.TargetDivisionType.Middle:
                    currentBoostQuantity += 2f;
                    break;
                case TargetDivision.TargetDivisionType.Outer:
                    currentBoostQuantity += 1f;
                    break;
            }

            if (currentBoostQuantity > maxBoostQuantity)*/
            currentBoostQuantity = maxBoostQuantity;
            
            UpdateBoostCanvasRef();

            if (!boostCanvasGroup.gameObject.activeInHierarchy || hideBoostBar.IsPlaying())
            {
                boostCanvasGroup.alpha = 0f;
                boostCanvasGroup.gameObject.SetActive(true);
                hideBoostBar.Pause();
                showAndHideBoost.Restart();
                showAndHideBoost.OnComplete(() =>
                {
                    boostCanvasGroup.gameObject.SetActive(false);
                });
            }
            
            boostGainParticle.Play();
        }
        
        private void BoostQuantityCheck()
        {
            if (boosting)
            {
                currentBoostQuantity -= Time.deltaTime;
                
                UpdateBoostCanvasRef();
                
                if (currentBoostQuantity <= 0)
                {
                    currentBoostQuantity = 0;
                    StopBoosting();
                }
            }
        }

        private void UpdateBoostCanvasRef()
        {
            boostCanvasRef.fillAmount = currentBoostQuantity / maxBoostQuantity;
        }

    }
}
