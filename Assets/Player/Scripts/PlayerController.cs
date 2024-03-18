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
        ChargingAtk,
        Dodging,
    }
    private State state;

    // Charging attack variables
    public float chargingMoveSpd = 3;

    // movement variables
    public bool canMove = true;
    public float moveSpd = 6f;

    // dodge variables
    Vector2 dodgeDir;
    private int dodgeMinCD = 0;
    private int dodgeCD = 30;
    private int dodgeMaxCD = 30;
    public float dodgeDeaccel = 0.8f;
    public float dodgeSpd = 50f;
    public float dodgeMinSpd = 6f;

    // collision components and variables
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    SpriteRenderer spriteRenderer;
    Animator animator;

    private PlayerInputHandler inputHandler;
    Vector2 lastMoveDirection = Vector2.zero;
    Vector2 movementInput;

    public FloatingSword floatingSword;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        inputHandler = PlayerInputHandler.Instance;
        lastMoveDirection.x = 0;
        lastMoveDirection.y = -1;
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
                    state = State.Dodging;
                }

                if ((isCharging()) && (floatingSword.isInPlayer)) // check if player pressed the dodge button
                {
                    floatingSword.isCharging = true;
                    moveSpd = chargingMoveSpd;
                }
                else moveSpd = 6;
                // -------------------------------------

                if (canMove)
                {
                    if (inputDirection != Vector2.zero)
                    {
                        bool canMove = TryMove(inputDirection);
                        if (!canMove)
                        {
                            canMove = TryMove(new Vector2(inputDirection.x, 0));
                            if (!canMove)
                            {
                                canMove = TryMove(new Vector2(0, inputDirection.y));
                            }
                        }
                        //animator.SetBool("isMoving", true);
                        //Animate();
                        //lastMoveDirection = movementInput;
                        //if (animator.GetFloat("moveX") > 0) swordAtk.direction = SwordAtk.AttackDirection.right;
                        //if (animator.GetFloat("moveX") < 0) swordAtk.direction = SwordAtk.AttackDirection.left;
                        //if (animator.GetFloat("moveY") < 0) swordAtk.direction = SwordAtk.AttackDirection.down;
                        //if (animator.GetFloat("moveY") > 0) swordAtk.direction = SwordAtk.AttackDirection.up;
                    }
                    //else
                    //{
                    //    //animator.SetBool("isMoving", false);
                    //    //Animate();
                    //}
                    dodgeCD = Mathf.Clamp(dodgeCD - 1, dodgeMinCD, dodgeMaxCD);
                }
                break;
            case State.Dodging:
                canMove = Dodge(dodgeDir);
                if (!canMove)
                {
                    canMove = Dodge(new Vector2(dodgeDir.x, 0));
                    if (!canMove)
                    {
                        canMove = Dodge(new Vector2(0, dodgeDir.y));
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

    public void Animate()
    {
        animator.SetFloat("moveX", movementInput.x);
        animator.SetFloat("moveY", movementInput.y);
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
                dodgeSpd = 50f;
                dodgeCD = dodgeMaxCD;
                state = State.Moving;
            }
            return true;
        }
        else
        {
            dodgeSpd = dodgeSpd * dodgeDeaccel;
            if (dodgeSpd <= dodgeMinSpd)
            {
                dodgeSpd = 50f;
                dodgeCD = dodgeMaxCD;
                state = State.Moving;
            }
            return false;
        }
    }

    public bool isCharging()
    {
        return inputHandler.FireInput;
    }
}
