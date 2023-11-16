using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeCore.enums;

public class CellTriggerHandler : MonoBehaviour
{
    public CellType cellType;
    public GameObject player; 


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player) 
        {
            Debug.Log("Player has entered the trigger");
            // Do something with the cell type
        //     switch(cellType)
        //     {
        //         // case CellType.PATH:
        //         //     Debug.Log("Player is on the correct path.");
        //         //     // Trigger your path event
        //         //     break;
        //         // case CellType.DEADEND:
        //         //     Debug.Log("Player is approaching a dead end.");
        //         //     // Trigger your dead end event
        //         //     break;
        //     }
        // }
        }
    }
}
