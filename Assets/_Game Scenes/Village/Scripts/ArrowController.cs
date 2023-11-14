using System;
using _Game_Scenes.Village.Scripts;
using DG.Tweening;
using UnityEngine;

namespace _Village.Scripts
{
    public class ArrowController : MonoBehaviour
    {
        [SerializeField] private QuestManagerSO questManager;
        [SerializeField] private GameObject modelObject;

        private bool canLookAt;
        private Vector3 lookPoint;

        private void OnEnable()
        {
            questManager.questGiverNpcChanged += ActivateArrow;
            questManager.QuestStarted += DeactivateArrow;
        }

        private void OnDisable()
        {
            questManager.questGiverNpcChanged -= ActivateArrow;
            questManager.QuestStarted -= DeactivateArrow;
        }

        private void ActivateArrow(VillageNpcDataSO data)
        {
            lookPoint = new Vector3(data.GetNpcLocation.x,data.GetNpcLocation.y + 2.2f, data.GetNpcLocation.z);
            canLookAt = true;
            modelObject.SetActive(true);
            
            modelObject.transform.DOScale(1.1f, 0.4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    
        private void DeactivateArrow(QuestManagerSO.QuesType n)
        {
            canLookAt = false;
            modelObject.SetActive(false);
        }

        private void Update()
        {
            if(canLookAt)
                modelObject.transform.LookAt(lookPoint);
        }
    }
}
