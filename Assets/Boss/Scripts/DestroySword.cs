using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySword : MonoBehaviour
{

    private float targetTime = 5.0f;

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

    }

    void timerEnded()
    {
        if(gameObject.transform.position.x > 300.0f || gameObject.transform.position.y < -300.0f)
        {
            Destroy(gameObject);
        }
        
    }
}
