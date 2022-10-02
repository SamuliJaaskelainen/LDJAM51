using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameObject shop;
    public GameObject level;

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
        if (Input.GetKeyDown(KeyCode.C)) {
            CloseShop();
        }
    }

    public void OpenShop()
    {
        shop.SetActive(true);
        level.SetActive(false);
    }

    public void CloseShop()
    {
        shop.SetActive(false);
        level.SetActive(true);
    }
}
