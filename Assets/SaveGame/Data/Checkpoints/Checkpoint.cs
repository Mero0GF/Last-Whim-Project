/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            ScriptChecker.lastCheckpoint = transform.position;
            ScriptChecker.onCheckpoint = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        ScriptChecker.onCheckpoint = false;
    }
}*/