using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// shake inspired by https://medium.com/nice-things-ios-android-development/basic-2d-screen-shake-in-unity-9c27b56b516


public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private float _shakeDuration = 0.2f;
    [SerializeField]
    private float _currentShakeDuration;

    [SerializeField]
    private float _speed = 1.0f;

    [SerializeField]
    private float _shakeMagnitude = 0.1f;

    private Vector3 _initialPosition;




    void Start()
    {
        _initialPosition = transform.position;
    }

    void Update()
    {
        if (_currentShakeDuration > 0)
        {
            transform.localPosition = _initialPosition + Random.insideUnitSphere * _shakeMagnitude;

            _currentShakeDuration -= Time.deltaTime * _speed;
        }
        else
        {
            _currentShakeDuration = 0f;
            transform.localPosition = _initialPosition;
        }
    }

    public void TriggerShake()
    {
        _currentShakeDuration = _shakeDuration;
    }
}
