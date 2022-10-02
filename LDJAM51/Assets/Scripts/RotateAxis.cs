using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAxis : MonoBehaviour
{
    public Vector3 axis;
    public float speed;
    public Space space;
    public bool stoppable = true;

    void Update()
    {
        transform.Rotate(axis, speed * (stoppable ? Time.deltaTime : Time.unscaledDeltaTime), space);
    }
}
