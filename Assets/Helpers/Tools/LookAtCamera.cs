using System;
using UnityEngine;

namespace Helpers.Tools
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private bool inverse;

        void Update()
        {
            if(inverse)
                this.transform.LookAt(2 * this.transform.position - Camera.main.transform.position ) ;
            else
                this.transform.LookAt(Camera.main.transform.position ) ;
            
        }
    }
}
