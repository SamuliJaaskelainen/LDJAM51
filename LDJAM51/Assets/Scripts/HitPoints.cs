using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoints : MonoBehaviour
{
    public static HitPoints Instance;

    public static int hitPoints = 3;
    public GameObject gameOver;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        hitPoints = 3;
        ShootCanvasManager.Instance.SetHP(hitPoints);
        Time.timeScale = 1.0f;
    }

    public void TakeDamage()
    {
        if (!gameOver.activeSelf)
        {
            hitPoints--;
            ShootCanvasManager.Instance.SetHP(hitPoints);
            Debug.LogFormat("Took damage, hp left: {0}", hitPoints);

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
    }
}
