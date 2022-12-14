using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using ManyMouseUnity;

public class ManyMouseHandler : MonoBehaviour
{
    public static ManyMouseHandler Instance;

    public static bool useMouse = true;
    public static bool showCrosshair = true;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject sceneMousePrefab;
    [SerializeField] Camera gameCamera;
    RaycastHit hit;

    List<ManyMouseCrosshair> crosshairs = new List<ManyMouseCrosshair>();
    Dictionary<int, int> playerMouseIds = new Dictionary<int, int>();
    int player1MouseId;
    int player2MouseId;

    public List<ManyMouseCrosshair> Crosshairs { get { return crosshairs; } }

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        int numMice = ManyMouseWrapper.MouseCount;
        crosshairs = new List<ManyMouseCrosshair>();

        InitializeCrosshairs();

        if (!useMouse)
        {
            ManyMouseWrapper.OnInitialized += InitializeCrosshairs;
        }

        LockMouse();
    }

    void OnDisable()
    {
        if (!useMouse)
        {
            ManyMouseWrapper.OnInitialized -= InitializeCrosshairs;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !useMouse)
        {
            ManyMouseWrapper.Instance.Reinitialize();
        }

        if (Input.GetMouseButtonDown(0))
        {
            LockMouse();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Screen.SetResolution(Screen.width, Screen.height, useMouse ? Screen.fullScreen : false);
            }
        }
    }

    void LockMouse()
    {
        Cursor.lockState = useMouse ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = false;
        Screen.SetResolution(Screen.width, Screen.height, useMouse ? Screen.fullScreen : true);
    }

    void InitializeCrosshairs()
    {
        if (useMouse)
        {
            ManyMouseCrosshair newCrosshair = Instantiate(sceneMousePrefab, canvas.transform).GetComponent<ManyMouseCrosshair>();
            crosshairs.Add(newCrosshair);
            newCrosshair.Initialize(0, true);
        }
        else
        {
            for (int i = 0; i < ManyMouseWrapper.MouseCount; i++)
            {
                if (crosshairs.Count == i)
                {
                    ManyMouseCrosshair newCrosshair = Instantiate(sceneMousePrefab, canvas.transform).GetComponent<ManyMouseCrosshair>();
                    crosshairs.Add(newCrosshair);
                }
                crosshairs[i].Initialize(i, false);
            }
        }
    }

    public ManyMouseCrosshair GetCrosshairByMouseId(int mouseId)
    {
        for (int i = 0; i < crosshairs.Count; i++)
        {
            if (crosshairs[i].Mouse.ID == mouseId)
            {
                return crosshairs[i];
            }
        }
        return null;
    }
    public ManyMouseCrosshair GetCrosshairByPlayer(int player)
    {
        if (useMouse)
        {
            return crosshairs[0];
        }
        else
        {
            for (int i = 0; i < crosshairs.Count; i++)
            {
                int playerId = player == 1 ? ManyMouseCrosshair.player1mouseId : ManyMouseCrosshair.player2mouseId;
                if (crosshairs[i].Mouse.ID == playerId)
                {
                    return crosshairs[i];
                }
            }
        }
        return null;
    }
}