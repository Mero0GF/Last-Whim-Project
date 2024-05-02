using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptChecker : MonoBehaviour, IData
{
    public static Vector2 lastCheckpoint;
    public bool onCheckpoint = false;


    private void OnTriggerEnter2D(Collider2D collider)   
    {
        onCheckpoint = true;
        Debug.Log("TRIGGER");
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
        Debug.Log("working3");
    }
   
    public void SaveData(GameData data)
    {
        if (onCheckpoint)
        {
            data.playerPosition = this.transform.position;
        }
        
    }
}


