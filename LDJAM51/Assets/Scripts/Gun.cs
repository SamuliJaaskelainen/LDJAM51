using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Mesh shotMesh;
    [SerializeField] Bullet[] bullets;
    [SerializeField] Vector3[] bulletOffsets;
    [SerializeField] float shake = 0.08f;
    public int ownerId;

    public void Shoot(Vector3 position, Vector3 forward)
    {
        for (int i = 0; i < bullets.Length; ++i)
        {
            Bullet bullet = Instantiate(bullets[i], null);
            bullet.transform.position = position;
            bullet.transform.forward = forward;
            bullet.transform.localPosition += bulletOffsets[i];
            bullet.Init(ownerId);
        }

        CameraShake.Instance.Shake(shake);
    }
}
