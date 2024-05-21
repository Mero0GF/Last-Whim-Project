using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointAnimation : MonoBehaviour
{

    private Animator animator;
    private new Collider2D collider;
    private GameObject player;
    private PlayerController playerController;

    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("Activation");
            player.transform.position = transform.position + new Vector3(0,0.4f,0);
            StartCoroutine(BlockPlayerMovement());
            StartCoroutine(BlockHitbox());
        }
    }

    IEnumerator BlockPlayerMovement()
    {
        playerController.LockMovement();
        playerController.dodgeSpd = 0;
        playerController.animator.SetBool("isDashing", false);
        playerController.animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(1.5f);
        playerController.dodgeSpd = playerController.dodgeSpdMax;
        playerController.UnlockMovement();
    }

    IEnumerator BlockHitbox()
    {
        collider.enabled = false;
        yield return new WaitForSeconds(5);
        collider.enabled = true;
    }
}
