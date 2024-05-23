using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneAux : MonoBehaviour
{
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            GetComponent<PlayableDirector>().Play();
        }
    }
}
