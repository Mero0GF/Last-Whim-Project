using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PersistentData")]
public class PersistentDataSO : ScriptableObject
{
    public bool hasSword;
    public bool beachCutscenePlayed;

    private void OnEnable()
    {
        hasSword = false;
        beachCutscenePlayed = false;
    }

    public void BeachCutsceneDone()
    {
        beachCutscenePlayed = true;
    }

    public void TutorialCutsceneDone()
    {
        hasSword = true;
    }
}
