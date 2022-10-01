using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootButton : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] string functionName;
    [SerializeField] string functionParam;

    // Update is called once per frame
    public void Press()
    {
        Debug.Log(functionName + ", " + functionParam);
        target.SendMessage(functionName, functionParam);
    }
}
