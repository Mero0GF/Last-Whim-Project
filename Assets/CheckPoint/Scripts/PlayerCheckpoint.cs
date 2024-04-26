using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    public PlayerController player;
    public Vector2 spawnPosition;
    public Vector2 spawnSwordPosition;
    public SpawnPositionData data;
    public FloatingSword sword;
    

    void Start()
    {
        data = SaveSystem.LoadSpawnPosition(this);
        spawnPosition.x = data.position[0];
        spawnPosition.y = data.position[1];
        player.transform.position = spawnPosition;
        spawnSwordPosition.x = data.position[0] + 0.37f;
        spawnSwordPosition.y = data.position[1] + 0.37f;
        sword.transform.position = spawnSwordPosition;
    }

    
    void Update()
    {
        if(player.transform.position.x < -10f && player.transform.position.y < -5f) 
        { 
            player.transform.position = spawnPosition;
            sword.transform.position = spawnSwordPosition;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spawnPosition = player.transform.position;
            spawnSwordPosition = sword.transform.position;
            Destroy(gameObject);
        }
        SaveSystem.SaveSpawnPosition(this);
    }

}
