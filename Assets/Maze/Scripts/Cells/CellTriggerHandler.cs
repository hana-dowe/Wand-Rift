using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeCore.enums;

public class CellTriggerHandler : MonoBehaviour
{
    public CellType cellType;
    public GameObject player; // Public reference to the player object


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player) // Check if the player has entered the trigger
        {
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
