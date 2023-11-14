using System;
using DG.Tweening;
using UnityEngine;

namespace _MeteorShower.Scripts
{
    public class DoorAnimation : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManager;


        private void OnEnable()
        {
            gameManager.preparingToStartGame += Animate;
        }

        private void OnDisable()
        {
            gameManager.preparingToStartGame -= Animate;
        }

        private void Animate()
        {
            this.transform.DOLocalRotate(new Vector3(0f, 200f, 0f), 1f).SetEase(Ease.InOutBack);
        }

    }
}
