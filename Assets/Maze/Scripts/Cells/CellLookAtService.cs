using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellLookAtService : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask wallsLayerMask; // Only for the walls
    public LayerMask deadEndsLayerMask; // Only for the dead ends

    public bool IsLookingAtDeadEnd()
    {
        RaycastHit hit;
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 1000, Color.red, 1f); // Debug raycast
        // Raycast to detect dead ends and walls
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity, wallsLayerMask | deadEndsLayerMask))
        {
            
            if (hit.collider.gameObject.layer == 14) // 14 is the dead ends layer
            {
                return true;
            }
        }

        return false;
    }
}
