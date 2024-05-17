using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    public float speed = 150.0f;

    private float targetTime = 2.0f;

    Transform player;

    private Vector3 direction = Vector3.forward;

    private bool circle = true;

    Vector3 center;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        center = player.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;

        
        targetTime -= Time.deltaTime;

        if (circle && targetTime <= 0.0f)
        {
        //Vector2 target = player.position;
        circle = !circle;
        transform.RotateAround(center, direction, speed * Time.deltaTime);
        }

        else if (!circle && targetTime <= 0.0f)
        {
        circle = !circle;
        transform.position = Vector2.MoveTowards(transform.position, center, speed/12 * Time.deltaTime);
        }

        else
        {
            transform.RotateAround(center, direction, speed * Time.deltaTime);
        }

    }

    /*
    void timerEnded()
    {
        targetTime = 5.0f;

        circle = !circle;

    }
    */
}
