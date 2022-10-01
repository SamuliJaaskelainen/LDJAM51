using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ManyMouseUnity;

public class ManyMouseHandler : MonoBehaviour
{
    [SerializeField] bool useMouse = true;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject sceneMousePrefab;

    List<ManyMouseCrosshair> crosshairs;


    public List<ManyMouseCrosshair> Crosshairs { get { return crosshairs; } }

    private void OnEnable()
    {
        int numMice = ManyMouseWrapper.MouseCount;
        crosshairs = new List<ManyMouseCrosshair>();

        InitializeCrosshairs();

        if (!useMouse)
        {
            ManyMouseWrapper.OnInitialized += InitializeCrosshairs;
        }
    }

    private void OnDisable()
    {
        if (!useMouse)
        {
            ManyMouseWrapper.OnInitialized -= InitializeCrosshairs;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !useMouse)
        {
            ManyMouseWrapper.Instance.Reinitialize();
        }

        if (Cursor.visible)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = useMouse ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = false;
            Screen.SetResolution(Screen.width, Screen.height, true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Screen.SetResolution(Screen.width, Screen.height, false);
        }
    }

    private void InitializeCrosshairs()
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