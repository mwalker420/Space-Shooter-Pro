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

    #region Weapons
    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _empBlastPrefab;
    public bool HasEMPWeapon;


    #endregion Weapons

    #region Advanced Movement Behavior
    [SerializeField]
    private bool _useAdvancedMovement = false;
    [SerializeField]
    private Vector3 _advancedDirection = new Vector3(1, -1, 0);
    #endregion Advanced Movement Behavior

    #region Shield Behavior
    [SerializeField]
    private GameObject _enemyShieldPrefab;
    [SerializeField]
    private bool _shieldIsEnabled;
    #endregion Shield Behavior

    #region Ramming Behavior
    [SerializeField]
    private bool _rammingIsEnabled = false;
    [SerializeField]
    private float _rammingSpeed = 4.0f;
    [SerializeField]
    private float _rammingDetectionDistance = 4.5f;
    private bool _rammingInProgress = false;
    #endregion Ramming Behavior

    #region Smart Enemy
    [SerializeField]
    private bool _isSmartEnemy = false;
    [SerializeField]
    private GameObject _rearFiringEnemyLaser;
    private bool _rearLaserFired = false;
    #endregion Smart Enemy

    UIManager _uiManager;

    private Rigidbody2D _rb;

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


        _enemyShieldPrefab.SetActive(_shieldIsEnabled);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UIManager is NULL");
        }


    }

    IEnumerator FireLasers()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            if (HasEMPWeapon)
            {
                var blastRef = Instantiate(_empBlastPrefab, transform.position, Quaternion.identity);
                blastRef.transform.parent = transform;
            }
            else
            {
                Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    void Update()
    {
        // handle ramming
        if (_rammingIsEnabled && !_rammingInProgress)
        {
            _rammingInProgress = CheckForRamming();
        }

        bool playerIsBehind = CheckForPlayerBehind();
        if (_isSmartEnemy && playerIsBehind)
        {
            FireRearWeapon();
        }


        // process movement
        if (_rammingInProgress)
        {
            RammingMovement();
        }
        else if (_useAdvancedMovement)
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

    private void FireRearWeapon()
    {
        if (!_rearLaserFired)
        {
            Debug.Log("FireRearWeapon");
            Instantiate(_rearFiringEnemyLaser, transform.position, Quaternion.identity);
            _rearLaserFired = true;
            StartCoroutine(ResetRearFiringLaser());

        }

    }
    IEnumerator ResetRearFiringLaser()
    {
        yield return new WaitForSeconds(3.0f);
        _rearLaserFired = false;
    }

    private bool CheckForPlayerBehind()
    {
        if (_player != null)
        {
            Vector3 relativePosition = _player.transform.position - transform.position;

            if (relativePosition.x < 1.0f && relativePosition.x > -1.0f && relativePosition.y > 0)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckForRamming()
    {
        if (!_rammingIsEnabled)
        {
            return false;
        }

        // get distance to player
        Vector3 distanceToPlayer = _player.transform.position - transform.position;

        // if distance is within ramming distance
        if (distanceToPlayer.magnitude <= _rammingDetectionDistance)
        {
            return true;
        }

        return false;
    }

    private void RammingMovement()
    {
        // get distance to player
        Vector3 relativePosition = _player.transform.position - transform.position;

        // if distance is within ramming distance
        if (relativePosition.magnitude <= _rammingDetectionDistance)
        {
            // then move toward player at ramming speed.
            relativePosition.Normalize(); //only care about the direction now
            transform.Translate(relativePosition * _rammingSpeed * Time.deltaTime);
        }
        else
        {
            _rammingInProgress = false;

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
        if (other.tag == "Player" && _rammingInProgress)
        {
            _rammingIsEnabled = false;
            _rammingInProgress = false;
        }

        if ((other.tag == "Player" || other.tag == "Laser") && _shieldIsEnabled)
        {
            _shieldIsEnabled = false;
            _enemyShieldPrefab.SetActive(false);
            if (_player != null)
            {
                _player.Damage();
            }
            return;
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            DoDeath();
        }

        if (other.tag == "Laser")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }
            Destroy(GetComponent<Collider2D>());

            Destroy(other.gameObject);
            DoDeath();
        }
    }

    private void DoDeath()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _audioManager.PlayExplosion();
        StopAllCoroutines(); //don't want enemy to get any shots off if destroyed
        Destroy(gameObject, 2.8f);
    }

    private void OnDrawGizmos()
    {
        if (_rammingIsEnabled)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _rammingDetectionDistance);
        }
    }
}
