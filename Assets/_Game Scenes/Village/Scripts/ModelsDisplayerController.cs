using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game_Scenes.Village.Scripts;
using UnityEngine;

public class ModelsDisplayerController : MonoBehaviour
{
    [SerializeField] private FindObjectQuestSO findObjectQuest;
    [SerializeField] private QuestManagerSO questManager;
    [Space] 
    [SerializeField] private GameObject findObjectGameObject;
    [SerializeField] private List<ThreeDObjectInfoHolder> threeDmodels;
    [SerializeField] private List<Transform> targetPositions;

    private void OnEnable()
    {
        findObjectQuest.correctObjectChoice += DeactivateObjects;
        questManager.startedFindObjectQuestObjectSelection += OrganizeObjectSelectionForFindObjectQuest;
    }

    private void OnDisable()
    {
        findObjectQuest.correctObjectChoice -= DeactivateObjects;
        questManager.startedFindObjectQuestObjectSelection -= OrganizeObjectSelectionForFindObjectQuest;
    }

    public void OrganizeObjectSelectionForFindObjectQuest()
    {
        foreach (var models in threeDmodels)
        {
            models.gameObject.SetActive(false);
        }
        
        List<ThreeDObjectInfoHolder> optionsObjectsModels = new();

        foreach (var modelData in findObjectQuest.RandomObjectsOptionsList)
        {
            foreach (var model in threeDmodels)
            {
                if (model.Get3DObjectInfo() == modelData)
                {
                    optionsObjectsModels.Add(model);
                    break;
                }
            }    
        }

        for (int i = 0; i < targetPositions.Count; i++)
        {
            optionsObjectsModels[i].transform.parent = targetPositions[i];
            optionsObjectsModels[i].transform.localPosition = Vector3.zero;
            optionsObjectsModels[i].gameObject.SetActive(true);
        }
        
        findObjectGameObject.SetActive(true);
        
    }

    private void DeactivateObjects(int n)
    {
        findObjectGameObject.SetActive(false);
    }

}
