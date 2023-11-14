using System;
using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    [CreateAssetMenu(fileName = "ParallaxEventChannel", menuName = "ScriptableObjects/EnvironmentRunner/ParallaxEventChannel")]
    public class ParallaxEventChannelSO : ScriptableObject
    {
        //changed position to the end of the line of the parallax, in other words, it is ready to be randomized again to be shown to the player
        public event Action gotParallaxed;

        public void OnGotParallaxed()
        {
            gotParallaxed?.Invoke();
        }
    }
}
