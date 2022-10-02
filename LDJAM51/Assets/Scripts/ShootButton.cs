using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootButton : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] string functionName;
    [SerializeField] string functionParam;
    [SerializeField] int requiredPlayer = -1;

    // Update is called once per frame
    public void Press(int shooterId)
    {
        Debug.Log("Press: " + shooterId);

        if (requiredPlayer < 0
            || shooterId == ManyMouseCrosshair.player1mouseId && requiredPlayer == 1
            || shooterId == ManyMouseCrosshair.player2mouseId && requiredPlayer == 2)
        {
            Debug.Log(functionName + ", " + functionParam);
            target.SendMessage(functionName, functionParam);
        }
    }
}
