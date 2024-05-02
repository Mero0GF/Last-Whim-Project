using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool spawn;
    public Vector2 playerPosition;
    public SerializableDictionary<string, bool> spawnEnemy;

    public GameData() //Initial values for new game.
    {
        this.spawn = true;
        playerPosition = Vector2.zero;
        spawnEnemy = new SerializableDictionary<string, bool>();
    }
}