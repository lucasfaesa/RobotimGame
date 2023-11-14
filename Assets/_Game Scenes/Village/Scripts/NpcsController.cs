using System.Collections;
using System.Collections.Generic;
using _Game_Scenes.Village.Scripts;
using UnityEngine;

public class NpcsController : MonoBehaviour
{
    [SerializeField] private List<VillageNpcInfoHolder> npcsPool;
    /*[Header("Npcs Settings")] 
    [SerializeField] private int questGiversQuantity = 1;*/
    [Header("Npcs Customizations")] 
    [SerializeField] private List<GameObject> npcsHatsPool;
    [Space]
    [SerializeField] private List<Color32> comboColorsGolden;
    [SerializeField] private List<Color32> comboColorsBlack;
    [SerializeField] private List<Color32> comboColorsPurple;
    [SerializeField] private List<Color32> comboColorsOther;
    [SerializeField] private List<Color32> comboColorsAnother;
    [SerializeField] private List<Color32> comboColorsSomething;
    [SerializeField] private List<Color32> comboColorsAnotherOne;
    [Space]
    [SerializeField] private List<NpcLocation> npcsLocations;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        ResetNpcs();
        SetNpcs();
    }
    
    private void SetNpcs()
    {
        foreach (var npc in npcsPool)
        {
            int randomPosNumber = Random.Range(0, npcsLocations.Count);
            int randomHatNumber = Random.Range(0, npcsHatsPool.Count);

            var npcTransform = npc.transform;
            npcTransform.parent = npcsLocations[randomPosNumber].transform;
            npc.NpcData.SetNpcStreetInfo(npcsLocations[randomPosNumber].StreetInfo);
            npc.SetNpcHat(npcsHatsPool[randomHatNumber]);
            npcTransform.localPosition = Vector3.zero;
            npcTransform.localRotation = Quaternion.identity;
            npc.NpcData.SetNpcLocation(npc.transform.position);
            
            npc.gameObject.SetActive(true);
            
            npcsLocations.RemoveAt(randomPosNumber);
            npcsHatsPool.RemoveAt(randomHatNumber);

            int randomColorNumber = Random.Range(0, 7);
            switch (randomColorNumber)
            {
                case 0:
                    npc.ChangeColors(comboColorsGolden[0],comboColorsGolden[1]);
                    break;
                case 1:
                    npc.ChangeColors(comboColorsBlack[0],comboColorsBlack[1]);
                    break;
                case 2:
                    npc.ChangeColors(comboColorsPurple[0],comboColorsPurple[1]);
                    break;
                case 3:
                    npc.ChangeColors(comboColorsOther[0],comboColorsOther[1]);
                    break;
                case 4:
                    npc.ChangeColors(comboColorsAnother[0],comboColorsAnother[1]);
                    break;
                case 5:
                    npc.ChangeColors(comboColorsSomething[0],comboColorsSomething[1]);
                    break;
                case 6:
                    npc.ChangeColors(comboColorsAnotherOne[0],comboColorsAnotherOne[1]);
                    break;
                default:
                    Debug.Log("Entered Default");
                    npc.ChangeColors(comboColorsGolden[0],comboColorsGolden[1]);
                    break;
            }
        }
    }
    
    private void ResetNpcs()
    {
        foreach (var npc in npcsPool)
        {
            npc.NpcData.SetNpcType(VillageNpcDataSO.NpcType.Default, QuestManagerSO.QuesType.None);
        }
    }
}
