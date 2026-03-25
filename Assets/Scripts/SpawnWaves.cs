using System.Collections;
using UnityEngine;

public class SpawnWaves : MonoBehaviour
{
    [SerializeField] private int[] waveNumberOfSpiders;

    [SerializeField] private GameObject spiderPrefab;

    private int waveCounter;

    public Vector3 minBounds;
    public Vector3 maxBounds;

    private void Start()
    {
        waveCounter = 0;
        
        StartCoroutine(SpawnWaveWithDelay());
    }

 

    private void SpawnSpiderWave(int waveCounterNumber)
    {

        Debug.Log("SpawnSpiderWave function called!");
        switch(waveCounterNumber){
            case 0:
                for (int i = 0; i < waveNumberOfSpiders[0]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                }
                break;
            case 1:
                for (int i = 0; i < waveNumberOfSpiders[1]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                }
                break;
            case 2:
                for (int i = 0; i < waveNumberOfSpiders[2]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                }
                break;
            case 3:
                for (int i = 0; i < waveNumberOfSpiders[3]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                }
                break;
            default:
                break;
        }

        
        /*for (int x = 0; x < waveNumberOfSpiders[waveCounterNumber] - 1; x++)
        {
            Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
            Debug.Log("Instantiated spider!");
        }*/
        
    }

    private IEnumerator SpawnWaveWithDelay()
    {
        for (int i = 0; i < waveNumberOfSpiders.Length; i++)
        {
            Debug.Log("Spawning Wave " + waveCounter);
            SpawnSpiderWave(waveCounter);
            waveCounter++;
            yield return new WaitForSeconds(20f);
        }
    }

    public Vector3 GetRandomPositionInArea()
    {
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);
        float randomZ = Random.Range(minBounds.z, maxBounds.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}
