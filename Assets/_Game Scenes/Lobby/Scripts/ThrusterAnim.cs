using DG.Tweening;
using UnityEngine;

namespace _Game_Scenes.Lobby.Scripts
{
    public class ThrusterAnim : MonoBehaviour
    {
        [SerializeField] private Transform thrusterTransform;
        
        // Start is called before the first frame update
        void Start()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(thrusterTransform.DOScaleX(1.15f, 0.3f).SetEase(Ease.OutBack, 3));
            sequence.SetLoops(-1, LoopType.Yoyo);
        }
    }
}
