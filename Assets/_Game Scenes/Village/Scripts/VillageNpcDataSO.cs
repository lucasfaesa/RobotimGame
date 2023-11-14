using System;
using System.Collections;
using System.Collections.Generic;
using _Game_Scenes.Village.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "VillageNpcData", menuName = "ScriptableObjects/VillageScene/VillageNpcData", order = 7)]
public class VillageNpcDataSO : ScriptableObject
{
    [SerializeField] private QuestManagerSO questManager;
    [Space]
    [SerializeField] private int npcId;
    [SerializeField] private NpcDialogDataSO npcDialogData;
    [SerializeField] private NpcType villageNpcType = NpcType.Default;
    [SerializeField] private QuestManagerSO.QuesType QuestType = QuestManagerSO.QuesType.None; //TODO make read only later
    [SerializeField] [ReadOnly] private StreetInfoSO npcStreetInfo;
    [SerializeField] [ReadOnly] private Vector3 npcLocation;

    public bool CanDialog { get; set; } = true;
    public event Action<NpcType, QuestManagerSO.QuesType> npcTypeStatusChanged;
    
    public enum NpcType {QuestGiver, QuestInteractor, Default }
    public int GetNpcId => npcId;
    public NpcType GetNpcType => villageNpcType;

    public void SetNpcType(NpcType npcType, QuestManagerSO.QuesType qType)
    {
        villageNpcType = npcType;
        QuestType = qType;
        npcTypeStatusChanged?.Invoke(npcType, qType);

        switch (npcType)
        {
            case NpcType.QuestGiver:
                questManager.ChangeCurrentQuestGiverNpc(this);
                break;
            case NpcType.QuestInteractor:
                questManager.ChangeCurrentQuestInteractorNpc(this);
                break;
        }
    }

    public QuestManagerSO.QuesType GetQuestType => QuestType;
    public NpcDialogDataSO NpcDialogData => npcDialogData;
    public StreetInfoSO GetNpcStreetInfo => npcStreetInfo;
    public Vector3 GetNpcLocation => npcLocation;
    public void SetNpcLocation(Vector3 loc) => npcLocation = loc;
    public void SetNpcStreetInfo(StreetInfoSO street) => npcStreetInfo = street;

    private void ResetSO()
    {
        CanDialog = true;
        villageNpcType = NpcType.Default;
        QuestType = QuestManagerSO.QuesType.None;
        npcStreetInfo = null;
        npcLocation = Vector3.zero;
    }
    private void OnDisable()
    {
        ResetSO();
    }
}
