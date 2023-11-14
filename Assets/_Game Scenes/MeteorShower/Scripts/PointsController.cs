using System;
using UnityEngine;

namespace _MeteorShower.Scripts
{
    public class PointsController : MonoBehaviour
    {
        [SerializeField] private PointsManagerSO pointsManager;

        private void Start()
        {
            pointsManager.Reset();
        }
    }
}
