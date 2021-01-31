using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnItem
{
    public GameObject itemPrefab;
    public int spawnWeight = 10; 
}

[CreateAssetMenu(fileName = "Wave1", menuName = "ScriptableObjects/Wave", order = 1)]
public class WaveObject : ScriptableObject
{
    public int totalEnemies = 1;
    public List<SpawnItem> enemies = new List<SpawnItem>();
    [Range(0.5f, 3f)]
    public float timeBetweenEnemySpawnItems = 3f;

    public int totalPowerups = 1;
    public List<SpawnItem> powerups = new List<SpawnItem>();
    [Range(0.5f, 3f)]
    public float timeBetweenPowerupSpawnItems = 3f;


}
