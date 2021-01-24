using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6.0f;
    [SerializeField]
    private float _thrusterSpeedIncreaseForLeftShift = 4.0f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    private int _maxLives = 3; //
    [SerializeField]
    private int _lives = 3;
    private int Lives
    {
        get { return _lives; }
        set
        {
            _lives = Mathf.Min(value, _maxLives);
            _uiManager.UpdateLives(_lives);
            RenderShipHealth();
        }
    }

    private SpawnManager _spawnManager;

    [SerializeField]
    private bool _isTripleShotActive = false;

    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private float _speedBoostMultiplier = 2.0f;

    [SerializeField]
    private Shields _shields;

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

    [SerializeField]
    private AudioClip _failedLaserClip;

    [SerializeField]
    private int _maxAmmoCount = 15;
    private int _currentAmmoCount;
    private int CurrentAmmoCount
    {
        get
        {
            return _currentAmmoCount;
        }
        set
        {
            _currentAmmoCount = value;
            _uiManager.SetAmmoIndicator(_currentAmmoCount, _maxAmmoCount);
        }
    }

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

        if (_shields == null)
        {
            Debug.LogError("Hey, don't forget to assign the shields in the Inspector");
        }

        CurrentAmmoCount = _maxAmmoCount;


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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            effectiveSpeed += _thrusterSpeedIncreaseForLeftShift;
        }

        if (_isSpeedBoostActive)
        {
            effectiveSpeed *= _speedBoostMultiplier;
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

        if (CurrentAmmoCount > 0)
        {
            if (_isTripleShotActive)
            {
                Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.96f, 0), Quaternion.identity);
            }
            CurrentAmmoCount--; // triple shot only counts one against ammo count.
            _audioSource.PlayOneShot(_laserSoundClip);

        }
        else
        {
            Debug.Log("Out of ammo");
            _audioSource.PlayOneShot(_failedLaserClip);
        }


    }

    private void RenderShipHealth()
    {
        _rightEngineFlame.SetActive(false);
        _leftEngineFlame.SetActive(false);

        if (Lives <= 2)
        {
            _rightEngineFlame.SetActive(true);
        }

        if (Lives == 1)
        {
            _leftEngineFlame.SetActive(true);
        }
    }

    public void Damage()
    {
        if (_shields.ShieldIsActive)
        {
            _shields.HandleShieldHit();
            return;
        }

        Lives--;

        if (Lives < 1)
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
        _shields.EnableShield();
    }

    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }

    public void ReplenishAmmo()
    {
        CurrentAmmoCount = _maxAmmoCount;
    }

    public void IncreaseHealth()
    {
        Lives++;
    }



}
