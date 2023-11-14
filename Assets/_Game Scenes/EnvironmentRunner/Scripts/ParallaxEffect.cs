using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Game_Scenes.EnvironmentRunner.Scripts
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private List<TargetData> targetsToMove;
        [SerializeField] private float speed = 5;
        [Header("If you want to raise OnGotParallaxed event after a specific index (to give the player a free start without obstacles)")]
        [SerializeField] private bool raiseParallexEventAfterIndex;
        [SerializeField] private int indexToRaiseEventAfter;
        
        private void Start()
        {
            for (int i = 0; i < targetsToMove.Count; i++)
            {
                targetsToMove[i] = new TargetData(targetsToMove[i].transform, 
                                                    targetsToMove[i].collider, 
                                                        targetsToMove[i].transform.localPosition, 
                                                        targetsToMove[i].collider.bounds.size.z * (i +1),
                                                                targetsToMove[i].parallaxEventChannelSo);
                
                if(raiseParallexEventAfterIndex && i >= indexToRaiseEventAfter)
                    targetsToMove[i].parallaxEventChannelSo.OnGotParallaxed();
                    
            }
        }

        void Update()
        {
            for (int i = 0; i < targetsToMove.Count; i++)
            {
                targetsToMove[i].transform.Translate(Vector3.back * (speed * Time.deltaTime));

                if (targetsToMove[i].initialPos.z - targetsToMove[i].transform.localPosition.z >= targetsToMove[i].moveAmount)
                {
                    var offscreenTarget = targetsToMove[i];
                    var lastTargetOfList = targetsToMove[^1];

                    offscreenTarget.transform.localPosition =
                        new Vector3(offscreenTarget.transform.localPosition.x,
                            offscreenTarget.transform.localPosition.y,
                            lastTargetOfList.transform.localPosition.z + offscreenTarget.collider.bounds.size.z);

                    offscreenTarget.moveAmount = targetsToMove.Count * offscreenTarget.collider.bounds.size.z;
                    offscreenTarget.initialPos = offscreenTarget.transform.localPosition;
                    
                    targetsToMove.RemoveAt(0);
                    targetsToMove.Add(offscreenTarget);
                    
                    if(offscreenTarget.parallaxEventChannelSo != null)
                        offscreenTarget.parallaxEventChannelSo.OnGotParallaxed();
                }
            }
        }

        [Serializable]
        class TargetData
        {
            public Transform transform;
            public BoxCollider collider;

            [Header("Optional")] 
            public ParallaxEventChannelSO parallaxEventChannelSo;

            [HideInInspector] public Vector3 initialPos;
            [HideInInspector] public float moveAmount;

            public TargetData(Transform trans, BoxCollider col, Vector3 pos, float amount, 
                                ParallaxEventChannelSO parEventChannel)
            {
                transform = trans;
                collider = col;
                initialPos = pos;
                moveAmount = amount;
                parallaxEventChannelSo = parEventChannel;
            }
        }
    }
}
