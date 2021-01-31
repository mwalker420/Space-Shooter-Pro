using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPBlast : MonoBehaviour
{
    private bool _doExpand = false;

    [SerializeField]
    private float _animatedScaleFactor = 1.05f;


    void Start()
    {
        Destroy(gameObject, 2.0f);
    }


    void FixedUpdate()
    {
        if (_doExpand)
        {
            transform.localScale *= _animatedScaleFactor;
        }
    }
    public void StartExpanding()
    {
        _doExpand = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
