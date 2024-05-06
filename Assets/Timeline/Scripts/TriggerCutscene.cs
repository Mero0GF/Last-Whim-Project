using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Animator animator;
    [SerializeField] private PersistentDataSO persistentDataSO;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        GetComponent<SpriteRenderer>().enabled = true;
        if (persistentDataSO.hasSword)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playableDirector.Play();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
