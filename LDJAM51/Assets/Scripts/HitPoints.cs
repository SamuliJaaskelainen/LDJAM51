using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class HitPoints : MonoBehaviour
{
    public static HitPoints Instance;

    public static int hitPoints = 3;
    public GameObject gameOver;

    public Volume volume;
    ChromaticAberration chromaticAberration;
    Vignette vignette;

    float damageEffectValue = 0.0f;
    float refreshEffectValue = 0.0f;

    public TextMeshProUGUI score;

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
                score.text = RoomManager.Instance.pupusMurdered + " pupus murdered";
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
            vignette.color.value = Color.red;
        }

        if (refreshEffectValue > 0.0f)
        {
            refreshEffectValue -= Time.deltaTime * 0.4f;
            if (refreshEffectValue < 0.0f)
            {
                refreshEffectValue = 0.0f;
            }
            chromaticAberration.intensity.value = refreshEffectValue;
            vignette.intensity.value = refreshEffectValue;
            vignette.color.value = Color.magenta;
        }
    }

    public void Refresh()
    {
        refreshEffectValue = 0.8f;
    }
}
