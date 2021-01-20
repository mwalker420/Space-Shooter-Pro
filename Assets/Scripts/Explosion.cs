using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionClip;

    void Start()
    {
        Destroy(gameObject, 3.0f);
    }

}
