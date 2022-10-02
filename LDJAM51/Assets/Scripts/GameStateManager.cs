using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public GameObject shop;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            shop.SetActive(true);
        }
    }
}
