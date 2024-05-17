using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersistentData
{
    public bool hasSword;
    public bool beachCutscenePlayed;

    public PersistentData() 
    { 
        hasSword = false;
        beachCutscenePlayed = false;
    }
}
