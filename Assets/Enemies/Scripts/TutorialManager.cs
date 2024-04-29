using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int totalToKill = 3;
    public int killed = 0; // the target manager changes this parameter

    private GameObject player;
    private GameObject sword;
    private new GameObject camera;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sword = GameObject.FindGameObjectWithTag("FloatingSword");
        camera = GameObject.FindGameObjectWithTag("Camera");
    }

    private void FixedUpdate()
    {
        if (killed >= totalToKill)
        {
            Vector2 distance = sword.transform.position - player.transform.position;

            player.transform.position = new Vector2(1.2f,-3.2f);
            sword.transform.position = new Vector2(player.transform.position.x + distance.x,player.transform.position.y + distance.y);
            camera.transform.position = player.transform.position;
            Destroy(gameObject);
        }
    }
}
