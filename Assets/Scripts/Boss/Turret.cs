using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    // aim laser turret at enemy and fire
    // take damage only when turret is hit

    [SerializeField]
    private float _firingPeriod = 1.5f;


    [SerializeField]
    private Transform _aimTarget;

    [SerializeField]
    private float _aimSpeed = 4.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private Boss _boss;

    void Start()
    {
        _aimTarget = GameObject.Find("Player").transform;
        if (_aimTarget == null)
        {
            Debug.LogError("Turret aim target is null");
        }

        Boss _boss = GetComponentInParent<Boss>();
        if(_boss == null)
        {
            Debug.LogError("Turrent cannot find parent Boss component");
        }
        StartCoroutine(FireLasers());
    }

    void Update()
    {
        if (_aimTarget == null)
        {
            return;
        }

        // inspired by https://www.youtube.com/watch?v=mKLp-2iseDc

        Vector2 direction = _aimTarget.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _aimSpeed * Time.deltaTime);
    }

    IEnumerator FireLasers()
    {
        while (true)
        {
            yield return new WaitForSeconds(_firingPeriod);

            //correct the rotation because laser prefab was initially made to go the other way
            Quaternion correctedRotation = transform.rotation * Quaternion.AngleAxis(180, transform.forward);
            GameObject laser = Instantiate(_laserPrefab, transform.position, correctedRotation);

            Destroy(laser, 1.0f); //self destruct laser if it doesn't hit anything
            
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _boss.Damage();
        }
    }
}
