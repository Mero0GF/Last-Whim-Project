using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BossController : MonoBehaviour
{

    public enum State
    {
        Static,
        FollowingPlayer,
        SeekingHead,
        MeleeAtk,
    }
    public State state;

    public bool gotHit = false;
    private float staggerSpd = 0;
    private float speed;
    private Vector3 headPos = new Vector3(0, 2.4f);
    private Vector3 rockSpawnPos = new Vector3(0, 10);
    private float distance;
    private Vector2 playerPos;

    private bool wait = false;
    private bool canMove = false;
    public float atkRange = 7;
    private float atkSpd = 25;
    private float atkCd = 0;
    private float atkCdMax = 120;
    private float atkCdMin = 0;

    [SerializeField] BossManager manager;
    public GameObject barrier;
    public GameObject rock;
    public GameObject head;
    private BossHead bossHead;
    private GameObject player;
    private GameObject sword;
    private FloatingSword floatingSword;

    private Animator animator;
    private SpriteRenderer spriteRendererBody;
    private SpriteRenderer spriteRendererHead;
    private SpriteRenderer spriteRendererBarrier;

    private void Start()
    {
        speed = manager.speed;
        state = State.Static;
        animator = GetComponent<Animator>();
        spriteRendererBody = GetComponent<SpriteRenderer>();
        spriteRendererHead = head.GetComponent<SpriteRenderer>();
        spriteRendererBarrier = barrier.GetComponent<SpriteRenderer>();
        bossHead = head.GetComponent<BossHead>();
        player = GameObject.FindGameObjectWithTag("Player");
        sword = GameObject.FindGameObjectWithTag("FloatingSword");
        floatingSword = sword.GetComponent<FloatingSword>();
        barrier.SetActive(true);
    }

    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        switch (state) {

            case State.Static:
                if (wait == false)
                {
                    StartCoroutine(Static());
                }
                break;



            case State.FollowingPlayer:
                barrier.SetActive(true);
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                barrier.transform.position = transform.position + new Vector3(0,-0.3f);
                flipSprite(spriteRendererBarrier, player);
                flipSprite(spriteRendererHead, player);
                flipSprite(spriteRendererBody, player);
                SpawnRock();
                if (gotHit == false)
                {
                    head.transform.position = transform.position + headPos;
                }
                if (distance <= atkRange && atkCd <= 0)
                {
                    playerPos = player.transform.position;
                    state = State.MeleeAtk;
                }
                atkCd = Mathf.Clamp(atkCd-1,atkCdMin,atkCdMax);
                //Debug.Log(atkCd);
                break;


            case State.SeekingHead:
                transform.position = Vector2.MoveTowards(transform.position, head.transform.position, speed * Time.deltaTime);
                flipSprite(spriteRendererBarrier, head);
                flipSprite(spriteRendererHead, head);
                flipSprite(spriteRendererBody, head);
                SpawnRock();
                break;

            case State.MeleeAtk:
                barrier.transform.position = transform.position + new Vector3(0, -0.3f);
                SpawnRock();
                flipSprite(spriteRendererBarrier, player);
                flipSprite(spriteRendererHead, player);
                flipSprite(spriteRendererBody, player);
                if (gotHit == false)
                {
                    head.transform.position = transform.position + headPos;
                }
                if (canMove)
                {
                    transform.position = Vector2.MoveTowards(transform.position, playerPos, atkSpd * Time.deltaTime);
                }
                if (atkCd <= 0)
                {
                    StartCoroutine(MeleeAtk());
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FloatingSword") && (floatingSword.state == FloatingSword.State.Attack))
        {
            barrier.SetActive(false);
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

    IEnumerator MeleeAtk()
    {
        atkCd = atkCdMax;

        // Attack 1
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        playerPos = player.transform.position;
        canMove = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.25f);
        animator.SetTrigger("Idle");
        canMove = false;

        // Attack 2
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        playerPos = player.transform.position;
        canMove = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.25f);
        animator.SetTrigger("Idle");
        canMove = false;

        // Attack 3
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.5f);
        playerPos = player.transform.position;
        canMove = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        canMove = false;

        barrier.SetActive(false);
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(2f);
        if (!gotHit)
        {
            state = State.FollowingPlayer;
            barrier.SetActive(true);
        }
    }

    private IEnumerator Static()
    {
        wait = true;
        yield return new WaitForSeconds(1f);
        state = State.FollowingPlayer;
    }

    private void SpawnRock()
    {
        if (!GameObject.FindGameObjectWithTag("Rock"))
        {
            Instantiate(rock, player.transform.position + rockSpawnPos, Quaternion.identity);
        }
    }

    private void flipSprite(SpriteRenderer sprite, GameObject objPos)
    {
        if (objPos.transform.position.x < transform.position.x)
        {
            sprite.flipX = true;
        }
        else if (objPos.transform.position.x > transform.position.x)
        {
            sprite.flipX = false;
        }
    }

}
