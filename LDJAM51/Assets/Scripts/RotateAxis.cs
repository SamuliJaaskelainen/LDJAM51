using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAxis : MonoBehaviour
{
    public Vector3 axis;
    public float speed;
    public Space space;

    void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime, space);
    }
}
