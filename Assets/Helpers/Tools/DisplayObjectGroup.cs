using System;
using UnityEngine;


namespace Inside
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DisplayObjectGroup : DisplayObject
    {
        public void SoloInteractable()
        {
            ForEachSibling((CanvasGroup cg) =>
            {
                cg.interactable = false;
            });

            CanvasGroup.interactable = true;
        }

        public void SetAllInteractable(bool state)
        {
            ForEachSibling((CanvasGroup cg) =>
            {
                cg.interactable = state;
            });

            CanvasGroup.interactable = state;
        }

        protected virtual void ForEachSibling(Action<CanvasGroup> action)
        {
            foreach (Transform s in transform.parent)
            {
                if (s == this.transform) continue;
                if (s.GetComponent<DisplayObjectGroup>() == null) continue;

                action.Invoke(s.GetComponent<CanvasGroup>());
            }
        }


        private CanvasGroup _canvasGroup;
        private CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                {
                    _canvasGroup = this.GetComponent<CanvasGroup>();
                }

                return _canvasGroup;
            }
        }
    }
}
