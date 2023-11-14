using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class NpcDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject[] npcObjects;

        public void ToggleNpc(bool status)
        {
            foreach (var objs in npcObjects)
            {
                objs.SetActive(status);
            }
        }        
    }
}
