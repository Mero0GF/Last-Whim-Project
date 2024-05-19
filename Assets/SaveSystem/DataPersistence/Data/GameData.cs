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
    public Vector2 checkpointPosition;
    public string sceneName;
    public long lastUpdated;
    public int lastSceneIndex;
    public GameData()
    {
        this.playerSpawnPosition = Vector2.zero;
        this.swordSpawnPosition = Vector2.zero;        
        this.checkpointPosition = Vector2.zero;
        playerPersistentData = new PersistentData();
        string sceneName = string.Empty;
        lastSceneIndex = -1;
    }

    public string GetNameLocation()
    {
        string name = "";
        switch(this.sceneName)
        {
            case "Beach":
                name = "Last location: Beach";
                break;

            case "SwordCave":
                name = "Last location: Cave";
                break;
            
            case "FirstBossRoom":
                name = "Last location: First Boss";
                break;

            case "Forest":
                name = "Last location: Forest";
                break;

            case "SecondBossRoom":
                name = "Last location: Second Boss";
                break;

            case "TempleEntrance":
                name = "Last location: Temple";
                break;

            case "ThirdBossRoom":
                name = "Last location: Third Boss";
                break;

            default:
                name = "";
                break;
        }
        return name;
    }
}
