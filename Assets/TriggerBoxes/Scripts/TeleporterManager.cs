using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{
    public float xPos;
    public float yPos;
    public float zPos;

    public float transitionTime = 1.5f;

    private GameObject player;
    private PlayerController playerController;
    private GameObject sword;
    public GameObject whitescreen;
    private new GameObject camera;
    Animator animator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        sword = GameObject.FindGameObjectWithTag("FloatingSword");
        camera = GameObject.FindGameObjectWithTag("Camera");
        animator = whitescreen.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        animator.SetTrigger("FadeIn");
        playerController.animator.SetBool("isDashing", false);
        playerController.animator.SetBool("isMoving", false);
        playerController.LockMovement();
        yield return new WaitForSeconds(1.5f);

        player.transform.position = new Vector2(xPos, yPos);
        sword.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
        camera.transform.position = player.transform.position;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        animator.SetTrigger("FadeOut");
        playerController.animator.SetBool("isWakingUp", true); // Unlocks movement
        yield return new WaitForSeconds(1.5f);
    }
}
