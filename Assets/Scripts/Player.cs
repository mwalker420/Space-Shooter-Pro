using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    [SerializeField]
    private bool _isTripleShotActive = false;

    private bool _isSpeedBoostActive = false;
    private float _speedMultiplier = 2.0f;

    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    GameObject _rightEngineFlame;
    [SerializeField]
    GameObject _leftEngineFlame;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    //var to store audio clip
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _laserSoundClip;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        _leftEngineFlame.SetActive(false);
        _rightEngineFlame.SetActive(false);

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL.");
        }

    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        float effectiveSpeed = _speed;
        if (_isSpeedBoostActive)
        {
            effectiveSpeed *= _speedMultiplier;
        }

        transform.Translate(direction * effectiveSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.96f, 0), Quaternion.identity);
        }

    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            if (_shieldVisualizer != null)
            {
                _shieldVisualizer.SetActive(false);
            }
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _rightEngineFlame.SetActive(true);
        }

        if (_lives == 1)
        {
            _leftEngineFlame.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            if (_spawnManager != null)
            {
                _spawnManager.OnPlayerDeath();
            }

            Destroy(gameObject);
        }
    }

    public void EnableTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void EnableSpeedBoost()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }

    public void EnableShield()
    {

        _isShieldActive = true;
        if (_shieldVisualizer != null)
        {
            _shieldVisualizer.SetActive(true);
        }

    }

    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }

    

}
