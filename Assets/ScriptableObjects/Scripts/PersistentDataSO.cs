using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PersistentData")]
public class PersistentDataSO : ScriptableObject
{
    public bool hasSword;
    public bool beachCutscenePlayed;
    public bool firstBossDone;

    private void OnEnable()
    {
        hasSword = false;
        beachCutscenePlayed = false;
        firstBossDone = false;
    }

    public void BeachCutsceneDone()
    {
        beachCutscenePlayed = true;
    }

    public void TutorialCutsceneDone()
    {
        hasSword = true;
    }

    public void FirstBossKilled()
    {
        firstBossDone = true;
    }
}
