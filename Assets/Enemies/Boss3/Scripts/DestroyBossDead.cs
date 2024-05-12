using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBossDead : MonoBehaviour
{
    [SerializeField] private GameObject Boss;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.FindGameObjectWithTag("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        if(Boss == null)
        {
            Destroy(gameObject);
        }
    }
}
