using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollow : MonoBehaviour
{

    public float speed = 2.5f;

    Transform player;

    Boss boss;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        boss = GetComponent<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        boss.LookAtPlayer();

        Vector2 target = player.position;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
