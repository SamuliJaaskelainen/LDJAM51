using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameObject shop;
    public GameObject level;
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 10.0f;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            Time.timeScale = Shop.shopOpen ? 0.0f : 1.0f;
        }
    }

    public void OpenShop()
    {
        shop.SetActive(true);

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

        // Hack: restore room camera
        if (roomCamera)
        {
            roomCamera.SetParent(roomCameraParent, true);
            roomCamera = null;
            roomCameraParent = null;
        }
    }
}
