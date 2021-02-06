﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    public enum PowerUpId
    {
        TripleShot, Speed, Shield, Ammo, Health, Missiles, Negative
    };
    [SerializeField]
    private PowerUpId powerupID;

    private AudioManager _audioManager;

    private bool _moveToPlayer;
    [SerializeField]
    private float _moveToPlayerSpeed = 10.0f;

    private GameObject _player;

    private void Start()
    {
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("AudioManager null for Powerup");
        }

        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.LogError("Player null for Powerup");
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            _moveToPlayer = true;
        }

        if (_moveToPlayer)
        {
            MoveToPlayer();
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }


        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }

    }

    private void MoveToPlayer()
    {
        if (_player != null)
        {
            Vector3 direction = _player.transform.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * _moveToPlayerSpeed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupID)
                {
                    case PowerUpId.TripleShot:
                        player.EnableTripleShot();
                        break;
                    case PowerUpId.Speed:
                        player.EnableSpeedBoost();
                        break;
                    case PowerUpId.Shield:
                        player.EnableShield();
                        break;
                    case PowerUpId.Ammo:
                        player.ReplenishAmmo();
                        break;
                    case PowerUpId.Health:
                        player.IncreaseHealth();
                        break;
                    case PowerUpId.Missiles:
                        player.EnableMissiles();
                        break;
                    case PowerUpId.Negative:
                        player.SlowDown();
                        break;
                    default:
                        Debug.Log("Default value");
                        break;
                }


            }
            if (powerupID == PowerUpId.Negative)
            {
                _audioManager.PlayPowerDown();
            }
            else
            {
                _audioManager.PlayPowerup();
            }

            Destroy(gameObject);
        }

        if (other.tag == "EnemyLaser")
        {
            Destroy(gameObject);
        }
    }


}
