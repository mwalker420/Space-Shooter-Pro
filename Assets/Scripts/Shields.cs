using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _shieldVisualizer;

    [SerializeField]
    private bool _isShieldActive = false;

    private UIManager _uiManager;

    public bool ShieldIsActive
    {
        get
        {
            return _isShieldActive;
        }
    }

    private enum ShieldStrength
    {
        high,
        med,
        low
    }

    [SerializeField]
    private ShieldStrength _shieldStrength;

    // Start is called before the first frame update
    void Start()
    {
        _shieldVisualizer = GetComponent<SpriteRenderer>();
        if (_shieldVisualizer != null)
        {
            _shieldVisualizer.enabled = false;
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

    }

    public void EnableShield()
    {
        _isShieldActive = true;
        _shieldStrength = ShieldStrength.high;
        if (_shieldVisualizer != null)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 0);
            _shieldVisualizer.enabled = true;
        }
        _uiManager.SetShieldStrength(1.0f);
        _uiManager.ShowShieldIndicator(true);
    }

    public void HandleShieldHit()
    {
        Vector3 shieldScale = new Vector3(2.5f, 2.5f, 0f);
        float indicatorValue = 1.0f;
        switch (_shieldStrength)
        {
            case ShieldStrength.high:
                _shieldStrength = ShieldStrength.med;
                shieldScale = new Vector3(2.0f, 2.0f, 0f);
                indicatorValue = 0.6f;
                break;
            case ShieldStrength.med:
                _shieldStrength = ShieldStrength.low;
                shieldScale = new Vector3(1.5f, 1.5f, 0f);
                indicatorValue = 0.3f;
                break;
            case ShieldStrength.low:
                _isShieldActive = false;
                _uiManager.ShowShieldIndicator(false);
                break;
        }

        if (_shieldVisualizer != null)
        {
            _shieldVisualizer.transform.localScale = shieldScale;
            _shieldVisualizer.enabled = _isShieldActive;
        }

        _uiManager.SetShieldStrength(indicatorValue);

    }


}


