using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using ManyMouseUnity;

public class ManyMouseHandler : MonoBehaviour
{
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
                Screen.SetResolution(Screen.width, Screen.height, false);
            }
        }

        if (useMouse)
        {
            Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                hit.transform.SendMessage("Hit");
            }
        }
    }

    void Shoot(int crosshairId)
    {
        Debug.Log("Firing on crosshair: " + crosshairId);

        Ray ray = gameCamera.ScreenPointToRay(crosshairs[crosshairId].GetScreenPosition());
        if (Physics.Raycast(ray, out hit))
        {
            hit.transform.SendMessage("Hit");
        }
    }

    void LockMouse()
    {
        Cursor.lockState = useMouse ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = false;
        Screen.SetResolution(Screen.width, Screen.height, true);
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
}