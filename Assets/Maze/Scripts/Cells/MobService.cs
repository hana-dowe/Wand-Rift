using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MobService : MonoBehaviour
{
    // mobtypes
    public GameObject tankyPrefab;
    public GameObject fastPrefab;
    public GameObject adcPrefab;

    private List<GameObject> mobPrefabs;

    private void Start()
    {
        mobPrefabs = new List<GameObject>
        {
            tankyPrefab,
            fastPrefab,
            adcPrefab
        };
        SpawnRandomMob();
    }

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
        // random mob
        GameObject selectedPrefab = mobPrefabs[Random.Range(0, mobPrefabs.Count)];
        Instantiate(selectedPrefab, transform.position, Quaternion.identity);
    }
}


