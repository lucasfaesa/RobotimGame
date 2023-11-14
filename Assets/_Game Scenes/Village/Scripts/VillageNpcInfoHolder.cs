using System;
using System.Collections;
using System.Collections.Generic;
using _Game_Scenes.Village.Scripts;
using UnityEngine;

public class VillageNpcInfoHolder : MonoBehaviour
{
    [SerializeField] private VillageNpcDataSO villageNpcData;
    [SerializeField] private NpcDialogDataSO villageNpcDialogData;
    [SerializeField] private GameObject greenSparks;
    [Space]
    [SerializeField] private List<Renderer> mainColorFirst;
    [SerializeField] private List<Renderer> subColorFirst;
    [Space]
    [SerializeField] private Transform hatPosition;
    
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    public VillageNpcDataSO NpcData => villageNpcData;
    public NpcDialogDataSO NpcDialogData => villageNpcDialogData;
    
    public void SetQuestGiverText(List<string> texts)
    {
        villageNpcDialogData.SetQuestGiverText(texts);
    }
    
    public void SetQuestInteractorText(List<string> texts)
    {
        villageNpcDialogData.SetQuestInteractorText(texts);
        greenSparks.SetActive(true);
    }

    private void OnEnable()
    {
        villageNpcDialogData.Initialize();
        villageNpcData.npcTypeStatusChanged += SetDialog;
    }

    public void SetDialog(VillageNpcDataSO.NpcType npcType, QuestManagerSO.QuesType qType)
    {
        switch (npcType)
        {
            case VillageNpcDataSO.NpcType.QuestGiver:
                switch (qType)
                {
                    case QuestManagerSO.QuesType.FindObject:
                        SetQuestGiverText(FindObjectQuestTypeDialogs.GetRandomFindObjectQuestGiverDialog());
                        break;
                }
            break;
            
            case VillageNpcDataSO.NpcType.QuestInteractor:
                switch (qType)
                {
                    case QuestManagerSO.QuesType.FindObject:
                        SetQuestInteractorText(FindObjectQuestTypeDialogs.GetRandomFindObjectQuestInteractorDialog());
                        break;
                }
            break;
            
            default:
                greenSparks.SetActive(false);
                break;
        }
    }
    
    public void ChangeColors(Color32 mainColor, Color32 subColor)
    {
        foreach (var renderer1 in mainColorFirst)
        {
            renderer1.materials[0].SetColor(BaseColor,mainColor);
            if(renderer1.materials.Length > 1)
                renderer1.materials[1].SetColor(BaseColor,subColor);
        }
        
        foreach (var renderer1 in subColorFirst)
        {
            renderer1.materials[0].SetColor(BaseColor,subColor);
            if(renderer1.materials.Length > 1)
                renderer1.materials[1].SetColor(BaseColor,mainColor);
        }
    }

    public void SetNpcHat(GameObject hat)
    {
        hat.transform.parent = hatPosition;
        hat.transform.localPosition = Vector3.zero;
        hat.SetActive(true);
    }

}
