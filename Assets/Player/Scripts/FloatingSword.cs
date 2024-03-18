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

    // Sword attack variables
    public bool isInPlayer = true;
    public bool isCharging = false;
    private float maxCharge = 250f;
    private float minCharge = 100f;
    private float charge = 100f;

    // Following player variables
    public bool canMove = true;
    public float speed = 0.6f;
    public float maxSpd = 6;
    public float minSpd = 0.6f;
    public float accel = 1.04f;
    public float deaccel = 0.9325f;
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
    public PlayerController playerController;

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

                if (isCharging) // checks if player is charging the attack and changes the value of some core variables
                {
                    speed = 0;
                    deaccel = 0.8f;
                    accel = 1.25f;
                    minSpd = 0.6f;
                    maxSpd = 160;
                    state = State.ChargingAtk;
                }

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
                if (playerController.isCharging()) // checks if player is pressing the button
                {
                    distance = Vector2.Distance(transform.position, (player.transform.position + new Vector3(pos, pos, 0)));

                    if (distance == 0)
                    {
                        speed = playerController.chargingMoveSpd;
                    }
                    else 
                    {
                        speed = Mathf.Clamp(speed * (accel * accel), minSpd, maxSpd);
                    }
                    transform.position = Vector2.MoveTowards(transform.position, (player.transform.position + new Vector3(pos, pos, 0)), speed * Time.deltaTime);
                }
                else
                {
                    speed = 0;
                    maxSpd = 6;
                    minSpd = 0.6f;
                    accel = 1.04f;
                    deaccel = 0.9325f;
                    isCharging = false;
                    state = State.FollowingPlayer;
                }
                break;
            case State.Bouncing:
                if (isCharging) // checks if player is charging the attack
                {
                    speed = 0;
                    deaccel = 0.2f;
                    accel = 1.4f;
                    minSpd = 0.6f;
                    maxSpd = 160;
                    state = State.ChargingAtk;
                }
                distance = Vector2.Distance(transform.position, (player.transform.position + new Vector3(pos, pos, 0)));
                if (distance > 0.25f)
                {
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
