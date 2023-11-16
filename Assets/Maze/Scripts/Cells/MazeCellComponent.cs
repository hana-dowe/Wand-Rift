using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeCore.types;
using MazeCore.enums;

public class MazeCellComponent : MonoBehaviour
{   
    public int x;
    public int y;

    public CellType cellType;
    public JunctionType junctionType;
    public bool isOnPathToExit;
    public Quaternion rotation;
    public Vector3 worldPosition;


}
