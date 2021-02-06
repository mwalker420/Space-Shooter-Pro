using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [SerializeField]
    private Vector3 _targetPosition;

    
    [SerializeField]
    private int _maxHealth = 10;
    private int _health;

    [SerializeField]
    private GameObject _explosionPrefab;

    private HealthIndicator _healthIndicator;

    private void Start()
    {
        _health = _maxHealth;

        _healthIndicator = GetComponentInChildren<HealthIndicator>();
        if(_healthIndicator == null)
        {
            Debug.LogError("Boss HealthIndicator is null");
        }

        _healthIndicator.Health = (float)_health / _maxHealth;
    }

    private void Update()
    {
        if (transform.position != _targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime);
        }
    }

    public void Damage()
    {
        _health--;
        _healthIndicator.Health = (float)_health / _maxHealth;
        if (_health == 0)
        {
            GameManager.Instance.GameOver(GameManager.GameOverState.Win);
            StartCoroutine(DoDeath());
            Destroy(gameObject, 1.4f);
        }
    }

    IEnumerator DoDeath()
    {
        Instantiate(_explosionPrefab, transform);

        for (var i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-2f, 2f), 0f);
            var explosion = Instantiate(_explosionPrefab, pos, Quaternion.identity);
        }
    }



}
