using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamTutorial : MonoBehaviour
{
    public GameObject newCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("pejis");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("penisis");
            newCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            newCamera.SetActive(false);
        }
    }

}
