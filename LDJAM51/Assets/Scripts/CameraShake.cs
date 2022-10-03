using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [Range(0.0f, 1.0f)]
    public float intensity = 0.0f;

    public float intensityMultiplier = 1.0f;

    [Range(0.0f, 100.0f)]
    public float speed = 10.0f;

    [Range(0.0f, 100.0f)]
    public float addMultiplier = 10.0f;

    public NoiseSettings noiseSettings;

    void Awake()
    {
        Instance = this;
    }

    public void Shake(float addIntensity = 1.0f)
    {
        intensity += addIntensity * addMultiplier;
    }

    void Update()
    {
        intensity -= Time.unscaledDeltaTime;
        intensity = Mathf.Clamp01(intensity);

        Quaternion targetRotation = Random.rotationUniform;
        targetRotation = Quaternion.Lerp(Quaternion.identity, targetRotation, intensity);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.unscaledDeltaTime);

        var cameras = RoomManager.Instance.currentRoom.blendListCamera.GetComponentsInChildren<CinemachineVirtualCamera>();
        // Add screen shake to each virtual camera
        foreach (var camera in cameras)
        {
            // Make sure camera has a noise profile
            var component = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (component == null)
            {
                component = camera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                component.m_NoiseProfile = noiseSettings;
            }
            component.m_AmplitudeGain = intensityMultiplier * intensity;
            component.m_FrequencyGain = speed;
        }
    }
}
