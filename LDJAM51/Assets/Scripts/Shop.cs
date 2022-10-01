using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    bool p1skip = false;
    bool p2skip = false;

    void OnEnable()
    {
        p1skip = false;
        p2skip = ManyMouseCrosshair.playerCount <= 1;
    }

    void OnDisable()
    {

    }

    public void Skip(string player)
    {
        if (player == "1")
        {
            p1skip = true;
        }
        else if (player == "2")
        {
            p2skip = true;
        }

        if (p1skip && p2skip)
        {
            gameObject.SetActive(false);
        }
    }
}
