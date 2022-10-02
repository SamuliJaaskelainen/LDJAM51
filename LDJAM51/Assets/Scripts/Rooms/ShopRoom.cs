using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Room))]
public class ShopRoom : MonoBehaviour {
    [FormerlySerializedAs("cameraLeavingShop")] public CinemachineVirtualCamera shopCamera;
    private                                            Room                     room;
    private                                            bool                     enteredStore = false;

    private void Awake() {
        Debug.Assert(shopCamera != null, "cameraLeavingShop != null");
    }

    void Start() {
        room = GetComponent<Room>();
    }

    void Update() {
        if (!room.blendListCamera.IsBlending && room.blendListCamera.LiveChild == shopCamera && !enteredStore) {
            Debug.Log("Entering store");
            FindObjectOfType<GameStateManager>().OpenShop();
            enteredStore = true;
        }
    }
}
