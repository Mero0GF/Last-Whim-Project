using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    // player states
    private enum State
    {
        Moving,
        Dashing,
    }
    private State state;

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
    public float dodgeSpd = 30f;
    public float dodgeMinSpd = 6f;

    // collision components and variables
    private bool isMoving = false;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    SpriteRenderer spriteRenderer;
    Animator animator;

    private PlayerInputHandler inputHandler;
    public Vector2 lastMoveDirection = Vector2.zero;
    public Vector2 inputDirection = Vector2.zero;

    public FloatingSword floatingSword;
    Collider2D playerCollider;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        inputHandler = PlayerInputHandler.Instance;
        lastMoveDirection.x = 0;
        lastMoveDirection.y = -1;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("working2");
    }

    private void FixedUpdate()
    {
        Vector2 inputDirection = new Vector2(inputHandler.MoveInput.x, inputHandler.MoveInput.y);
        switch (state)
        {
            case State.Moving:
                // ---------- State changing ----------
                if ((inputHandler.DodgeInput) && (dodgeCD <= dodgeMinCD)) // check if player pressed the dodge button
                {
                    dodgeDir = inputDirection;
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

                if (canMove)
                {
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

    public bool isChargingAtk()
    {
        return inputHandler.FireInput;
    }

    public void SwordPickup()
    {
        hasSword = true;
    }
}
