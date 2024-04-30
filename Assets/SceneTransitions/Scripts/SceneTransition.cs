using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private PlayerController playerController;
    private GameObject player;

    public Animator animator;

    public string scene;
    float transitionTime = 1f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.LockMovement();
            playerController.dodgeSpd = 0;
            LoadScene(scene);
        }
    }

    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }
    
    IEnumerator LoadLevel(string sceneName)
    {
        animator.SetTrigger("Start");
        playerController.animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
