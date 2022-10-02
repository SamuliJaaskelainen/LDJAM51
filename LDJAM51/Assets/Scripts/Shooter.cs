using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Shooter : MonoBehaviour {
    [FormerlySerializedAs("bullet")] public GameObject bulletPrefab;
    public                                  float      shootDelay          = 5.0f;
    public                                  float      warningTime         = 3.0f;
    public                                  float      flashTime           = 0.5f;
    public                                  float      flashRangeMultipier = 3.0f;
    private                                 float      timeUntilShoot;
    private                                 Light[]    lights;
    private                                 float[]    lightMaxIntensities;
    private                                 float[]    lightRanges;

    void Start() {
        timeUntilShoot = shootDelay + Random.Range(0.0f, shootDelay);
        lights = GetComponentsInChildren<Light>();
        lightMaxIntensities = new float[lights.Length];
        lightRanges = new float[lights.Length];
        for (int i = 0; i < lights.Length; i++) {
            lightMaxIntensities[i] = lights[i].intensity;
            lightRanges[i] = lights[i].range;
        }
        AdjustLights();
    }

    void Update() {
        timeUntilShoot -= Time.deltaTime;
        AdjustLights();
        if (timeUntilShoot < 0.0f) {
            Shoot();
            timeUntilShoot = shootDelay;
        }
    }

    private void Shoot() {
        var bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
    }

    private void AdjustLights() {
        float intensity = 0.0f;
        float range     = 1.0f;
        if(timeUntilShoot < warningTime) {
            if (timeUntilShoot < flashTime) {
                range = Mathf.SmoothStep(flashRangeMultipier, 1.0f, timeUntilShoot / flashTime);
                intensity = Mathf.SmoothStep(flashRangeMultipier, 1.0f, timeUntilShoot / flashTime);
            } else {
                intensity = Mathf.SmoothStep(1.0f, 0.0f, (timeUntilShoot - flashTime) / (warningTime - flashTime));
            }
        }
        
        for(int i = 0; i < lights.Length; ++i) {
            lights[i].intensity = intensity * lightMaxIntensities[i];
            lights[i].range = range * lightRanges[i];
        }
    }
}
