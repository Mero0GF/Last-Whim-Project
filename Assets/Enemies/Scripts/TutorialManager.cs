using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int totalToKill = 3;
    public int killed = 0; // the target manager changes this parameter

    private GameObject bridgeBarrier;

    private void Start()
    {
        bridgeBarrier = GameObject.FindGameObjectWithTag("Barrier");
    }

    private void FixedUpdate()
    {
        if (killed >= totalToKill)
        {
            Destroy(bridgeBarrier);
            Destroy(gameObject);
        }
    }
}
