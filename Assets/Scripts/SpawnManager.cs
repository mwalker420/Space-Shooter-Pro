using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpEntry
{
    public GameObject powerUp;
    public int spawnWeight = 1; // The larger the relative weight the higher chances of spawning.
}

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private PowerUpEntry[] _powerupEntries;

    private int _weightedSpawnTotal;
    private List<int> _weightedIndexLookupList = new List<int>();

    [SerializeField]
    private float _waitTime = 5.0f;

    private bool _stopSpawning = false;

    public float advancedMovementProbability = 0.3f;

    private void Start()
    {
        BuildWeightedLookupTable();
    }

    private void BuildWeightedLookupTable()
    {
        // This is probably not particulary memory efficient but
        // so long as the weights for the handful of powerups
        // don't get out of control this should suffice.
        for (var idx = 0; idx < _powerupEntries.Length; idx++)
        {
            PowerUpEntry entry = _powerupEntries[idx];
            for (var i = 0; i < entry.spawnWeight; i++)
            {
                _weightedIndexLookupList.Add(idx);
            }
        }
        _weightedSpawnTotal = _weightedIndexLookupList.Count;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.0f, 9.0f), 8f, 0);

            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            bool useAdvancedMovement = Random.value <= advancedMovementProbability;
            newEnemy.GetComponent<Enemy>().useAdvancedMovement = useAdvancedMovement;
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_waitTime);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9.0f), 8f, 0);

            int randomWeightedIndex = Random.Range(0, _weightedSpawnTotal);
            int powerUpIndex = _weightedIndexLookupList[randomWeightedIndex];

            GameObject newPowerup = Instantiate(_powerupEntries[powerUpIndex].powerUp, posToSpawn, Quaternion.identity);

            newPowerup.transform.parent = transform;
            yield return new WaitForSeconds(Random.Range(3, 8));
        }

    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
