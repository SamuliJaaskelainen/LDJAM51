using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameObject shop;
    public GameObject level;
    public GameObject playerUI;
    private Transform roomCameraParent;
    private Transform roomCamera;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            OpenShop();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CloseShop();
        }

        if (HitPoints.hitPoints > 0)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(1) && !Shop.shopOpen)
            {
                Time.timeScale = 10.0f;
            }
            else if (Input.GetKeyUp(KeyCode.F) || Input.GetMouseButtonUp(1))
            {
                Time.timeScale = Shop.shopOpen ? 0.0f : 1.0f;
            }
        }
        else
        {
            Time.timeScale = 0.0f;
        }
    }

    public void OpenShop()
    {
        shop.SetActive(true);
        playerUI.SetActive(false);

        // Hack: prevent room camera from being deactivated when shop is opened
        roomCamera = level.GetComponent<RoomManager>().currentRoom.blendListCamera.transform;
        roomCameraParent = roomCamera.parent;
        roomCamera.SetParent(null, true);

        level.SetActive(false);
    }

    public void CloseShop()
    {
        shop.SetActive(false);
        level.SetActive(true);
        playerUI.SetActive(true);

        // Hack: restore room camera
        if (roomCamera)
        {
            roomCamera.SetParent(roomCameraParent, true);
            roomCamera = null;
            roomCameraParent = null;
        }
    }
}
