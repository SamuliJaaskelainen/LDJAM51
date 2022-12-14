using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXCollider : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource audioSource;

    float timer;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {

        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}