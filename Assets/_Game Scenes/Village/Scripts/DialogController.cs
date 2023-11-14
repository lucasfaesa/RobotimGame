using System;
using System.Collections;
using System.Collections.Generic;
using _Game_Scenes.Village.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogController : MonoBehaviour
{
    [SerializeField] private QuestManagerSO questManager;
    [Space]
    [SerializeField] private GameObject dialogWindow;
    [Space]    
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI npcDialogText;
    [Space]
    [SerializeField] private UnityEvent dialogStarted;
    [SerializeField] private UnityEvent dialogEnded;
    
    private string npcName;
    private List<string> dialogTexts = new();
    
    private bool inDialog;
    private bool inQuest;
    private bool inOverlay;
    
    private int currentDialogIndex;

    private VillageNpcDataSO npcData;

    private void OnEnable()
    {
        questManager.QuestStarted += StartedQuest;
        questManager.QuestEnded += EndedQuest;

    }

    private void OnDisable()
    {
        questManager.QuestStarted -= StartedQuest;
        questManager.QuestEnded -= EndedQuest;
    }

    public void StartedQuest(QuestManagerSO.QuesType qType)
    {
        inQuest = true;
    }

    public void EndedQuest()
    {
        inQuest = false;
    }
    
    public void ShowDialog(VillageNpcDataSO data)
    {
        npcData = data;
        
        switch (data.GetNpcType)
        {
            case VillageNpcDataSO.NpcType.Default:
                SetDialog(data.NpcDialogData.GetNpcName, data.NpcDialogData.GetDefaultText);
                break;
            case VillageNpcDataSO.NpcType.QuestGiver:
                SetDialog(data.NpcDialogData.GetNpcName, data.NpcDialogData.GetQuestGiverText);
                break;
            case VillageNpcDataSO.NpcType.QuestInteractor:
                SetDialog(data.NpcDialogData.GetNpcName, data.NpcDialogData.GetInteractorText);
                break;
        }

    }
    
    public void SetDialog(string name, List<string> dialogs)
    {
        dialogWindow.SetActive(true);
        dialogStarted?.Invoke();
        inDialog = true;
        
        npcName = name;
        dialogTexts = dialogs;

        currentDialogIndex = 0;
        WriteDialogText(name, dialogs[0]);
    }
    
    public void SetDialog(string name, string dialog)
    {
        dialogWindow.SetActive(true);
        dialogStarted?.Invoke();
        inDialog = true;
        
        npcName = name;

        WriteDialogText(name, dialog);
    }

    private void SkipToNextDialog()
    {
        if (currentDialogIndex < dialogTexts.Count - 1 && dialogTexts.Count > 0)
        {
            currentDialogIndex++;
            WriteDialogText(npcName, dialogTexts[currentDialogIndex]);    
        }
        else
        {
            DialogEnded();
        }
        
    }

    private void WriteDialogText(string name, string npcDialogs)
    {
        npcNameText.text = name;
        npcDialogText.text = npcDialogs;
    }

    private void DialogEnded()
    {
        dialogWindow.SetActive(false);
        inDialog = false;
        dialogEnded?.Invoke();

        switch (npcData.GetNpcType)
        {
            case VillageNpcDataSO.NpcType.QuestGiver:
                if (inQuest) return;
                questManager.SetCurrentQuest(npcData.GetQuestType);
                break;
            case VillageNpcDataSO.NpcType.QuestInteractor:
                switch (npcData.GetQuestType)
                {
                    case QuestManagerSO.QuesType.FindObject:
                        questManager.ActivateFindObjectQuestObjectSelection();
                        npcData.SetNpcType(VillageNpcDataSO.NpcType.Default, QuestManagerSO.QuesType.None); 
                        break;
                }
                break;
        }
    }

    private void Update()
    {
        if (inDialog)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                SkipToNextDialog();
            }
        }
    }
}
