using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerWakeupCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private PersistentDataSO persistentDataSO;
    [SerializeField] private GameObject blackscreen;

    private void Awake()
    {
        if (persistentDataSO.beachCutscenePlayed)
        {
            blackscreen.GetComponent<SpriteRenderer>().enabled = false;
            blackscreen.GetComponent<Animator>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            playableDirector.Play();
        }
    }
}
