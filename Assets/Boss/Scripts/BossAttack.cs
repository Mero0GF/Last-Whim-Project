using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    private Transform player;
    [SerializeField] private GameObject enemySword;

    private float targetTime = 2.0f;

    private GameObject swordattack1;
    private GameObject swordattack2;
    private GameObject swordattack3;

    //private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        //enemySword = GameObject.FindGameObjectWithTag("EnemySword");
        //enemySword.AddComponent<Rigidbody2D>();

        //count++;

    }

    // Update is called once per frame
    void Update()
    {

        targetTime -= Time.deltaTime;

        if(targetTime <= 0.0f)
        {
            timerEnded();
        }

        
    }

    void timerEnded()
    {
        targetTime = 2.0f;
        
        player = GameObject.FindGameObjectWithTag("Player").transform;


        //Vector3 spawnPos = new Vector3(player.position.x, player.position.y + 5, player.position.z);


        swordattack1 = GameObject.Instantiate(enemySword, new Vector3(player.position.x-3, player.position.y + 5, player.position.z), enemySword.transform.rotation);
        swordattack2 = GameObject.Instantiate(enemySword, new Vector3(player.position.x, player.position.y + 5, player.position.z), enemySword.transform.rotation);
        swordattack3 = GameObject.Instantiate(enemySword, new Vector3(player.position.x+3, player.position.y + 5, player.position.z), enemySword.transform.rotation);
    }
}
