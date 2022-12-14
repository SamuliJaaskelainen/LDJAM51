using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootCanvasManager : MonoBehaviour
{
    public static ShootCanvasManager Instance;

    public static float borderPixelSize = 0.0f;

    [SerializeField] CanvasScaler mainCanvasScaler;
    [SerializeField] RectTransform gameWindow;
    [SerializeField] RawImage gameImage;

    [SerializeField] List<MeshFilter> p1guns = new List<MeshFilter>();
    [SerializeField] List<MeshFilter> p2guns = new List<MeshFilter>();

    [SerializeField] List<GameObject> hearts = new List<GameObject>();

    [SerializeField] Transform timerArrow;

    RenderTexture gameRenderTexture;

    private void Awake()
    {
        Instance = this;
        SetPlayerTwo(false);
    }

    void Start()
    {
        Debug.LogFormat("Resolution ({0}, {1})", Screen.width, Screen.height);
        mainCanvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        gameRenderTexture = new RenderTexture(Screen.width, Screen.height, 32);
        Camera.main.targetTexture = gameRenderTexture;
        gameImage.texture = gameRenderTexture;

        Debug.LogFormat("RenderTexture resolution ({0}, {1})", gameRenderTexture.width, gameRenderTexture.height);
        UpdateBorder();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }

    void UpdateBorder()
    {
        borderPixelSize = Mathf.Clamp(borderPixelSize, 0.0f, Screen.height / 2.0f);
        int actualPixelSize = (int)Mathf.Floor(borderPixelSize);
        gameWindow.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        gameWindow.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        gameWindow.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, actualPixelSize, gameWindow.rect.height - actualPixelSize);
        gameWindow.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, actualPixelSize, gameWindow.rect.width - actualPixelSize);
        gameWindow.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, actualPixelSize, gameWindow.rect.width - actualPixelSize);
        gameWindow.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, actualPixelSize, gameWindow.rect.height - actualPixelSize);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.PageUp))
        {
            borderPixelSize += 10.0f * Time.unscaledDeltaTime;
            UpdateBorder();
        }
        else if (Input.GetKey(KeyCode.PageDown))
        {
            borderPixelSize -= 10.0f * Time.unscaledDeltaTime;
            UpdateBorder();
        }
    }

    public void SetGun(int player, int gunSlot, Mesh newMesh)
    {
        if (player == 1)
        {
            p1guns[gunSlot].mesh = newMesh;
        }
        else
        {
            p2guns[gunSlot].mesh = newMesh;
        }
    }

    public void SetGunActive(int player, int gunSlot, bool active)
    {
        if (player == 1)
        {
            p1guns[gunSlot].gameObject.SetActive(active);
        }
        else
        {
            p2guns[gunSlot].gameObject.SetActive(active);
        }
    }

    public void SetHP(int hp)
    {
        hearts[0].SetActive(hp >= 3);
        hearts[1].SetActive(hp >= 2);
        hearts[2].SetActive(hp >= 1);
    }

    public void SetTimerArrow(float progress)
    {
        timerArrow.localEulerAngles = new Vector3(0.0f, 0.0f, 360.0f - 360.0f * progress);
    }

    public void SetPlayerTwo(bool enabled)
    {
        for (int i = 0; i < p2guns.Count; ++i)
        {
            p2guns[i].gameObject.SetActive(enabled);
        }
    }
}
