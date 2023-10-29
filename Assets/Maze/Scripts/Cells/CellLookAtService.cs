using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellLookAtService : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask wallsLayerMask; // Only for the walls
    public LayerMask deadEndsLayerMask; // Only for the deadends

    public bool IsLookingAtDeadEnd()
    {
        RaycastHit hit;

        // detect deadends and walls
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity, wallsLayerMask | deadEndsLayerMask))
        {   
            //need to add a check for the deadend layer to prevent seeing through walls
            if ((1 << hit.collider.gameObject.layer) == wallsLayerMask.value)
            {
                Debug.Log("Blocked by a wall!");
            }
            else if ((1 << hit.collider.gameObject.layer) == deadEndsLayerMask.value)
            {
                // Perform a second raycast to check for walls between the camera and the hit point
                if (!Physics.Raycast(playerCamera.transform.position, hit.point - playerCamera.transform.position, Vector3.Distance(hit.point, playerCamera.transform.position), wallsLayerMask))
                {
                    Debug.Log("Looking at a deadend!");
                    
                    return true;
                }
            }
        }
    
        return false;
    }
    
 
}
