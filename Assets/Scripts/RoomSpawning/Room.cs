﻿using UnityEngine;

public class Room : MonoBehaviour
{
    // roomtype shows type of the room
    // 0 - start room
    // 1 - regular room
    // 2 - finish room
    public byte roomType;
    
    // GameObject, which contains enemies in the room
    public GameObject enemies;

    // GameObject, which contains content of the whole room
    public GameObject room;

    ////////////////////////////////////////////////////////////
    // fixed by unknownMusician
    // check if it's correct & delete these comments

    public Room() {
        roomType = 1;
    }

    private void Start() {
        room = this.gameObject;
        enemies = room.transform.GetChild(0).gameObject;
    }

    //public Room(GameObject roomGameObject) {
    //    room = roomGameObject;
    //    enemies = roomGameObject.transform.GetChild(0).gameObject;
    //    roomType = 1;
    //}

    ////////////////////////////////////////////////////////////

    public bool IsThereAnyEnemy(GameObject room) {
        return room.transform.GetChild(0).childCount != 0;
    }

    public static class RoomType {
        readonly public static byte start = 0;
        readonly public static byte regular = 1;
        readonly public static byte finish = 2;
    }
}
