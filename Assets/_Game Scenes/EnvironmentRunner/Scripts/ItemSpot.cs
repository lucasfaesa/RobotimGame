using UnityEngine;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class ItemSpot : MonoBehaviour
    {
        private ItemObject currentItemOnThisSpot;
        
        public void SetItemOnSpot(ItemObject itemObject, int spotIndex)
        {
            currentItemOnThisSpot = itemObject;
            itemObject.ActivateItem(this.transform);
        }
        
        public void Clear()
        {
            if(currentItemOnThisSpot != null)
                currentItemOnThisSpot.DeactivateItem();

            currentItemOnThisSpot = null;
        }
    }
}
