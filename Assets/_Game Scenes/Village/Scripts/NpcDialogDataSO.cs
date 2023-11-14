using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcDialogData", menuName = "ScriptableObjects/VillageScene/NpcDialogData", order = 8)]
public class NpcDialogDataSO : ScriptableObject
{
    [SerializeField] [ReadOnly] private string npcName;
    [SerializeField] [ReadOnly] private string defaultText;
    [SerializeField] [ReadOnly] private List<string> questGiverText;
    [SerializeField] [ReadOnly] private List<string> questInteractorText;

    public string GetNpcName => npcName;
    public string GetDefaultText => defaultText;
    public List<string> GetQuestGiverText => questGiverText;
    public List<string> GetInteractorText => questInteractorText;
    

    public void Initialize()
    {
        SetNpcName();
        SetDefaultText();
    }

    private void ResetSO()
    {
        npcName = "";
        defaultText = "";
        questGiverText = new List<string>();
        questInteractorText = new List<string>();
    }
    private void OnDisable()
    {
        ResetSO();
    }

    private void SetNpcName()
    {
        npcName = NpcsNames.GetRandomName();
    }

    private void SetDefaultText()
    {
        defaultText = BlankDialogs.GetRandomBlankDialog();
    }
    
    public void SetQuestGiverText(List<string> texts)
    {
        questGiverText = texts;
    }
    
    public void SetQuestInteractorText(List<string> texts)
    {
        questInteractorText = texts;
    }
}
