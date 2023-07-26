using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnItems;

    [SerializeField] private float timeReloadSpawn;

    [SerializeField] private Transform spawnPoint;

    private bool _isCompleteLevel;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SpawnController = this;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        Instantiate(spawnItems[Random.Range(0, spawnItems.Length )], spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(timeReloadSpawn);
        if (!GameManager.Instance.IsCompleteLevel)
            StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
    }
}