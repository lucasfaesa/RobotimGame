using System;
using UnityEngine;

namespace _Village.Scripts
{
    [CreateAssetMenu(fileName = "LevelCompletion", menuName = "ScriptableObjects/VillageScene/LevelCompletion")]
    public class LevelCompletionSO : ScriptableObject
    {
        public bool LevelCompleted { get; set; } //if the player loses this will be false

        public void Reset()
        {
            LevelCompleted = false;
        }
    }
}
