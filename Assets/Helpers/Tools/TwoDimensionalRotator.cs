using DG.Tweening;
using UnityEngine;

namespace Helpers.Tools
{
    public class TwoDimensionalRotator : MonoBehaviour
    {
        [SerializeField] private Transform objectToRotate;
        [SerializeField] private float duration = 2f;
        private void Start()
        {
            objectToRotate.DOLocalRotate(new Vector3(0, 0, -360), duration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Restart);
        }

    }
}
