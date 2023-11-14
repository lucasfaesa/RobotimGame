using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcLocation : MonoBehaviour
{
    [SerializeField] private StreetInfoSO streetInfo;

    public StreetInfoSO StreetInfo => streetInfo;
}
