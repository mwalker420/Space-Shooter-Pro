using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    [SerializeField, Range(0f,1.0f)]
    private float _health = 1.0f;

    private Material _material;

    private float _redThreshold = 0.3f;
    private float _yellowThreshold = 0.6f;

    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if(_health < _redThreshold)
            {
                _material.color = Color.red;
            }
            else if (_health < _yellowThreshold)
            {
                _material.color = Color.yellow;
            } else
            {
                _material.color = Color.green;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _material = gameObject.GetComponent<SpriteRenderer>()?.material;
        if(_material == null)
        {
            Debug.LogError("HealthIndicator material is null");
        }

        Health = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
