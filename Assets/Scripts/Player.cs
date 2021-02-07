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
    private GameObject _missilePrefab;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    #region Lives
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
    #endregion Lives

    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _isMissileActive = false;

    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private float _speedBoostMultiplier = 2.0f;

    [SerializeField]
    private bool _useSlowDown = false;

    [SerializeField]
    private float _slowDownFactor = 0.5f;

    [SerializeField]
    private Shields _shields;

    [SerializeField]
    GameObject _rightEngineFlame;
    [SerializeField]
    GameObject _leftEngineFlame;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    #region Audio

    //var to store audio clip
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _laserSoundClip;

    [SerializeField]
    private AudioClip _failedLaserClip;

    #endregion Audio

    [SerializeField]
    private GameObject _explosionPrefab;

    #region Ammo
    [SerializeField]
    private int _maxAmmoCount = 15;
    [SerializeField]
    private bool _replenishAmmoOnEnableMissiles;
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
    #endregion Ammo

    #region Thrusters
    [SerializeField]
    private int _maxThrusterValue = 60;
    private int _currentThrusterValue;
    private int CurrentThrusterValue
    {
        get { return _currentThrusterValue; }
        set
        {
            if (value >= 0 && value <= _maxThrusterValue)
            {
                _currentThrusterValue = value;
                _uiManager.SetThrusterIndicator(_currentThrusterValue, _maxThrusterValue);
            }
        }
    }

    [SerializeField]
    private float _thrusterReplenishStartDelay = 2.0f;
    [SerializeField]
    private float _thrusterReplenishRate = 0.3f;
    private bool _thrusterReplenishIsActive = false;

    #endregion Thrusters

    [SerializeField]
    private MainCamera _mainCamera;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);


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
        CurrentThrusterValue = _maxThrusterValue;


        _mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        if (_mainCamera == null)
        {
            Debug.LogError("Main Camera reference is NULL");
        }



    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
        ReplenishThrusters();
    }

    void ReplenishThrusters()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_thrusterReplenishIsActive)
            {
                _thrusterReplenishIsActive = false;
                StopCoroutine("ReplenishThrustersRoutine");
            }

        }
        else if (!_thrusterReplenishIsActive)
        {
            _thrusterReplenishIsActive = true;
            StartCoroutine("ReplenishThrustersRoutine");
        }
    }

    IEnumerator ReplenishThrustersRoutine()
    {
        yield return new WaitForSeconds(_thrusterReplenishStartDelay);
        while (true)
        {
            yield return new WaitForSeconds(_thrusterReplenishRate);
            CurrentThrusterValue++;
            if (CurrentThrusterValue >= _maxThrusterValue)
            {
                break;
            }
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        float effectiveSpeed = _speed;

        if (_useSlowDown)
        {
            effectiveSpeed *= _slowDownFactor;
        }

        if (Input.GetKey(KeyCode.LeftShift) && CurrentThrusterValue > 0)
        {
            effectiveSpeed += _thrusterSpeedIncreaseForLeftShift;
            CurrentThrusterValue--;
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
            if (_isMissileActive)
            {
                Instantiate(_missilePrefab, transform.position, Quaternion.identity);
            }
            else if (_isTripleShotActive)
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
        _mainCamera.TriggerShake();

        if (Lives < 1)
        {

            GameManager.Instance.GameOver();

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 1.0f);
        }
    }

    public void EnableTripleShot()
    {
        //Debug.Log("Player::EnableTripleShot");
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
        //Debug.Log("Player::EnableSpeedBoost");
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
        //Debug.Log("Player::EnableShield");
        _shields.EnableShield();
    }

    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }

    public void ReplenishAmmo()
    {
        //Debug.Log("Player::ReplenishAmmo");
        CurrentAmmoCount = _maxAmmoCount;
    }

    public void IncreaseHealth()
    {
        Lives++;
    }

    public void EnableMissiles()
    {
        //Debug.Log("Player::Enable Missiles");
        _isMissileActive = true;
        StartCoroutine(MissilePowerDown());
        if (_replenishAmmoOnEnableMissiles)
        {
            ReplenishAmmo();
        }

    }

    IEnumerator MissilePowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isMissileActive = false;
    }

    public void SlowDown()
    {
        _useSlowDown = true;
        StartCoroutine(SlowDownRoutine());
    }

    IEnumerator SlowDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _useSlowDown = false;
    }


}
