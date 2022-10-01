using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Room : MonoBehaviour {
    public float     spawnWeight = 1.0f;
    public Transform start { get; private set; }
    
    public CinemachineBlendListCamera blendListCamera { get; private set; }
    public Transform                  end   { get; private set; }
    
    public ICinemachineCamera endCamera   { get; private set; }

    void Awake() {
        blendListCamera = GetComponentInChildren<CinemachineBlendListCamera>();
        Debug.Assert(blendListCamera != null, "Room prefab " + name + " does not have a CinemachineBlendListCamera component");
        
        start = blendListCamera.transform.Find("start");
        Debug.Assert(start != null, "Room prefab " + name + " does not have a child called 'start'");

        end = blendListCamera.transform.Find("end");
        Debug.Assert(end != null, "Room prefab " + name + " does not have a child called 'end'");

        endCamera = end.GetComponent<ICinemachineCamera>();

        blendListCamera.enabled = false;
        blendListCamera.m_Loop  = false;
    }
}
