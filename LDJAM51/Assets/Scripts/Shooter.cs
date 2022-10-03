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

    bool isOnPlayersFrontSide() {
        Transform target    = HitPoints.Instance.transform;
        Vector3   direction = target.position - transform.position;

        if (Physics.Raycast(
                transform.position,
                direction,
                direction.magnitude,
                ~LayerMask.GetMask("NoBlockLineOfSight")
            )) {
            Debug.DrawLine(transform.position, target.position, Color.red);
            return false; 
        }
        Debug.DrawLine(transform.position, target.position, Color.green);
        return Vector3.Dot(direction.normalized, target.forward) < -0.5;
    }

    void Start() {
        if (RoomManager.Instance.difficulty < 0.2f) {
            shootDelay = 9999.0f;
        }
        shootDelay = Mathf.Lerp(7.0f, 3.0f, RoomManager.Instance.difficulty);
        
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
        if (!isOnPlayersFrontSide()) return;
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
