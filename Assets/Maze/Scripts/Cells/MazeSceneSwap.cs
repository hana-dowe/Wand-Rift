using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeSceneSwap : MonoBehaviour
{
    public string sceneToLoad; 

    private void Start()
    {
        Debug.Log("Scene swap script loaded");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered trigger");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}