using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerTriggerInteractor : MonoBehaviour
{
    private bool collidingWithNpc;
    //private List<VillageNpcDataSO> collidingNpcsDatas = new List<VillageNpcDataSO>();
    
    private VillageNpcDataSO collidingNpcData;
    public event Action<VillageNpcDataSO> collidedWithNpc;
    public event Action<VillageNpcDataSO> uncollidedWithNpc;

    private Coroutine turnRoutine;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VillageNpc"))
        {
            if (other.TryGetComponent(out VillageNpcInfoHolder villageNpcInfoHolder))
            {
                if (!villageNpcInfoHolder.NpcData.CanDialog) return;
                
                collidingNpcData = villageNpcInfoHolder.NpcData;
                collidedWithNpc?.Invoke(collidingNpcData);
            }
            
            //collidingNpcsDatas.Add(other.transform);
            //Debug.Log(collidingNpcsDatas.Count);
           // Debug.Log("Collided With NPC");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("VillageNpc"))
        {
            if (other.TryGetComponent(out VillageNpcInfoHolder villageNpcInfoHolder))
            {
                if (!villageNpcInfoHolder.NpcData.CanDialog) return;
                
                if(turnRoutine != null)
                    StopCoroutine(turnRoutine);
                
                turnRoutine = StartCoroutine(NpcReturnToOriginalRotation(other.transform));
                collidingNpcData = villageNpcInfoHolder.NpcData;
                uncollidedWithNpc?.Invoke(collidingNpcData);
            }

            //collidingNpcsDatas.Remove(other.transform);
         //   Debug.Log("Left NPC");
        }
    }

    private IEnumerator NpcReturnToOriginalRotation(Transform npc)
    {
        yield return new WaitForSeconds(1f);
        npc.DOLocalRotate(Vector3.zero, 0.7f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("VillageNpc"))
        {
            other.transform.DOLookAt(this.transform.position, 0.4f);
        }
    }
}
