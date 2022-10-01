using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour {
    public  GameObject[] roomGameObjects;
    private Room[]       rooms;
    private float[]      cumulativeRoomWeights;
    private Room         currentRoom;
    private Room         nextRoom;
    
    void Start()
    {
        Debug.Assert(roomGameObjects.Length > 0, "No rooms in RoomManager");
        rooms = new Room[roomGameObjects.Length];
        cumulativeRoomWeights = new float[roomGameObjects.Length];

        for (int i = 0; i < roomGameObjects.Length; i++)
        {
            rooms[i] = roomGameObjects[i].GetComponent<Room>();
            Debug.Assert(rooms[i] != null, "Room prefab " + roomGameObjects[i].name + " does not have a Room component");
            cumulativeRoomWeights[i] = rooms[i].spawnWeight + (i > 0 ? cumulativeRoomWeights[i - 1] : 0);
        }
        Debug.Assert(cumulativeRoomWeights[cumulativeRoomWeights.Length - 1] > 0, "No rooms have a spawn weight greater than 0");
        currentRoom = SpawnRoom(transform.position, transform.rotation);
        nextRoom = SpawnRoom(currentRoom.end.position, currentRoom.end.rotation);

        currentRoom.blendListCamera.enabled = true;
    }

    private void Update() {
        if (!currentRoom.blendListCamera.IsBlending && currentRoom.blendListCamera.LiveChild == currentRoom.endCamera) {
            NextRoom();
        }
    }

    private void NextRoom() {
        Destroy(currentRoom.gameObject);
        currentRoom                         = nextRoom;
        nextRoom                            = SpawnRoom(currentRoom.end.position, currentRoom.end.rotation);
        currentRoom.blendListCamera.enabled = true;
    }

    private Room SpawnRoom(Vector3 startPosition, Quaternion startRotation) {
        float randomWeight = Random.Range(0.0f, cumulativeRoomWeights[cumulativeRoomWeights.Length - 1]);
        Room room = rooms[0];
        for (int i = 1; i < rooms.Length; ++i) {
            if(randomWeight >= cumulativeRoomWeights[i - 1]) {
                room = rooms[i];
                break;
            }
        }
        room = Instantiate(room, transform);
        
        room.transform.position -= room.start.position;
        room.transform.position += startPosition;
        float angle;
        Vector3 axis;
        (startRotation * Quaternion.Inverse(room.start.rotation)).ToAngleAxis(out angle, out axis);
        room.transform.RotateAround(startPosition, axis, angle);
        return room;
    }
}
