using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _Lobby._LevelSelector.Scripts;
using _MeteorShower.Scripts;
using _Village.Scripts;
using _WorldTravelScene.Scripts.Objective;
using _WorldTravelScene.Scripts.Questions;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace _WorldTravelScene.Scripts.Crate
{
    public class CratesController : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManager;
        [SerializeField] private LevelCompletionSO levelCompletion;
        [SerializeField] private ObjectivesManagerSO objectivesManager;
        [SerializeField] private ActionsTogglerSO actionsToggler;
        [Space] 
        [SerializeField] private GameObject crateInfosObject;
        [SerializeField] private TextMeshProUGUI crateAmountTxt;
        [Space] 
        [SerializeField] private GameObject aimTransform;
        [Space]
        [SerializeField] private CrateBehavior cratePrefab;
        [SerializeField] private List<CrateBehavior> pooledCrates;
        [Space]
        [SerializeField] private Transform crateDropRef;
        [SerializeField] private Transform worldTransform;
        
        private PlayerInputActions playerInputActions;

        
        private bool canDeployCrate;
        private int crateQuantity;
        private float crateDelay = 0.6f;
        private float lastTimeCrate;

        private Coroutine checkDelayed;
        
        private void Awake() { playerInputActions = new PlayerInputActions(); }

        private void Start() { crateInfosObject.SetActive(false); }

        void OnEnable()
        {
            objectivesManager.newObjectiveGot += SetCrateQuantity;
            objectivesManager.objectiveStarted += ShowCrateQuantityCanvas;
            objectivesManager.objectiveCompleted += ObjectiveCompleted;
            playerInputActions.Enable();
            actionsToggler.ToggleMovement += ToggleInputActions;
        }
        
        private void OnDisable()
        {
            objectivesManager.newObjectiveGot -= SetCrateQuantity;
            objectivesManager.objectiveStarted -= ShowCrateQuantityCanvas;
            objectivesManager.objectiveCompleted -= ObjectiveCompleted;
            playerInputActions.Disable();
            actionsToggler.ToggleMovement -= ToggleInputActions;
        }
        
        private void ToggleInputActions(bool toggle)
        {
            if(toggle)
                playerInputActions.Enable();
            else
                playerInputActions.Disable();
        }
        
        void Update()
        {
            if (playerInputActions.Player.Jump.WasPressedThisFrame() && canDeployCrate)
            {
                if (Time.time - lastTimeCrate < crateDelay && lastTimeCrate != 0) return;
                
                DropCrate();
            }
        }

        private void SetCrateQuantity(ObjectiveInfo objectiveInfo, ObjectivesController.LevelDifficulty difficulty)
        {
            crateQuantity = objectiveInfo.CountriesQuantityToCompleteMission + (int)difficulty;
        }

        private void ShowCrateQuantityCanvas(ObjectiveInfo n)
        {
            aimTransform.SetActive(true);
            canDeployCrate = true;
            crateInfosObject.SetActive(true);
            crateAmountTxt.text = "x" + crateQuantity;
        }

        private void DropCrate()
        {
            if (--crateQuantity == 0) { canDeployCrate = false; }
            crateAmountTxt.text = "x" + crateQuantity;

            lastTimeCrate = Time.time;
            foreach (var crate in pooledCrates)
            {
                if (crate.ReadyToBeDeployed)
                {
                    crate.gameObject.SetActive(true);
                    
                    crate.SetCrate(crateDropRef.up, worldTransform, crateDropRef,this);
                    var crateTransform = crate.transform;
                    crateTransform.parent = null;
                    crateTransform.rotation = quaternion.identity;
                    return;
                }
            }
            
            InstantiateCrate(); //if foreach loop doesnt find a ready to be deployed crate
        }

        private void InstantiateCrate()
        {
            CrateBehavior crate = Instantiate(cratePrefab, crateDropRef.transform);
            crate.SetCrate(crateDropRef.transform.up, worldTransform, crateDropRef,this);
            var crateTransform = crate.transform;
            crateTransform.parent = null;
            crateTransform.rotation = quaternion.identity;
            crate.gameObject.SetActive(true);
            
            pooledCrates.Add(crate);
        }

        public void CrateReturned()
        {
            if (checkDelayed != null)
            {
                StopCoroutine(CheckDelayed());
                checkDelayed = null;
            }
            checkDelayed = StartCoroutine(CheckDelayed());
        }

        private IEnumerator CheckDelayed()
        {
            yield return new WaitForEndOfFrame();
            
            if (objectivesManager.CurrentObjective != null && !pooledCrates.Any(x=>x.gameObject.activeInHierarchy) && crateQuantity == 0) //se não tiver mais nenhum ativo (ou seja, todos estão disponiveis para lançamento) e a quantity para lançar for zero, é game over
            {
                levelCompletion.LevelCompleted = false;
                gameManager.GameLost();
                gameManager.GameEnded();
                Debug.Log("Game OVer");
            }

            checkDelayed = null;

        }

        private void ObjectiveCompleted(ObjectiveInfo objectiveInfo)
        {
            aimTransform.SetActive(false);
            canDeployCrate = false;
            crateInfosObject.SetActive(false);
        }
        
    }
}
