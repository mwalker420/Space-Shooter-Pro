using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave1", menuName = "ScriptableObjects/Wave", order = 1)]
public class WaveObject : ScriptableObject
{

    public List<GameObject> enemies = new List<GameObject>();

    [Range(0, 1)]
    public float advancedMovementProbability;

    [Range(0.5f, 3f)]
    public float timeBetweenEnemies = 3f;
}
