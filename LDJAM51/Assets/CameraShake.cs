using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [Range(0.0f, 1.0f)]
    public float intensity = 0.0f;

    [Range(0.0f, 100.0f)]
    public float speed = 10.0f;

    void Awake()
    {
        Instance = this;
    }

    public void Shake(float addIntensity = 1.0f)
    {
        intensity += addIntensity;
    }

    void Update()
    {
        intensity -= Time.unscaledDeltaTime;
        intensity = Mathf.Clamp01(intensity);

        Quaternion targetRotation = Random.rotationUniform;
        targetRotation = Quaternion.Lerp(Quaternion.identity, targetRotation, intensity);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.unscaledDeltaTime);
    }
}
