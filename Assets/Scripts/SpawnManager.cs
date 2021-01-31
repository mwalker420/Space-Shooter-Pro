using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpEntry
{
    public GameObject powerUp;
    public int spawnWeight = 10; // The larger the relative weight the higher chances of spawning.
}

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _spawnItemContainer;

    private bool _stopSpawning = false;

    [SerializeField]
    private List<WaveObject> _waves = new List<WaveObject>();
    [SerializeField]
    private float _timeBetweenWaves;

    private void BuildWeightedLookupTable(List<SpawnItem> spawnItems, out List<int> weightedIndexLookupList, out int weightedSpawnTotal)
    {
        // SMELL: This is probably not particulary memory efficient but
        // so long as the weights for the handful of spawnItems
        // don't get out of control this should suffice.
        weightedIndexLookupList = new List<int>();
        for (var idx = 0; idx < spawnItems.Count; idx++)
        {
            SpawnItem spawnItem = spawnItems[idx];
            for (var i = 0; i < spawnItem.spawnWeight; i++)
            {
                weightedIndexLookupList.Add(idx);
            }
        }
        weightedSpawnTotal = weightedIndexLookupList.Count;
    }

    public void StartSpawning()
    {

        StartCoroutine(SpawnEnemiesRoutine());
        StartCoroutine(SpawnPowerupsRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        List<int> weightedIndexLookupList;
        int weightedSpawnTotal;

        yield return new WaitForSeconds(3.0f); //initial pause before starting

        int waveIdx = 0;
        foreach (var wave in _waves)
        {
            Debug.Log("Wave " + waveIdx);
            waveIdx++;


            BuildWeightedLookupTable(wave.enemies, out weightedIndexLookupList, out weightedSpawnTotal);

            int spawnCount = 0;
            while (spawnCount < wave.totalEnemies && _stopSpawning == false)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9.0f), 8f, 0);

                int randomWeightedIndex = Random.Range(0, weightedSpawnTotal);
                int powerUpIndex = weightedIndexLookupList[randomWeightedIndex];


                GameObject newSpawnItem = Instantiate(wave.enemies[powerUpIndex].itemPrefab, posToSpawn, Quaternion.identity);
                newSpawnItem.transform.parent = _spawnItemContainer.transform;

                spawnCount++;
                yield return new WaitForSeconds(wave.timeBetweenEnemySpawnItems);

            }

            if (_stopSpawning)
            {
                break;
            }
            yield return new WaitForSeconds(_timeBetweenWaves);

        }
        Debug.Log("Finished with waves");
    }

    IEnumerator SpawnPowerupsRoutine()
    {
        List<int> weightedIndexLookupList;
        int weightedSpawnTotal;

        yield return new WaitForSeconds(3.0f); //initial pause before starting

        int waveIdx = 0;
        foreach (var wave in _waves)
        {
            Debug.Log("Wave " + waveIdx);
            waveIdx++;

            BuildWeightedLookupTable(wave.powerups, out weightedIndexLookupList, out weightedSpawnTotal);

            int spawnCount = 0;
            while (spawnCount < wave.totalPowerups && _stopSpawning == false)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9.0f), 8f, 0);

                int randomWeightedIndex = Random.Range(0, weightedSpawnTotal);
                int powerUpIndex = weightedIndexLookupList[randomWeightedIndex];

                GameObject newSpawnItem = Instantiate(wave.powerups[powerUpIndex].itemPrefab, posToSpawn, Quaternion.identity);
                newSpawnItem.transform.parent = _spawnItemContainer.transform;

                spawnCount++;
                yield return new WaitForSeconds(wave.timeBetweenPowerupSpawnItems);
            }

            if (_stopSpawning)
            {
                break;
            }
            yield return new WaitForSeconds(_timeBetweenWaves);

        }
        Debug.Log("Finished with waves");
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
