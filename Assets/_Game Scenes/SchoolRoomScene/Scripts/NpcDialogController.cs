using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class NpcDialogController : MonoBehaviour
    {
        [SerializeField] private QuestsManagerSO questsManager;
        [Space]
        [SerializeField] private GameObject npcDialogWindowObject;
        [SerializeField] private CanvasGroup[] npcImages;
        [SerializeField] private CanvasGroup dialogWindow;
        [SerializeField] private TextMeshProUGUI dialogText;
        [SerializeField] private float delayBetweenWords = 0.01f;

        private bool correctAnswer;
        private CanvasGroup npcImageToShow;
        
        private void OnEnable()
        {
            questsManager.finishedNpcDeliverAnimation += AnimateDialogWindowDeliver;
            questsManager.deliveredResearchSuccess += DeliverSuccess;
            questsManager.deliveredResearchError += DeliverError;
            questsManager.newQuestGot += AnimateDialogWindowQuest;
        }

        private void OnDisable()
        {
            questsManager.finishedNpcDeliverAnimation -= AnimateDialogWindowDeliver;
            questsManager.deliveredResearchSuccess -= DeliverSuccess;
            questsManager.deliveredResearchError -= DeliverError;
            questsManager.newQuestGot -= AnimateDialogWindowQuest;
        }

        private void DeliverSuccess(ResearchTypeSO _)
        {
            npcDialogWindowObject.SetActive(false);
            
            correctAnswer = true;
        }

        private void DeliverError(ResearchTypeSO _)
        {
            npcDialogWindowObject.SetActive(false);
            
            correctAnswer = false;
        }
        
        private void AnimateDialogWindowQuest(SchoolQuest schoolQuest)
        {
            DeactivateAllNpcImages();
            
            npcImageToShow = npcImages[0];
            
            npcImageToShow.gameObject.SetActive(true);
            
            dialogWindow.alpha = 0f;
            
            dialogText.text = $"<size=80%>{NpcDialogs.GetRandomDialogIntroduction()}</size>\n<color=black>\"{schoolQuest.Description}\" </color>";
            dialogText.maxVisibleCharacters = 0;
            
            
            npcDialogWindowObject.SetActive(true);

            Sequence fadeNpcSequence = DOTween.Sequence().Append(npcImageToShow.DOFade(1f, 0.7f)).PrependInterval(0.3f);
            dialogWindow.DOFade(1f, 1f);

            fadeNpcSequence.OnComplete(() => { StartCoroutine(TypewriterEffect()); });
        }
        
        private void AnimateDialogWindowDeliver()
        {
            DeactivateAllNpcImages();
            
            npcImageToShow = correctAnswer ? npcImages[1] : npcImages[2];
            
            npcImageToShow.gameObject.SetActive(true);
            
            dialogWindow.alpha = 0f;
            
            dialogText.text = correctAnswer ? NpcDialogs.GetRandomDialogSuccessAnswer() : NpcDialogs.GetRandomDialogErrorAnswer();
            
            dialogText.maxVisibleCharacters = 0;
            
            
            npcDialogWindowObject.SetActive(true);

            Sequence fadeNpcSequence = DOTween.Sequence().Append(npcImageToShow.DOFade(1f, 0.7f)).PrependInterval(0.3f);
            dialogWindow.DOFade(1f, 1f);

            fadeNpcSequence.OnComplete(() => { StartCoroutine(TimedTypewriterEffect()); });
        }
        
        private IEnumerator TypewriterEffect()
        {
            dialogText.ForceMeshUpdate();

            TMP_TextInfo dialogTextInfo = dialogText.textInfo;

            int totalVisibleCharacters = dialogTextInfo.characterCount;

            int visibleCount = 1;

            while (visibleCount != totalVisibleCharacters + 1)
            {
                yield return new WaitForSeconds(delayBetweenWords);
                
                dialogText.maxVisibleCharacters = visibleCount;
                
                
                visibleCount += 1;
            }
            
            questsManager.QuestDialogEnded();
            
        }
        
        private IEnumerator TimedTypewriterEffect()
        {
            dialogText.ForceMeshUpdate();

            TMP_TextInfo dialogTextInfo = dialogText.textInfo;

            int totalVisibleCharacters = dialogTextInfo.characterCount;

            int visibleCount = 1;

            while (visibleCount != totalVisibleCharacters+1)
            {
                yield return new WaitForSeconds(delayBetweenWords);
                
                dialogText.maxVisibleCharacters = visibleCount;
                
                
                visibleCount += 1;
            }

            Sequence fadeNpcSequence = DOTween.Sequence().Append(npcImageToShow.DOFade(0f, 0.7f)).PrependInterval(2.3f);
            Sequence sequence = DOTween.Sequence().Append(dialogWindow.DOFade(0f, 1f)).PrependInterval(2f);
            
            fadeNpcSequence.OnComplete(() =>
            {
                questsManager.FinishedNpcDeliverDialog();    
                npcDialogWindowObject.SetActive(false);
            });
            
        }

        private void DeactivateAllNpcImages()
        {
            foreach (var image in npcImages)
            {
                image.gameObject.SetActive(false);
                image.alpha = 0;
            }
        }
    }
}
