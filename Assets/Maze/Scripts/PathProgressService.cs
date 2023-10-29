using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeCore.types;
using MazeCore.enums;

public class PathProgressService : MonoBehaviour
{
    public LayerMask pathLayerMask;
    public Transform playerTransform;

    public bool IsOnCorrectPath()
    {
        RaycastHit hit;

        // if (Physics.Raycast(playerTransform.position, Vector3.down, out hit, 2f, pathLayerMask))
        // {
        //     MazeCellComponent cellComponent = hit.collider.GetComponent<MazeCellComponent>();
        //     if (cellComponent != null && cellComponent.isOnPathToExit)
        //     {
        //         Debug.Log("Player is on the correct path!");
        //         return true;
        //     }
        // }

        return false;
    }



    void Update()
    {
        if (IsOnCorrectPath())
        {
            
        }
    }
}


