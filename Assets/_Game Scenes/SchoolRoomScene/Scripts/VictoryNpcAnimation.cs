using _MeteorShower.Scripts;
using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class VictoryNpcAnimation : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManagerSo;
        [SerializeField] private Transform[] npcArmsTransform;
        [SerializeField] private Transform npcLightBulbTransform;
        
        private Sequence armsSequence;
        private Sequence bulbSequence;
        
        void Start()
        {
            armsSequence = DOTween.Sequence().Append(npcArmsTransform[0].DOLocalRotate(new Vector3(360f, 0f, 0f), 0.8f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
            armsSequence.Insert(0,npcArmsTransform[1].DOLocalRotate(new Vector3(360f, 0, 0), 0.8f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
            
            bulbSequence = DOTween.Sequence().Append(npcLightBulbTransform.DOScale(0.1212116f, 0.3f).SetEase(Ease.InOutSine));
            bulbSequence.Append(npcLightBulbTransform.DOScale(0.1022365f, 0.3f).SetEase(Ease.InOutSine));
            
            AnimateArms();
            AnimateLightBulb();
        }
        
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
            armsSequence.Kill();
            bulbSequence.Kill();
        }

        private void AnimateArms()
        {
            armsSequence.Restart();
            armsSequence.OnComplete(AnimateArms);
        }

        private void AnimateLightBulb()
        {
            bulbSequence.Restart();
            bulbSequence.OnComplete(AnimateLightBulb);
        }
        
    }
}
