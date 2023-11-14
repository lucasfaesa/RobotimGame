using _MeteorShower.Scripts;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class CrateNpcAnimation : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManagerSo;
        [SerializeField] private Transform headTransform;
        private Sequence headMovementSequence;
        
        private int headTarget = 18;
        
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
            Vector3 targetPos = new Vector3(-63.274f,-10.704f , headTarget = headTarget == 18 ? -28 : 18);

            headMovementSequence = DOTween.Sequence().Append(headTransform.DOLocalRotate(targetPos, 0.4f).SetEase(Ease.InOutSine));
            headMovementSequence.OnComplete(AnimateHead);
        }
    }
}
