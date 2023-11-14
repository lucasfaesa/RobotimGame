using System.Collections;
using System.Collections.Generic;
using _Game_Scenes.Village.Scripts;
using _MeteorShower.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace _Village.Scripts
{
    public class CanvasController : MonoBehaviour
    {
        [Header("SO")]
        [SerializeField] private QuestManagerSO questManager;
        [SerializeField] private GameManagerSO gameManager;
        [Space]
        [SerializeField] private PlayerTriggerInteractor playerTriggerInteractor;
        [Space]
        [Header("NPC Interaction")]
        [SerializeField] private GameObject interactTextObject;
        [Header("NPC Dialog")]
        [SerializeField] private DialogController dialogController;
        [Header("Quest Log")] 
        [SerializeField] private GameObject questDescriptionObject;
        [SerializeField] private TextMeshProUGUI completedQuestsText;
        [SerializeField] private TextMeshProUGUI totalQuestsText;
        [SerializeField] private TextMeshProUGUI questDescriptionText;
        [Header("Quest Types SO")]
        [SerializeField] private FindObjectQuestSO findObjectQuest;
        [Header("Tutorial Screen")] 
        [SerializeField] private GameObject tutorialScreen;
        [Header("Game Finished Screen")] 
        [SerializeField] private GameFinishedScreenController gameFinishedScreen;
    
        private bool inDialog;
        private bool inQuest;
        private bool inOverlayQuest;
    
        private bool canInteractWithNpc;
        private List<VillageNpcDataSO> collidingNpcsDatas = new List<VillageNpcDataSO>();
        private Coroutine leaveDialogStatusDelay;
    
        private void OnEnable()
        {
            tutorialScreen.SetActive(true);
            questDescriptionObject.SetActive(false);
            UpdateTotalQuests(questManager.TotalQuestsQuantity);
            UpdateCompletedQuests();
            gameManager.gameEnded += GameFinished;
            questManager.QuestStarted += ShowCurrentQuestDescription;
            questManager.QuestEnded += UpdateCompletedQuests;
            questManager.QuestEnded += HideCurrentQuestDescription;
            questManager.InOverlay += OverlayQuest;
            playerTriggerInteractor.collidedWithNpc += AddNpcToCollisionList;
            playerTriggerInteractor.uncollidedWithNpc += RemoveNpcFromCollisionList;
        }

        private void OnDisable()
        {
            gameManager.gameEnded -= GameFinished;
            questManager.QuestStarted -= ShowCurrentQuestDescription;
            questManager.QuestEnded -= UpdateCompletedQuests;
            questManager.QuestEnded -= HideCurrentQuestDescription;
            questManager.InOverlay -= OverlayQuest;
            playerTriggerInteractor.collidedWithNpc -= AddNpcToCollisionList;
            playerTriggerInteractor.uncollidedWithNpc -= RemoveNpcFromCollisionList;
        }

        private void LockCursor(bool status)
        {
            if (status)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void HideTutorial()
        {
            tutorialScreen.SetActive(false);
            LockCursor(true);
        }

        private void GameFinished()
        {
            StartCoroutine(ShowGameFinishedScreen());
        }

        private IEnumerator ShowGameFinishedScreen()
        {
            yield return new WaitForSeconds(0.4f);
            gameFinishedScreen.ShowGameFinishedScreen();
            MovementToggle.CameraOrbitActive(false);
            MovementToggle.PlayerMovementActive(false);
            LockCursor(false);
        }
    
        #region Quests
        private void UpdateTotalQuests(int total)
        {
            totalQuestsText.text = total.ToString();
        }

        private void UpdateCompletedQuests()
        {
            inQuest = false;
            completedQuestsText.text = questManager.CompletedQuests.ToString();
        }

        private void ShowCurrentQuestDescription(QuestManagerSO.QuesType qType)
        {
            inQuest = true;
        
            questDescriptionObject.SetActive(true);

            switch (qType)
            {
                case QuestManagerSO.QuesType.FindObject:
                    WriteFindObjectTextDescription();
                    break;
            }
        }

        #region Find Object Quest
        private void WriteFindObjectTextDescription()
        {
            questDescriptionText.text = "Encontrar o Poliedro que possui ";
        
            switch (findObjectQuest.CurrentCorrectParameter)
            {
                case FindObjectQuestSO.ObjectParameterToAnswerCorrectly.Faces:
                    questDescriptionText.text += findObjectQuest.GetCorrectObject.NumberOfFaces + " Faces";
                    break;
                case FindObjectQuestSO.ObjectParameterToAnswerCorrectly.Edges:
                    questDescriptionText.text += findObjectQuest.GetCorrectObject.NumberOfEdges + " Arestas";
                    break;
                case FindObjectQuestSO.ObjectParameterToAnswerCorrectly.Vertices:
                    questDescriptionText.text += findObjectQuest.GetCorrectObject.NumberOfVertices + " Vértices";
                    break;
            }

            questDescriptionText.text +=$"\n{findObjectQuest.TargetNpc.NpcDialogData.GetNpcName} está com o objeto na rua ";

            var parallelStreets = findObjectQuest.TargetNpc.GetNpcStreetInfo.GetParallelStreets();
            var concurrentStreets = findObjectQuest.TargetNpc.GetNpcStreetInfo.GetConcurrentStreets();
        
            if (parallelStreets.Count > 0 && concurrentStreets.Count > 0)
            {
                int randomNumber = Random.Range(0, 2);
        
                switch (randomNumber)
                {
                    case 0:
                        questDescriptionText.text +=
                            "paralela a rua " + parallelStreets[Random.Range(0, parallelStreets.Count)].StreetName;
                        break;
                    case 1:
                        questDescriptionText.text +=
                            "concorrente a rua " + concurrentStreets[Random.Range(0, concurrentStreets.Count)].StreetName;
                        break;
                }
                questDescriptionText.text += "\n<color=yellow>Para se guiar, aperte a tecla \"M\" e consulte o mapa.</color>";
                return;
            }

            if (parallelStreets.Count > 0)
            {
                questDescriptionText.text += "paralela a rua " + parallelStreets[Random.Range(0, parallelStreets.Count)].StreetName;
            }
            else
            {
                questDescriptionText.text += "concorrente a rua " + concurrentStreets[Random.Range(0, concurrentStreets.Count)].StreetName;
            }
            
            questDescriptionText.text += "\n<color=yellow>Para se guiar, aperte a tecla \"M\" e consulte o mapa.</color>";
            
        }

        private void OverlayQuest(bool status)
        {
            inOverlayQuest = status;
        }

        #endregion
    
        private void HideCurrentQuestDescription()
        {
            questDescriptionObject.SetActive(false);
        }
        #endregion
    
        #region Dialog
        public void PlayerInDialog(bool status)
        {
            if (!status)
            {
                MovementToggle.PlayerMovementActive(true);
                if (leaveDialogStatusDelay != null)
                    StopCoroutine(leaveDialogStatusDelay);
                leaveDialogStatusDelay = StartCoroutine(LeaveDialogStatusDelay());
            }
            else
            {
                inDialog = true;
                MovementToggle.PlayerMovementActive(false);
                interactTextObject.SetActive(false);
            }
        }
    
        private IEnumerator LeaveDialogStatusDelay() //Isto evita o reentrar imediato na conversa quando se spamma o botão
        {
            yield return new WaitForSeconds(0.65f);
            inDialog = false;
        }
    
        private void ShowInteractText()
        {
            interactTextObject.SetActive(true);
            canInteractWithNpc = true;
        }

        private void HideInteractText()
        {
            interactTextObject.SetActive(false);
            canInteractWithNpc = false;
        }

        public void ShowNpcDialog(VillageNpcDataSO data)
        {
            dialogController.ShowDialog(data);
        }
    
        #endregion
    
        private void Update()
        {
            if (canInteractWithNpc)
            {
                if (Keyboard.current.eKey.wasPressedThisFrame && !inDialog && collidingNpcsDatas[0].CanDialog && !inOverlayQuest)
                {
                    ShowNpcDialog(collidingNpcsDatas[0]);
                }
            }
        }

        private void AddNpcToCollisionList(VillageNpcDataSO npcData)
        {
            collidingNpcsDatas.Add(npcData);
            ShowInteractText();
        }
        private void RemoveNpcFromCollisionList(VillageNpcDataSO npcData)
        {
            collidingNpcsDatas.Remove(npcData);
            if(collidingNpcsDatas.Count <= 0)
                HideInteractText();
        }
    
 
    
    }
}
