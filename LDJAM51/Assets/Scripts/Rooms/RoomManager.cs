using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

class RoomCollection {
    private Room[] rooms;
    private float[]      cumulativeRoomWeights;
    public RoomCollection(GameObject[] roomGameObjects) {
        Debug.Assert(roomGameObjects.Length > 0);
        
        rooms = new Room[roomGameObjects.Length];
        cumulativeRoomWeights = new float[roomGameObjects.Length];
        for (int i = 0; i < roomGameObjects.Length; i++)
        {
            rooms[i] = roomGameObjects[i].GetComponent<Room>();
            Debug.Assert(rooms[i] != null, "Room prefab " + roomGameObjects[i].name + " does not have a Room component");
            cumulativeRoomWeights[i] = rooms[i].spawnWeight + (i > 0 ? cumulativeRoomWeights[i - 1] : 0);
        }
        Debug.Assert(cumulativeRoomWeights[cumulativeRoomWeights.Length - 1] > 0, "No rooms have a spawn weight greater than 0");
    }
    
    public Room SpawnRoom(Vector3 startPosition, Quaternion startRotation, Transform parent, float difficulty) {
        float randomWeight = Random.Range(0.0f, cumulativeRoomWeights[cumulativeRoomWeights.Length - 1]);
        Room  room         = rooms[cumulativeRoomWeights.Length - 1];
        for (int i = 0; i < rooms.Length - 1; ++i) {
            if(randomWeight < cumulativeRoomWeights[i]) {
                room = rooms[i];
                break;
            }
        }
        room = GameObject.Instantiate(room, parent);
        Debug.LogFormat("Spawning room {0} with difficulty {1}", room.name, difficulty);
        
        room.transform.position -= room.start.position;
        room.transform.position += startPosition;
        float   angle;
        Vector3 axis;
        (startRotation * Quaternion.Inverse(room.start.rotation)).ToAngleAxis(out angle, out axis);
        room.transform.RotateAround(startPosition, axis, angle);

        SpawnGroup.Spawn(room.transform, difficulty);
        return room;
    }
}

public class RoomManager : MonoBehaviour {
    public                                           bool           debug = false;
    [FormerlySerializedAs("roomGameObjects")] public GameObject[]   normalRooms;
    public GameObject[]   shopRooms;
    public                                           int            roomsUntilMaxDifficulty   = 10;
    public                                           int            roomsBetweenShops         = 2;
    public                                           int            roomsBetweenShopsIncrease = 1;
    public                                           int            maxRoomsBetweenShops      = 6;
    public                                          Room           currentRoom {get; private set;}
    private                                          Room           nextRoom;
    private                                          float          difficulty = 0.0f;
    private                                          RoomCollection normalRoomCollection;
    private                                          RoomCollection shopRoomCollection;
    private                                          int            roomsUntilShop;
    
    void Start() {
        normalRoomCollection = new RoomCollection(normalRooms);
        shopRoomCollection = new RoomCollection(shopRooms);
        
        roomsUntilShop = roomsBetweenShops;

        currentRoom = normalRoomCollection.SpawnRoom(transform.position, transform.rotation, transform, difficulty);
        nextRoom = shopRoomCollection.SpawnRoom(currentRoom.end.position, currentRoom.end.rotation, transform, difficulty);
        roomsUntilShop                      = roomsBetweenShops;
        
        currentRoom.blendListCamera.enabled = true;
    }

    private void Update() {
        if (!currentRoom.blendListCamera.IsBlending && currentRoom.blendListCamera.LiveChild == currentRoom.endCamera) {
            NextRoom();
        }
    }

    private void NextRoom() {
        Destroy(currentRoom.gameObject);
        currentRoom = nextRoom;
        if (roomsUntilShop == 0) {
            nextRoom          =  shopRoomCollection.SpawnRoom(currentRoom.end.position, currentRoom.end.rotation, transform, difficulty);
            roomsBetweenShops += roomsBetweenShopsIncrease;
            roomsBetweenShops =  Math.Min(maxRoomsBetweenShops, roomsBetweenShops);
            roomsUntilShop    =  roomsBetweenShops;
        } else {
            nextRoom = normalRoomCollection.SpawnRoom(currentRoom.end.position, currentRoom.end.rotation, transform, difficulty);
            --roomsUntilShop;
        }
        currentRoom.blendListCamera.enabled = true;
        difficulty = Mathf.Min(1.0f, difficulty + 1.0f / roomsUntilMaxDifficulty);
    }
}
