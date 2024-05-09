using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossHead : MonoBehaviour
{
    public float maxSpd = 30f;
    public float speed;
    public float deaccel = 0.97f;
    public Vector2 direction;

    public bool gotHit = false;

    public GameObject body;
    public GameObject boss;
    private GameObject sword;
    private FloatingSword floatingSword;
    public BossController bossController;
    private Collider2D col;
    private Rigidbody2D rb;

    public enum State
    {
        Static,
        Bouncing,
        Exposed,
    }
    public State state;

    private void Start()
    {
        sword = GameObject.FindGameObjectWithTag("FloatingSword");
        floatingSword = sword.GetComponent<FloatingSword>();
        state = State.Static;
        speed = maxSpd;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case State.Static:
                if (gotHit)
                {
                    state = State.Bouncing;
                }
                break;




            case State.Bouncing:
                rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
                speed = Mathf.Clamp(speed * deaccel, 1, 30);
                if (speed <= 1)
                {
                    speed = maxSpd;
                    col.enabled = true;
                    state = State.Exposed;
                }
                break;

            case State.Exposed:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FloatingSword") && (floatingSword.state == FloatingSword.State.Attack) && (state == State.Exposed))
        {
            Destroy(boss);
        }

    }

}
