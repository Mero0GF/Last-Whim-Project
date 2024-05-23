using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    // player states
    public enum State
    {
        Moving,
        Dashing,
        GotHit,
    }
    public State state;

    // Floating sword variables
    public bool hasSword = false;

    // Charging attack variables
    public float chargingMoveSpd = 3;

    // movement variables
    public bool canMove = true;
    public float moveSpd = 6f;

    // dodge variables
    Vector2 dodgeDir;
    private int dodgeMinCD = 0;
    private int dodgeCD = 0;
    private int dodgeMaxCD = 30;
    public float dodgeDeaccel = 0.95f;
    public float dodgeSpdMax = 30f;
    public float dodgeSpd = 30f;
    public float dodgeMinSpd = 6f;

    // collision components and variables
    private bool isMoving = false;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    SpriteRenderer spriteRenderer;
    public Animator animator;

    private PlayerInputHandler inputHandler;
    public Vector2 lastMoveDirection = Vector2.zero;
    public Vector2 inputDirection = Vector2.zero;

    public PersistentDataSO persistentDataSO;
    private GameObject Sword;
    private FloatingSword floatingSword;
    Collider2D playerCollider;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public bool enteredCheckpoint = false;

    private void Start()
    {
        Sword = GameObject.FindGameObjectWithTag("FloatingSword");
        floatingSword = Sword.GetComponent<FloatingSword>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        inputHandler = PlayerInputHandler.Instance;
        lastMoveDirection.x = 0;
        lastMoveDirection.y = -1;
        /*if (persistentDataSO.hasSword)
        {
            hasSword = true;
        }*/
    }


    private void FixedUpdate()
    {
        Vector2 inputDirection = new Vector2(inputHandler.MoveInput.x, inputHandler.MoveInput.y);
        switch (state)
        {
            case State.Moving:
                if (canMove)
                {
                    // ---------- State changing ----------
                    if ((inputHandler.DodgeInput) && (dodgeCD <= dodgeMinCD)) // check if player pressed the dodge button
                    {
                        if (inputDirection == Vector2.zero)
                        {
                            dodgeDir = lastMoveDirection;
                        }
                        else dodgeDir = inputDirection;
                        state = State.Dashing;
                    }
                    if ((isChargingAtk()) && (floatingSword.isAvailable) && (floatingSword.atkCD == 0) && (hasSword)) // check if player pressed the dodge button
                    {
                        floatingSword.isChargingAtk = true;
                        moveSpd = chargingMoveSpd;
                    }
                    else
                    {
                        floatingSword.isChargingAtk = false;
                        moveSpd = 6;
                    }
                    // ------------------------------------
                    if (inputDirection != Vector2.zero)
                    {
                        isMoving = TryMove(inputDirection);
                        if (!isMoving)
                        {
                            isMoving = TryMove(new Vector2(inputDirection.x, 0));
                            if (!isMoving)
                            {
                                isMoving = TryMove(new Vector2(0, inputDirection.y));
                            }
                        }
                        animator.SetBool("isMoving", true);
                        Animate(inputDirection);
                        lastMoveDirection = inputDirection;
                        //if (animator.GetFloat("moveX") > 0) swordAtk.direction = SwordAtk.AttackDirection.right;
                        //if (animator.GetFloat("moveX") < 0) swordAtk.direction = SwordAtk.AttackDirection.left;
                        //if (animator.GetFloat("moveY") < 0) swordAtk.direction = SwordAtk.AttackDirection.down;
                        //if (animator.GetFloat("moveY") > 0) swordAtk.direction = SwordAtk.AttackDirection.up;
                    }
                    else
                    {
                        animator.SetBool("isMoving", false);
                        Animate(inputDirection);
                    }
                    dodgeCD = Mathf.Clamp(dodgeCD - 1, dodgeMinCD, dodgeMaxCD);
                }
                break;





            case State.Dashing:
                if ((isChargingAtk()) && (floatingSword.isAvailable) && (floatingSword.atkCD == 0)) // check if player pressed the dodge button
                {
                    floatingSword.isChargingAtk = true;
                    moveSpd = chargingMoveSpd;
                }
                else
                {
                    floatingSword.isChargingAtk = false;
                    moveSpd = 6;
                }
                animator.SetBool("isDashing", true);
                isMoving = Dodge(dodgeDir);
                if (!isMoving)
                {
                    dodgeSpd = dodgeSpd / dodgeDeaccel;
                    isMoving = Dodge(new Vector2(dodgeDir.x, 0));
                    if (!isMoving)
                    {
                        dodgeSpd = dodgeSpd / dodgeDeaccel;
                        isMoving = Dodge(new Vector2(0, dodgeDir.y));
                    }
                }
                break;

            case State.GotHit:
                animator.SetTrigger("tookHit");
                StartCoroutine(RestartScene());
                break;
        }
    }

    private bool TryMove(Vector2 dir)
    {
        int count = rb.Cast(dir, movementFilter, castCollisions, moveSpd * Time.fixedDeltaTime + collisionOffset);
        if (count == 0)
        {
            rb.MovePosition(rb.position + dir * moveSpd * Time.fixedDeltaTime);
            return true;
        }
        else return false;
    }

    public void Animate(Vector2 inputDirection)
    {
        animator.SetFloat("moveX", inputDirection.x);
        animator.SetFloat("moveY", inputDirection.y);
        animator.SetFloat("lastMoveX", lastMoveDirection.x);
        animator.SetFloat("lastMoveY", lastMoveDirection.y);
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    private bool Dodge(Vector2 dodgeDir)
    {
        int count = rb.Cast(dodgeDir, movementFilter, castCollisions, dodgeSpd * Time.fixedDeltaTime + collisionOffset);
        if (count == 0)
        {
            rb.MovePosition(rb.position + dodgeDir * dodgeSpd * Time.fixedDeltaTime);
            dodgeSpd = dodgeSpd * dodgeDeaccel;
            if (dodgeSpd <= dodgeMinSpd)
            {
                dodgeSpd = 30f;
                dodgeCD = dodgeMaxCD;
                animator.SetBool("isDashing", false);
                state = State.Moving;
            }
            return true;
        }
        else
        {
            dodgeSpd = dodgeSpd * dodgeDeaccel;
            if (dodgeSpd <= dodgeMinSpd)
            {
                dodgeSpd = 30f;
                dodgeCD = dodgeMaxCD;
                animator.SetBool("isDashing", false);
                state = State.Moving;
            }
            return false;
        }
    }

    public void CancelWakeup()
    {
        animator.SetBool("isWakingUp", false);
    }

    public bool isChargingAtk()
    {
        return inputHandler.FireInput;
    }

    public IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadData(GameData data)
    {
        if(data.lastSceneIndex == 0) //indice da cene MenuScene
        {
            if(data.playerPersistentData.beachCutscenePlayed == false)
            {
                Vector2 playerSpawnPosition;
                playerSpawnPosition.x = -4.53f;
                playerSpawnPosition.y = -8;
                this.transform.position = playerSpawnPosition;
            }
            /*else if(data.checkpointPosition.x == 0 && data.checkpointPosition.y == 0)
            {
                Vector2 playerSpawnPosition;
                playerSpawnPosition.x = -4.53f;
                playerSpawnPosition.y = -8;
                this.transform.position = playerSpawnPosition;
                data.playerPersistentData.beachCutscenePlayed = false;
            }*/
            else
            {
                this.transform.position = data.checkpointPosition;
            }                
        }
        else
        {
            this.transform.position = data.playerSpawnPosition;
        }
        
        //load values from our game data into the srciptable object
        this.persistentDataSO.hasSword = data.playerPersistentData.hasSword;
        this.persistentDataSO.beachCutscenePlayed = data.playerPersistentData.beachCutscenePlayed;
        this.hasSword = data.playerPersistentData.hasSword;

        
    }

    public void SaveData(GameData data)
    {
        data.playerSpawnPosition = this.transform.position;
        
        //save values for our scriptable object into the game data
        data.playerPersistentData.hasSword = this.persistentDataSO.hasSword;
        data.playerPersistentData.beachCutscenePlayed = this.persistentDataSO.beachCutscenePlayed;

        if (enteredCheckpoint)
        {
            data.checkpointPosition = this.transform.position;
            //get scene name
            Scene scene = SceneManager.GetActiveScene();
            data.sceneName = scene.name;
            data.lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
            enteredCheckpoint = false;
        }
    }

    public bool Interact()
    {
        return inputHandler.InteractInput;
    }

    public void SwordPickup()
    {
        persistentDataSO.hasSword = true;
        hasSword = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            Debug.Log(collision.tag);
            enteredCheckpoint = true;
            DataPersistenceManager.instance.SaveGame();
        }

        if (((collision.tag == "Enemy") || (collision.tag == "Rock") || (collision.tag == "EnemySword") || (collision.tag == "EnemyEye")) && (state == State.Moving))
        {
            state = State.GotHit;
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((collision.tag == "Enemy") || (collision.tag == "Rock") || (collision.tag == "EnemySword") || (collision.tag == "EnemyEye")) && (state == State.Moving))
        {
            state = State.GotHit;
            
        }
    }

}
