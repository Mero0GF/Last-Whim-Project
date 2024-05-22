using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FloatingSword : MonoBehaviour, IDataPersistence
{
    public enum State
    {
        FollowingPlayer,
        ChargingAtk,
        Attack,
        Retrieving,
        Bouncing,
        Static,
        
    }
    public State state;

    private List<RaycastHit2D> hit = new List<RaycastHit2D>();
    public ContactFilter2D contactFilter;

    // Sword retrieving variables
    private float retrievingMinSpd = 2;
    private float retrievingDistanceOffset = 2f;

    // Sword attack variables
    private bool enemyHitAux = false;
    public int atkCD = 0;
    private int maxAtkCD = 60;
    private int minAtkCD = 0;
    public bool isAvailable = true;
    public bool isChargingAtk = false;
    private float distanceOffset = 0.6f;
    private bool canAttack = false;
    public Vector2 atkDir = Vector2.zero;
    private float chargeSpd = 2;
    private float atkDeaccel = 0.8f;
    private float maxCharge = 220f;
    private float minCharge = 100f;
    private float charge = 0;

    // Following player variables
    private Quaternion q;
    private float defaultAngle = 0;
    public float angle = 30;
    public float rotationSpd = 3f;
    private float atkRotationSpd = 20f;
    public float rotationOffset = 0;
    private float atkRotationOffset = 90;
    public float speed = 0.6f;
    public float maxSpd = 6;
    public float minSpd = 0.6f;
    public float accel = 1.04f;
    public float deaccel = 0.9325f;
    public float maxDistance = 0.8f;
    private float pos = 0.45f;
    private float distance;

    // Bouncing variables
    private float y;
    private bool flagUp = true,flagDown = false;
    public float deaccelPoint = 0.1f;
    public float limitPosUp = 0.1f;
    public float limitPosDown = -0.1f;
    public float bounceSpd = 0.5f;

    SpriteRenderer spriteRenderer;
    private GameObject player;
    private PlayerController playerController;

    Rigidbody2D rb;

    GameData data;
    PersistentDataSO persistentDataSO;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        LayerMask mask = LayerMask.GetMask("Col");
        contactFilter.SetLayerMask(mask);
        if (playerController.hasSword) state = State.FollowingPlayer;
        else state = State.Static;
    }

    private void FixedUpdate()
    {
        // calcula o angulo que a espada deve se mover quando tiver seguindo o jogador (e em outros estados tb)
        if (playerController.lastMoveDirection.y > 0) rotationOffset = -90;
        else if (playerController.lastMoveDirection.y < 0) rotationOffset = 90;
        angle = Mathf.Atan2((player.transform.position.y + pos) - transform.position.y, (player.transform.position.x + pos) - transform.position.x) * Mathf.Rad2Deg + rotationOffset;
        if (angle < -25 || angle > 25) angle = 25;
        Vector2 direction = (player.transform.position + new Vector3(pos, pos, 0)) - transform.position;
        // ----------------------------------------------------------------------------------------------

        switch (state)
        {
            case State.FollowingPlayer:
                
                distance = Vector2.Distance(transform.position, (player.transform.position + new Vector3(pos, pos, 0)));

                if (isChargingAtk) // checks if player is charging the attack and changes the value of some core variables
                {
                    accel = 1.12f;
                    minSpd = 6f;
                    maxSpd = 160;
                    state = State.ChargingAtk;
                }
                else if (speed > maxSpd)
                {
                    speed = Mathf.Clamp(speed * 0.8f, 5.8f, maxCharge);

                }
                else
                {
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
                            q = Quaternion.AngleAxis(defaultAngle, Vector3.forward);
                            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpd);
                            speed = Mathf.Clamp(speed * (deaccel * deaccel), minSpd, maxSpd);
                        }
                        else
                        {
                            if (direction.x > 0)
                            {
                                if (angle < 0) angle = -angle;
                                q = Quaternion.AngleAxis(-angle, Vector3.forward);
                                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpd);
                            }
                            else if (direction.x < 0)
                            {
                                if (angle < 0) angle = -angle;
                                q = Quaternion.AngleAxis(angle, Vector3.forward);
                                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpd);
                            }
                            else
                            {
                                q = Quaternion.AngleAxis(defaultAngle, Vector3.forward);
                                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpd);
                            }
                            speed = Mathf.Clamp(speed * (accel * accel), minSpd, maxSpd);
                        }
                    }
                }
                atkCD = Mathf.Clamp(atkCD-1,minAtkCD,maxAtkCD);
                transform.position = Vector2.MoveTowards(transform.position, (player.transform.position + new Vector3(pos, pos, 0)), speed * Time.deltaTime);
                break;





            case State.ChargingAtk:
                if (isChargingAtk) // checks if player is pressing the button
                {
                    angle = Mathf.Atan2((transform.position.y + playerController.lastMoveDirection.y) - transform.position.y, (transform.position.x + playerController.lastMoveDirection.x) - transform.position.x) * Mathf.Rad2Deg + atkRotationOffset;
                    distance = Vector2.Distance(transform.position, (player.transform.position + new Vector3(pos, pos, 0)));
                    // all the variables inside this state were changed on "FollowingPlayer" state before
                    if (canAttack)
                    {
                        q = Quaternion.AngleAxis(angle, Vector3.forward);
                        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * atkRotationSpd);
                        charge = Mathf.Clamp(charge + chargeSpd, minCharge, maxCharge); // charges the speed/power of the attack
                    }
                    if ((distance <= distanceOffset))
                    {
                        speed = playerController.chargingMoveSpd;
                        canAttack = true;
                    }
                    else 
                    {
                        speed = Mathf.Clamp(speed * accel , minSpd, maxSpd); 
                    }
                    transform.position = Vector2.MoveTowards(transform.position, (player.transform.position + new Vector3(pos, pos, 0)), speed * Time.deltaTime);
                }
                else if(canAttack) // change back the variables to their original values and setup for the attack state
                {
                    maxSpd = 6;
                    minSpd = 0.6f;
                    accel = 1.04f;
                    atkDir = playerController.lastMoveDirection;
                    speed = charge;
                    canAttack = false;
                    state = State.Attack;
                }
                else
                {
                    maxSpd = 6;
                    minSpd = 0.6f;
                    accel = 1.04f;
                    speed = Mathf.Clamp(speed * deaccel, 5.8f, maxCharge);
                    transform.position = Vector2.MoveTowards(transform.position, (player.transform.position + new Vector3(pos, pos, 0)), speed * Time.deltaTime);
                    state = State.FollowingPlayer;
                }
                break;





            case State.Attack:
                isAvailable = false;
                Vector2 swordPos = new Vector2(transform.position.x, transform.position.y);
                int enemyMask = LayerMask.GetMask("Enemies");
                int colMask = LayerMask.GetMask("Col");
                RaycastHit2D collision = Physics2D.Raycast(swordPos, atkDir, speed * Time.deltaTime, colMask);
                if (enemyHitAux == true)
                {
                    charge = 0;
                    speed = 0;
                    isAvailable = true;
                    state = State.Retrieving;
                }

                // checks collision with walls
                if (collision == true)
                {
                    rb.position = collision.point;
                    charge = 0;
                    speed = 0;
                    isAvailable = true;
                    state = State.Retrieving;
                }
                else
                {
                    RaycastHit2D enemyHit = Physics2D.Raycast(swordPos, atkDir, speed * Time.deltaTime, enemyMask);

                    // checks collision with enemies
                    if (enemyHit)
                    {
                        rb.position = enemyHit.point;
                        speed = 4;
                        enemyHitAux = true;
                    }
                    rb.MovePosition(rb.position + atkDir * speed * Time.deltaTime);
                    speed = Mathf.Clamp(speed * atkDeaccel, 1, maxCharge);
                    if (speed == 1)
                    {
                        charge = 0;
                        speed = 0;
                        isAvailable = true;
                        state = State.Retrieving;
                    }
                }
                break;





            case State.Retrieving:
                // isAvailable set true on Attack state
                distance = Vector2.Distance(transform.position, (player.transform.position + new Vector3(pos, pos, 0)));
                if ((isChargingAtk) && (distance != 0)) 
                {
                    if (direction.x > 0)
                    {
                        if (angle < 0) angle = -angle;
                        q = Quaternion.AngleAxis(-angle, Vector3.forward);
                        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpd);
                    }
                    else if (direction.x < 0)
                    {
                        if (angle < 0) angle = -angle;
                        q = Quaternion.AngleAxis(angle, Vector3.forward);
                        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpd);
                    }
                    else
                    {
                        q = Quaternion.AngleAxis(defaultAngle, Vector3.forward);
                        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpd);
                    }
                    speed = Mathf.Clamp(speed*accel, retrievingMinSpd, maxCharge);
                    transform.position = Vector2.MoveTowards(transform.position, (player.transform.position + new Vector3(pos, pos, 0)), speed * Time.deltaTime);
                }
                else if (distance > retrievingDistanceOffset)
                {
                    q = Quaternion.AngleAxis(defaultAngle, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpd);
                    speed = Mathf.Clamp(speed * deaccel, retrievingMinSpd, maxCharge);
                    if (speed == retrievingMinSpd) speed = 0;
                    transform.position = Vector2.MoveTowards(transform.position, (player.transform.position + new Vector3(pos, pos, 0)), speed * Time.deltaTime);
                }
                if (distance <= retrievingDistanceOffset)
                {
                    transform.position = Vector2.MoveTowards(transform.position, (player.transform.position + new Vector3(pos, pos, 0)), speed * Time.deltaTime);
                    atkCD = maxAtkCD;
                    enemyHitAux = false;
                    state = State.FollowingPlayer;
                }
                break;





            case State.Bouncing:
                distance = Vector2.Distance(transform.position, (player.transform.position + new Vector3(pos, pos, 0)));
                if (isChargingAtk) // checks if player is charging the attack
                {
                    accel = 1.4f;
                    minSpd = 0.6f;
                    maxSpd = 160;
                    canAttack = true;
                    state = State.ChargingAtk;
                }

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
                q = Quaternion.AngleAxis(defaultAngle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpd);
                atkCD = Mathf.Clamp(atkCD - 1, minAtkCD, maxAtkCD);
                break;





            case State.Static:
                if (playerController.hasSword) state = State.FollowingPlayer;
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

    public void SaveData(GameData data)
    {
        Vector2 swordSpawnPosition;
        swordSpawnPosition.x = player.transform.position.x + pos;
        swordSpawnPosition.y = player.transform.position.y + pos;
        data.swordSpawnPosition = swordSpawnPosition;
    }

    public void LoadData(GameData data)
    {
        if(data.playerPersistentData.hasSword == true)
        {
            if(data.lastSceneIndex == 0)
            {
                Vector2 swordSpawnPosition;
                swordSpawnPosition.x = data.checkpointPosition.x + pos;
                swordSpawnPosition.y = data.checkpointPosition.y + pos;
                this.transform.position = swordSpawnPosition;
            }
            else
            {
                this.transform.position = data.swordSpawnPosition;
            }                                    
        }   
    }

    // On trigger functions

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (state == State.Attack)
    //    {
    //        if (collision.tag == "Wall")
    //        {
    //            charge = 0;
    //            speed = 0;
    //            isAvailable = true;
    //            state = State.Retrieving;
    //        }
    //    }
    //    else
    //    {
    //        // Does nothing
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (state == State.Attack)
    //    {
    //        if (collision.tag == "Wall")
    //        {
    //            charge = 0;
    //            speed = 0;
    //            isAvailable = true;
    //            state = State.Retrieving;
    //        }
    //    }
    //    else
    //    {
    //        // Does nothing
    //    }
    //}
}

