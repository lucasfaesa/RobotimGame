using _MeteorShower.Scripts;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class BooksNpcAnimation : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManagerSo;
        [SerializeField] private Transform headTransform;
        private Sequence headMovementSequence;
        
        private int headTarget = 347;
        
        private void OnEnable()
        {
            gameManagerSo.gameEnded += KillTweens;
        }

        private void OnDisable()
        {
            gameManagerSo.gameEnded -= KillTweens;
        }

        private void KillTweens()
        {
            headMovementSequence.Kill();
        }        
        void Start()
        {
            AnimateHead();
        }

        private void AnimateHead()
        {
            Vector3 targetPos = new Vector3(-8.616f, headTarget = headTarget == 347 ? 23 : 347, -1.265f);

            headMovementSequence = DOTween.Sequence().Append(headTransform.DOLocalRotate(targetPos, 0.4f).SetEase(Ease.InOutSine));
            headMovementSequence.OnComplete(AnimateHead);
        }

    }
}
