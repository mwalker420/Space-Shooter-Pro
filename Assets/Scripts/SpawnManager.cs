using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;

    [SerializeField]
    private float _waitTime = 5.0f;

    private bool _stopSpawning = false;


    void Start()
    {
        
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
            int randomPowerUp = Random.Range(0, 3);
            GameObject newPowerup = Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);

            newPowerup.transform.parent = transform;
            yield return new WaitForSeconds(Random.Range(3, 8));
        }

    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
