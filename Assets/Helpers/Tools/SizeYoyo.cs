using DG.Tweening;
using UnityEngine;

namespace Helpers.Tools
{
    public class SizeYoyo : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float targetScale;
        [SerializeField] private float time;
        [SerializeField] private Ease easeType = Ease.InOutSine;
        [SerializeField] private int loops = -1;
        [SerializeField] private bool executeOnStart;
        
        void Start()
        {
            if (executeOnStart)
            {
                AnimateYoYo();
            }    
        }

        public void AnimateYoYo()
        {
            target.DOScale(targetScale, time).SetEase(easeType).SetLoops(loops, LoopType.Yoyo);
        }
    }
}
