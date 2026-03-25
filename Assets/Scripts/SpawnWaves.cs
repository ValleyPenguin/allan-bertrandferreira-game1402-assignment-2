using System.Collections;
using UnityEngine;

public class SpawnWaves : MonoBehaviour
{
    [SerializeField] private int[] waveNumberOfSpiders;

    [SerializeField] private GameObject spiderPrefab;

    private int waveCounter;

    public Vector3 minBounds;
    public Vector3 maxBounds;

    public int numberOfSpidersSpawned;
    
    public int numberOfSpidersKilled;

    public bool didPlayerWin;

    private void Start()
    {
        didPlayerWin = false;
        numberOfSpidersSpawned = 0;
        waveCounter = 0;
        
        StartCoroutine(SpawnWaveWithDelay());

    }

    private void Update()
    {
        if (waveCounter == waveNumberOfSpiders.Length)
        {
            Debug.Log("In final wave!");
            if (numberOfSpidersKilled >= numberOfSpidersSpawned && !didPlayerWin)
            {
                Toast.Instance.ShowToast("You killed all the spiders, you win!");
                didPlayerWin = true;
            }
        }
    }

    private void SpawnSpiderWave()
    {

        Debug.Log("SpawnSpiderWave function called!");
   
        switch(waveCounter){
            case 0:
                for (int i = 0; i < waveNumberOfSpiders[0]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                    numberOfSpidersSpawned++;
                }
                break;
            case 1:
                for (int i = 0; i < waveNumberOfSpiders[1]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                    numberOfSpidersSpawned++;
                }
                break;
            case 2:
                for (int i = 0; i < waveNumberOfSpiders[2]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                    numberOfSpidersSpawned++;
                }
                break;
            case 3:
                for (int i = 0; i < waveNumberOfSpiders[3]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                    numberOfSpidersSpawned++;
                }
                break;
            case 4:
                for(int i = 0; i < waveNumberOfSpiders[4]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                    numberOfSpidersSpawned++;
                }
                break;
            case 5:
                for (int i = 0; i < waveNumberOfSpiders[5]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                    numberOfSpidersSpawned++;
                }
                break;
            case 6:
                for (int i = 0; i < waveNumberOfSpiders[6]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                    numberOfSpidersSpawned++;
                }
                break;
            case 7:
                for (int i = 0; i < waveNumberOfSpiders[7]; i++)
                {
                    Instantiate(spiderPrefab, GetRandomPositionInArea(), Quaternion.identity);
                    numberOfSpidersSpawned++;
                }
                break;
            default:
                break;
        }

        Debug.Log(numberOfSpidersSpawned);
    }

    private IEnumerator SpawnWaveWithDelay()
    {
        for (int i = 0; i < waveNumberOfSpiders.Length; i++)
        {
            Debug.Log("Spawning Wave " + waveCounter);
            SpawnSpiderWave();
            Toast.Instance.ShowToast("Spawning Wave " + waveCounter + "! (" + waveNumberOfSpiders[waveCounter] + " Spiders!)");
            waveCounter++;
            Toast.Instance.HideToastWithDelay(2f);
            yield return new WaitForSeconds(10f);
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
