using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public float spread = 1.0f;

    public override void Shoot(Vector3 position, Vector3 forward)
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
            bullet.transform.localPosition += new Vector3(Random.insideUnitCircle.x * spread, Random.insideUnitCircle.y * spread, 0.0f);
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
