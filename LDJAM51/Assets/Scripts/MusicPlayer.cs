using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip[] clips;
    public AudioSource _audioSource;

    void Start()
    {
        _audioSource.loop = false;
        _audioSource.clip = GetRandomClip();
        _audioSource.Play();
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = GetRandomClip();
            _audioSource.Play();
        } 
        
    }
}
