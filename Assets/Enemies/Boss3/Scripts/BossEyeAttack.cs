using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEyeAttack : MonoBehaviour
{

    private Transform player;
    [SerializeField] private GameObject enemyEye;

    private float targetTime = 3.5f;

    private GameObject eye1;
    private GameObject eye2;
    private GameObject eye3;
    private GameObject eye4;
    private GameObject eye5;

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
        targetTime = 3.5f;

        player = GameObject.FindGameObjectWithTag("Player").transform;


        //Vector3 spawnPos = new Vector3(player.position.x, player.position.y + 5, player.position.z);


        eye1 = GameObject.Instantiate(enemyEye, new Vector3(player.position.x, player.position.y + 10, player.position.z), enemyEye.transform.rotation);
        eye2 = GameObject.Instantiate(enemyEye, new Vector3(player.position.x-8, player.position.y + 6, player.position.z), enemyEye.transform.rotation);
        eye3 = GameObject.Instantiate(enemyEye, new Vector3(player.position.x+8, player.position.y + 6, player.position.z), enemyEye.transform.rotation);
        eye4 = GameObject.Instantiate(enemyEye, new Vector3(player.position.x-6, player.position.y - 8, player.position.z), enemyEye.transform.rotation);
        eye5 = GameObject.Instantiate(enemyEye, new Vector3(player.position.x+6, player.position.y - 8, player.position.z), enemyEye.transform.rotation);
    }
}
