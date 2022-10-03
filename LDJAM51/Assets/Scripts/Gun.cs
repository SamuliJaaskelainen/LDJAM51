using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Mesh shotMesh;
    public Bullet shopBullet;
    public Bullet[] bullets;
    public Vector3[] bulletOffsets;
    public float shake = 0.08f;
    public int ownerId;
    
    public AudioClip[] clips;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public virtual void Shoot(Vector3 position, Vector3 forward)
    {
        for (int i = 0; i < bullets.Length; ++i)
        {
            Bullet bullet;
            if (Shop.shopOpen)
            {
                bullet = Instantiate(shopBullet, null);
            }
            else
            {
                bullet = Instantiate(bullets[i], null);
            }
            
            bullet.transform.position = position;
            bullet.transform.forward = forward;
            bullet.transform.localPosition += bulletOffsets[i];
            bullet.Init(ownerId);   

            if (Shop.shopOpen)
            {
                return;
            }
        }

        CameraShake.Instance.Shake(shake);
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}
