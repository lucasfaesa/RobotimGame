using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class NpcsController : MonoBehaviour
    {
        [SerializeField] private ActionsTogglerSO actionsToggler;
        [SerializeField] private PlayerResearchDataSO playerResearchData;
        [SerializeField] private QuestsManagerSO questsManager;
        [Space]
        [SerializeField] private NpcDisplay[] npcPoses;
        [SerializeField] private NpcDisplay npcVictoryPose;
        [SerializeField] private NpcDisplay npcDefeatPose;
        [Space] 
        [SerializeField] private Transform trailTransform;
        
        int previousNumber = -1;
        private float delayTime = 1f;
        private int cont = 0;

        private void OnEnable()
        {
            playerResearchData.deliveredResearch += RemovePlayerControl;
            questsManager.deliveredResearchSuccess += AnimateResearchSuccess;
            questsManager.deliveredResearchError += AnimateResearchError;
        }

        private void OnDisable()
        {
            playerResearchData.deliveredResearch -= RemovePlayerControl;
            questsManager.deliveredResearchSuccess -= AnimateResearchSuccess;
            questsManager.deliveredResearchError -= AnimateResearchError;
        }

        private void ShowNpcIdle()
        {
            DeactivateAllNpcs();
            npcPoses[0].ToggleNpc(true);
        }
        
        private void RemovePlayerControl(ResearchTypeSO _)
        {
            actionsToggler.MovementToggle(false);
        }

        private void AnimateResearchSuccess(ResearchTypeSO _)
        {
            AnimateNpcResearching(true);
        }

        private void AnimateResearchError(ResearchTypeSO _)
        {
            AnimateNpcResearching(false);   
        }
        
        private void AnimateNpcResearching(bool correctAnswer)
        {
            if (cont == 9)
            {
                EndAnimation(correctAnswer);
                return;
            }
            
            int randomRange = Random.Range(0, npcPoses.Length);

            while (randomRange == previousNumber) { randomRange = Random.Range(0, npcPoses.Length); }

            previousNumber = randomRange;
            
            DeactivateAllNpcs();
            
            trailTransform.DOMove(npcPoses[randomRange].transform.position, 0.1f).SetEase(Ease.Linear);
            
            npcPoses[randomRange].ToggleNpc(true);

            delayTime -= 0.1f;

            float z = 0;
            Sequence waitForSeconds = DOTween.Sequence().Append(DOTween.To(x => z = x, 0, 1, delayTime));
            waitForSeconds.OnComplete(() =>
            {
                cont++;
                AnimateNpcResearching(correctAnswer);
            });
        }

        private void EndAnimation(bool correctAnswer)
        {
            DeactivateAllNpcs();
            cont = 0;
            delayTime = 1f;
            
            if(correctAnswer) 
                npcVictoryPose.ToggleNpc(true);
            else 
                npcDefeatPose.ToggleNpc(true);
            
            float z = 0;
            Sequence waitForSeconds = DOTween.Sequence().Append(DOTween.To(x => z = x, 0, 1, 2f));
            waitForSeconds.OnComplete(()=>
            {
                ShowNpcIdle();
                questsManager.FinishedNpcDeliverAnimation(); 
                actionsToggler.MovementToggle(true);
            });
            
            
        }

        private void DeactivateAllNpcs()
        {
            foreach (var npc in npcPoses) { npc.ToggleNpc(false); }
            npcVictoryPose.ToggleNpc(false);
            npcDefeatPose.ToggleNpc(false);
        }
        
        [ContextMenu("Animate NPC Researching Victory")]
        public void ContextClickFunction()
        {
            AnimateNpcResearching(true);
        }
        [ContextMenu("Animate NPC Researching Defeat")]
        public void Context1ClickFunction()
        {
            AnimateNpcResearching(false);
        }
            
    }
}
