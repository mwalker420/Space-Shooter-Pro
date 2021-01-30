using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;

    private Animator _anim;

    private AudioManager _audioManager;

    [SerializeField]
    private GameObject _laserPrefab;

    public bool useAdvancedMovement = false;
    [SerializeField]
    private Vector3 _advancedDirection = new Vector3(1, -1, 0);


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("Animator is NULL.");
        }

        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("AudioSource null for Enemy");
        }

        StartCoroutine(FireLasers());

    }

    IEnumerator FireLasers()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        }
    }

    void Update()
    {
        if (useAdvancedMovement)
        {
            AdvancedMovement();
        }
        else
        {
            RegularMovement();
        }


        // come back in at a random spot at the top
        // if enemy made it to the bottom
        if (transform.position.y < -6.0f)
        {
            float randomX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomX, 8f, 0);
        }

    }

    private void RegularMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void AdvancedMovement()
    {
        // move side to side

        if (transform.position.x > 9f)
        {
            _advancedDirection.x *= -1f;
        }
        else if (transform.position.x < -9f)
        {
            _advancedDirection.x *= -1f;
        }

        transform.Translate(_advancedDirection * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioManager.PlayExplosion();
            StopAllCoroutines(); //don't want enemy to get any shots off if destroyed
            Destroy(gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioManager.PlayExplosion();
            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
            StopAllCoroutines(); //don't want enemy to get any shots off if destroyed
            Destroy(gameObject, 2.8f);
        }
    }
}
