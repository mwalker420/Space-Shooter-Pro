using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectAndDestroyPickup : MonoBehaviour
{
    // Start is called before the first frame update
    private Enemy _enemy;
    private bool _laserFired;

    [SerializeField]
    private float _laserResetTime = 1.0f;

    private Rigidbody2D _rb;

    void Start()
    {
        _enemy = gameObject.GetComponent<Enemy>();
        if (_enemy == null)
        {
            Debug.LogError("Enemy is NULL. The DetectAndDestroyPickup script depends on an accompanying Enemy script on the game object");
        }

        _rb = gameObject.GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("Rigid body is NULL");
        }
    }

    void FixedUpdate()
    {
        if (!_laserFired && PowerupDetected())
        {
            _enemy.FireLaser();
            _laserFired = true;
            StartCoroutine(ResetLaser());
        }
    }

    IEnumerator ResetLaser()
    {
        yield return new WaitForSeconds(_laserResetTime);
        _laserFired = true;
    }

    private bool PowerupDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Powerup")
            {
                return true;
            }

        }
        return false;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
