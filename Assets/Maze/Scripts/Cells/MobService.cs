using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MobService : MonoBehaviour
{
    // Reference to the mob prefabs
    public GameObject tankyPrefab;
    public GameObject fastPrefab;
    public GameObject adcPrefab;

    // List to hold all the prefabs
    private List<GameObject> mobPrefabs;

    private void Start()
    {
        // Initialize the list and add the prefabs
        mobPrefabs = new List<GameObject>
        {
            tankyPrefab,
            fastPrefab,
            adcPrefab
        };
        SpawnRandomMob();
    }

    // Call this method when you want to spawn a random mob
    public void SpawnRandomMob()
    {
        if (mobPrefabs == null || mobPrefabs.Count == 0)
        {
            Debug.LogError("No mob prefabs set.");
            return;
        }
        if (Random.Range(0, 100) > 30)
        {
            return;
        }
        // Choose a random prefab from the list
        GameObject selectedPrefab = mobPrefabs[Random.Range(0, mobPrefabs.Count)];

        // Instantiate (spawn) the selected mob at this object's location
        Instantiate(selectedPrefab, transform.position, Quaternion.identity);
    }
}


