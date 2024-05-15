using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTest : MonoBehaviour
{

    private GameObject[] sceneBarriers;
    [SerializeField] PersistentDataSO persistentDataSO;

    void Start()
    {
        if (persistentDataSO.firstBossDone)
        {
            sceneBarriers = new GameObject[2];
            sceneBarriers = GameObject.FindGameObjectsWithTag("Barrier");
            for (int i = 0; i < sceneBarriers.Length; i++)
            {
                sceneBarriers[i].SetActive(false);
            }
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (persistentDataSO.firstBossDone)
        {
            for (int i = 0; i < sceneBarriers.Length; i++)
            {
                sceneBarriers[i].SetActive(false);
            }
            Destroy(gameObject);
        }
    }

}
