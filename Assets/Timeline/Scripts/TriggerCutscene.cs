using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Animator animator;

    private GameObject player;
    private PlayerController playerController;

    //private bool triggerActivated = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            playableDirector.Play();
            GetComponent<Collider2D>().enabled = false;
        }
    }

    //private void FixedUpdate()
    //{
    //    if (triggerActivated)
    //    {
    //        playableDirector.Play();
    //        Destroy(gameObject);
    //    }
    //}

    //public void FadeInActivation()
    //{
    //    animator.SetTrigger("FadeIn");
    //}

    //public void FadeInComplete()
    //{
    //    triggerActivated = true;
    //}
}
