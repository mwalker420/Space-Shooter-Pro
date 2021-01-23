using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField]
    private GameObject _indicatorForgroundObject;

    [SerializeField]
    private GameObject _indicatorBackgroundObject;

    private RectTransform _rectTransform;

    private float _percentage;
    public float Percentage
    {
        get { return _percentage; }
        set
        {
            _percentage = Mathf.Clamp(value, 0, 1.0f);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100 * _percentage);

        }
    }

    private void Start()
    {
        _rectTransform = _indicatorForgroundObject.GetComponent<RectTransform>();
    }

    public void ShowIndicator(bool show)
    {
        _indicatorBackgroundObject.SetActive(show);
        _indicatorForgroundObject.SetActive(show);
    }

}
