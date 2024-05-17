using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private GameObject sword;
    private FloatingSword floatingSword;
    private Animator animator;

    private void Start()
    {
        sword = GameObject.FindGameObjectWithTag("FloatingSword");
        floatingSword = sword.GetComponent<FloatingSword>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FloatingSword") && (floatingSword.state == FloatingSword.State.Attack))
        {
            animator.SetTrigger("Open");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("FloatingSword") && (floatingSword.state == FloatingSword.State.Attack))
        {
            animator.SetTrigger("Open");
        }
    }

    private void DisableCollider()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
