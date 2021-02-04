using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceDetector : MonoBehaviour
{
    private Enemy _enemy;
    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        if (_enemy == null)
        {
            Debug.LogError("AvoidanceDetector expects to be associated with an enemy");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _enemy.EvadeObject();

        }
    }
}