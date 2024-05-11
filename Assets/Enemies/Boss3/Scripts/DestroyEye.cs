using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEye : MonoBehaviour
{

    private float targetTime = 4.0f;

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
            timerEnded();
        }

        void timerEnded()
        {
            Destroy(gameObject);
        }
    }
}
