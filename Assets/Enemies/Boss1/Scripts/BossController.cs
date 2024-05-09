using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BossController : MonoBehaviour
{

    public enum State
    {
        FollowingPlayer,
        SeekingHead,
    }
    public State state;

    private float staggerSpd = 0;
    private float speed;
    public Vector3 headPos = new Vector3(0, 3);

    [SerializeField] BossManager manager;
    public GameObject head;
    private BossHead bossHead;
    private GameObject player;
    private GameObject sword;
    private FloatingSword floatingSword;

    private void Start()
    {
        speed = manager.speed;
        state = State.FollowingPlayer;
        bossHead = head.GetComponent<BossHead>();
        player = GameObject.FindGameObjectWithTag("Player");
        sword = GameObject.FindGameObjectWithTag("FloatingSword");
        floatingSword = sword.GetComponent<FloatingSword>();
    }

    private void FixedUpdate()
    {
        switch (state) {
            case State.FollowingPlayer:
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                if (!bossHead.gotHit)
                head.transform.position = transform.position + headPos;

                break;


            case State.SeekingHead:
                transform.position = Vector2.MoveTowards(transform.position, head.transform.position, speed * Time.deltaTime);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FloatingSword") && (floatingSword.state == FloatingSword.State.Attack))
        {
            StartCoroutine(Stagger());
            bossHead.direction = floatingSword.atkDir;
            bossHead.gotHit = true;
        }
    }

    IEnumerator Stagger()
    {
        speed = staggerSpd;
        yield return new WaitForSeconds(1.5f);
        speed = manager.speed;
        state = State.SeekingHead;
        bossHead.gotHit = false;
    }
}
