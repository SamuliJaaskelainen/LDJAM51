using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HitPoints : MonoBehaviour
{
    public static HitPoints Instance;

    public static int hitPoints = 3;
    public GameObject gameOver;

    public Volume volume;
    ChromaticAberration chromaticAberration;
    Vignette vignette;

    float damageEffectValue = 0.0f;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        hitPoints = 3;
        ShootCanvasManager.Instance.SetHP(hitPoints);
        Time.timeScale = 1.0f;

        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out vignette);
    }

    public void TakeDamage()
    {
        if (!gameOver.activeSelf)
        {
            hitPoints--;
            ShootCanvasManager.Instance.SetHP(hitPoints);
            Debug.LogFormat("Took damage, hp left: {0}", hitPoints);

            damageEffectValue = 1.0f;
            CameraShake.Instance.Shake(0.15f);

            if (hitPoints <= 0)
            {
                Time.timeScale = 0.0f;
                gameOver.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage();
        }

        if (damageEffectValue > 0.0f)
        {
            damageEffectValue -= Time.deltaTime * 0.33f;
            if (damageEffectValue < 0.0f)
            {
                damageEffectValue = 0.0f;
            }
            chromaticAberration.intensity.value = damageEffectValue;
            vignette.intensity.value = damageEffectValue;
        }
    }
}
