using System;
using UnityEngine;

namespace _SpaceshipCombat.Scripts
{
    public class PlayerSpaceshipTriggerInteractor : MonoBehaviour
    {
        public event Action<bool, DeskAnswerHolder> playerCollidingWithDesk;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("SpaceshipDesk"))
            {
                if (other.TryGetComponent(out DeskAnswerHolder deskAnswerHolder))
                {
                    //if (!deskAnswerHolder.CanBeInteracted) return;
                    playerCollidingWithDesk?.Invoke(true, deskAnswerHolder);
                }
                //    Debug.Log("Collided With desk");
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("SpaceshipDesk"))
            {
                if (other.TryGetComponent(out  DeskAnswerHolder deskAnswerHolder))
                {
                    //if (!deskAnswerHolder.CanBeInteracted) return;
                
                    playerCollidingWithDesk?.Invoke(false, deskAnswerHolder);
                }
            
                //      Debug.Log("Left desk");
            }
        }

        public void ForceTriggerExit()
        {
            playerCollidingWithDesk?.Invoke(false, null);
//        Debug.Log("Left desk");
        }
    }
}
