using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwordAttack : MonoBehaviour
{
    private float targetTime = 6.0f;

    public bool enableSwordAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            enableSwordAttack = !enableSwordAttack;
            targetTime = 6.0f;
        }
    }
}
