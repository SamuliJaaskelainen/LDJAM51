using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithResolution : MonoBehaviour
{
    public float referencePixelsX = 3840.0f;
    void Update()
    {
        float scale = Screen.width / referencePixelsX; 
        transform.localScale = new Vector3(scale, scale, scale);
    }
}