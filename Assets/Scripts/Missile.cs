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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is NULL.");
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
        var target = GameObject.Find("Enemy(Clone)");
        if (target != null)
        {
            return target.transform;
        }
        return null;
    }
}
