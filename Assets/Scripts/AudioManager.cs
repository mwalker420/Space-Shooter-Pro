using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource _audioSource;

    [SerializeField]
    private AudioClip _explosionAudio;

    [SerializeField]
    private AudioClip _powerupAudio;

    [SerializeField]
    private AudioClip _powerDownAudio;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is NULL for AudioManager");
        }
    }

    public void PlayExplosion()
    {
        _audioSource.PlayOneShot(_explosionAudio);
    }

    public void PlayPowerup()
    {
        _audioSource.PlayOneShot(_powerupAudio);
    }

    public void PlayPowerDown()
    {
        _audioSource.PlayOneShot(_powerDownAudio);
    }


}
