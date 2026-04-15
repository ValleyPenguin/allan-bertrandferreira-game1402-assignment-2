using UnityEngine;
using System.Collections;

public class FrogSpawner : MonoBehaviour
{
    [SerializeField] private Transform minBounds;
    [SerializeField] private Transform maxBounds;

    private RaycastHit hit;

    [SerializeField] private GameObject frogPrefab;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private int maxFrogs = 67;

    private int _currentFrogCount = 0;

    private bool _spawnFrogs;

    private Vector3 _currentSpawnTarget;

    private void Start()
    {
        _spawnFrogs = true;
        StartCoroutine(SpawnFrogWithDelay(4f));
    }

    private Vector3 findRandomSpotToSpawnFrog()
    {
        float randomX = Random.Range(minBounds.position.x, maxBounds.position.x);
        float randomZ = Random.Range(minBounds.position.z, maxBounds.position.z);
        
        if (Physics.Raycast(new Vector3(randomX, minBounds.position.y, randomZ), Vector3.down, out hit, 9999f, groundLayer))
        {
            _currentSpawnTarget = hit.point;
            return hit.point;
        }
        else
        {
            return new Vector3(0f, 0f, 0f);
        }    
    }

    private IEnumerator SpawnFrogWithDelay(float delay)
    {
        while (_spawnFrogs)
        {
            if(_currentFrogCount <= maxFrogs)
            {
                Vector3 spawnPoint = findRandomSpotToSpawnFrog();
                Instantiate(frogPrefab, spawnPoint, Quaternion.identity);
                Debug.Log("Spawned a frog at: " + spawnPoint);
                _currentFrogCount++;
            }

            yield return new WaitForSeconds(delay);
        }
    }
}
