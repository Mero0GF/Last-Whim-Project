using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//dados a serem salvos
[System.Serializable]
public class GameData
{
    public Vector2 playerSpawnPosition;
    public Vector2 swordSpawnPosition;
    public PersistentData playerPersistentData;
    public GameData()
    {
        this.playerSpawnPosition = Vector2.zero;
        this.swordSpawnPosition = Vector2.zero;        
        playerPersistentData = new PersistentData();
    }
}
