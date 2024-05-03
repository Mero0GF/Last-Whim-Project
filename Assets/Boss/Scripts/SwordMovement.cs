using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{

    private Transform target;

    [SerializeField] private float speed = 5.0f;

    private float targetTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z - 45);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.right = target.position - transform.position;

        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            timerEnded();
        }

        void timerEnded()
        {
            Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
