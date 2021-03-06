﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField]
    private float _rotationSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;



    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1) * _rotationSpeed * Time.deltaTime, Space.Self);
    }

    // check for laser collision (trigger)
    // instantiate explosion at position of asteriod (this)
    // destroy the explosion after 3 seconds
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.5f);
        }
    }

}
