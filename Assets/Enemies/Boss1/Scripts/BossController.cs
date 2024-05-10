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

    public bool gotHit = false;
    private float staggerSpd = 0;
    private float invincibilityFrame = 0;
    private float speed;
    public Vector3 headPos = new Vector3(0, 3);
    public Vector3 rockSpawnPos = new Vector3(0, 10);

    [SerializeField] BossManager manager;
    public GameObject barrier;
    public GameObject rock;
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
                barrier.transform.position = transform.position + new Vector3(0,0.5f);
                SpawnRock();
                if (gotHit == false)
                {
                    head.transform.position = transform.position + headPos;
                    invincibilityFrame = Mathf.Clamp(invincibilityFrame - 1, manager.invincibilityFrameMin, manager.invincibilityFrameMax);
                    if (invincibilityFrame > 0)
                    {
                        barrier.SetActive(true);
                    }
                    else barrier.SetActive(false);
                }
                break;


            case State.SeekingHead:
                transform.position = Vector2.MoveTowards(transform.position, head.transform.position, speed * Time.deltaTime);
                SpawnRock();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FloatingSword") && (floatingSword.state == FloatingSword.State.Attack) && (state == State.FollowingPlayer) && (invincibilityFrame <= 0))
        {
            StartCoroutine(Stagger());
        }
        if (collision.CompareTag("BossHead") && (state == State.SeekingHead))
        {
            StartCoroutine(PickingUpHead());
        }
    }

    IEnumerator Stagger()
    {
        speed = staggerSpd; 
        bossHead.direction = floatingSword.atkDir;
        invincibilityFrame = manager.invincibilityFrameMax;
        gotHit = true;
        yield return new WaitForSeconds(1.5f);
        speed = manager.speed;
        state = State.SeekingHead;
    }

    IEnumerator PickingUpHead()
    {
        speed = staggerSpd;
        yield return new WaitForSeconds(1.5f);
        speed = manager.speed;
        gotHit = false;
        head.transform.position = transform.position + headPos;
        bossHead.state = BossHead.State.Static;
        state = State.FollowingPlayer;
    }

    private void SpawnRock()
    {
        if (!GameObject.FindGameObjectWithTag("Rock"))
        {
            Instantiate(rock, player.transform.position + rockSpawnPos, Quaternion.identity);
        }
    }

}
