using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingSword : MonoBehaviour
{
    private enum State
    {
        FollowingPlayer,
        ChargingAtk,
        Bouncing
    }
    private State state;

    // Following player variables
    public bool canMove = true;
    public float speed = 0.6f;
    public float maxSpd = 6;
    public float minSpd = 0.6f;
    public float accel = 1.04f;
    public float deaccel = 0.935f;
    public float maxDistance = 0.8f;
    private float pos = 0.37f;
    private float distance;

    // Bouncing variables
    private float y;
    private bool flagUp = true,flagDown = false;
    public float deaccelPoint = 0.1f;
    public float limitPosUp = 0.1f;
    public float limitPosDown = -0.1f;
    public float bounceSpd = 0.5f;

    SpriteRenderer spriteRenderer;
    public GameObject player;

    Collider2D swordCollider;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    void Start()
    {
        transform.position = new Vector3(pos, pos, 0);
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = State.FollowingPlayer;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.FollowingPlayer:
                distance = Vector2.Distance(transform.position, (player.transform.position + new Vector3(pos, pos, 0)));
                Vector2 direction = (player.transform.position + new Vector3(pos, pos, 0)) - transform.position;

                if (distance == 0)
                {
                    speed = 0;
                    y = transform.position.y;
                    state = State.Bouncing;
                }
                else if (distance > 0)
                {
                    if (distance <= maxDistance)
                    {
                        speed = Mathf.Clamp(speed * (deaccel * deaccel), minSpd, maxSpd);
                    }
                    else
                    {
                        speed = Mathf.Clamp(speed * (accel * accel), minSpd, maxSpd);
                    }
                }

                transform.position = Vector2.MoveTowards(transform.position, (player.transform.position + new Vector3(pos, pos, 0)), speed * Time.deltaTime);
                break;
            case State.ChargingAtk:
                break;
            case State.Bouncing:
                distance = Vector2.Distance(transform.position, (player.transform.position + new Vector3(pos, pos, 0)));
                if (distance > 0.25f)
                {
                    speed = 0;
                    state = State.FollowingPlayer;
                }
                if (flagUp)
                {
                    BounceUp();
                }
                else if (flagDown)
                {
                    BounceDown();
                }
                break;
        }
    }

    private void BounceUp()
    {
        if (transform.position.y > (y + deaccelPoint)) bounceSpd = Mathf.Clamp(bounceSpd * 0.935f, 0.05f, 0.5f); //deacceleration
        else bounceSpd = Mathf.Clamp(bounceSpd * 1.07f, 0.06f, 0.5f); // acceleration
        transform.position = Vector2.MoveTowards(transform.position, (transform.position + new Vector3(0, limitPosUp, 0)), bounceSpd * Time.deltaTime);
        if (bounceSpd <= 0.05f)
        {
            flagUp = false;
            flagDown = true;
        }
    }

    private void BounceDown()
    {
        if (transform.position.y < (y - deaccelPoint)) bounceSpd = Mathf.Clamp(bounceSpd * 0.935f, 0.05f, 0.5f); //deacceleration
        else bounceSpd = Mathf.Clamp(bounceSpd * 1.07f, 0.06f, 0.5f); // acceleration
        transform.position = Vector2.MoveTowards(transform.position, (transform.position + new Vector3(0, limitPosDown, 0)), bounceSpd * Time.deltaTime);
        if (bounceSpd <= 0.05f)
        {
            flagDown = false;
            flagUp = true;
        }
    }
}
