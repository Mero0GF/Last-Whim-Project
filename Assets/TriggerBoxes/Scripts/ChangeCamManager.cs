using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCamManager : MonoBehaviour
{
    float newSize = 9.5f;
    float originalSize = 6.5f;

    private new GameObject camera;
    private CinemachineVirtualCamera cameraSettings;
    private GameObject camFocus;

    private void Start()
    {
        camFocus = GameObject.FindGameObjectWithTag("CamFocus");
        camera = GameObject.FindGameObjectWithTag("Camera");
        cameraSettings = camera.GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraSettings.m_Lens.OrthographicSize = newSize;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraSettings.m_Lens.OrthographicSize = originalSize;
        }
    }
}
