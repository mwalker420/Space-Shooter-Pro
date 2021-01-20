using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    // ID for Powerups
    //0 = Triple shot
    //1 = Speed
    //2 = Shield
    [SerializeField]
    private int powerupID;

    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("AudioManager null for Powerup");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
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
                    case 0:
                        player.EnableTripleShot();
                        break;
                    case 1:
                        player.EnableSpeedBoost();
                        break;
                    case 2:
                        player.EnableShield();
                        break;
                    default:
                        Debug.Log("Default value");
                        break;
                }


            }
            _audioManager.PlayPowerup();
            Destroy(gameObject);
        }
    }


}
