using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StreetInfo", menuName = "ScriptableObjects/VillageScene/StreetInfo", order = 6)]
public class StreetInfoSO : ScriptableObject
{
    [field:SerializeField] [field: ReadOnly] public string StreetName { get; set; }
    [SerializeField] private List<StreetInfoSO> parallelStreets;
    [SerializeField] private List<StreetInfoSO> concurrentStreets;
    
    public List<StreetInfoSO> GetParallelStreets() => parallelStreets;
    public List<StreetInfoSO> GetConcurrentStreets() => concurrentStreets;
}
