using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "3dObjectInfo", menuName = "ScriptableObjects/VillageScene/3dObjectInfo", order = 11)]

public class ThreeDObjectInfoSO : ScriptableObject
{
    public enum ThreeDObjectShape {Polyhedron, RoundBody}

    [SerializeField] private ThreeDObjectShape objectShape;
    [SerializeField] private int numberOfFaces;
    [SerializeField] private int numberOfVertices;
    [SerializeField] private int numberOfEdges;

    public ThreeDObjectShape ObjectShape => objectShape;
    public int NumberOfFaces => numberOfFaces;
    public int NumberOfVertices => numberOfVertices;
    public int NumberOfEdges => numberOfEdges;

}
