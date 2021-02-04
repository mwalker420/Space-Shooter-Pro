using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;
    [SerializeField]
    private float _rotateSpeed = 200f;
    [SerializeField]
    private float _selfDestructTime = 5.0f;

    private Transform _target;
    private Rigidbody2D rb;

    private GameObject _spawnItemContainer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is NULL.");
        }

        _spawnItemContainer = GameObject.Find("Spawn_Item_Container");
        if (_spawnItemContainer == null)
        {
            Debug.LogError("Spawn_Item_Container is null for missile");
        }


        _target = AquireTargetTransform();
        Destroy(gameObject, _selfDestructTime);
    }


    void FixedUpdate()
    {
        if (_target != null)
        {
            // Inspiration from https://www.youtube.com/watch?v=0v_H3oOR0aU

            Vector2 direction = (Vector2)_target.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * _rotateSpeed;

        }
        else
        {
            _target = AquireTargetTransform();
        }

        rb.velocity = transform.up * _speed;

    }


    private Transform AquireTargetTransform()
    {
        //var target = GameObject.Find("Enemy(Clone)");
        //if (target != null)
        //{
        //    return target.transform;
        //}
        //return null;

        Component[] enemyList = _spawnItemContainer.GetComponentsInChildren<Enemy>();
        if (enemyList.Length == 0)
        {
            return null;
        }

        Component nearestEnemy = null;
        float minSqrDistance = Mathf.Infinity;
        foreach(var enemy in enemyList)
        {
            float sqrDistance = (transform.position - enemy.transform.position).sqrMagnitude;
            if(sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy.transform;


    }
}
