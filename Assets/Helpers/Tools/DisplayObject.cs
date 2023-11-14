using System;
using System.Collections.Generic;
using UnityEngine;


namespace Inside
{
    public class DisplayObject : MonoBehaviour
    {
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }

        public void Solo()
        {
            ForEachSibling((Transform t) =>
            {
                t.gameObject.SetActive(false);
            });

            gameObject.SetActive(true);
        }

        public void HideAll()
        {
            ForEachSibling((Transform t) =>
            {
                t.gameObject.SetActive(false);
            });

            gameObject.SetActive(false);
        }

        protected virtual void ForEachSibling(Action<Transform> action)
        {
            foreach (Transform s in transform.parent)
            {
                if (s == this.transform) continue;
                if (s.GetComponent<DisplayObject>() == null) continue;

                action.Invoke(s);
            }
        }
    }
}
