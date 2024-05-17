using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRock : MonoBehaviour
{
    
    private GameObject player;
    public GameObject warningPrefab;
    private GameObject warning;
    private Vector3 destroyPosOffset = new Vector3(0,0.7f);
    private Vector3 destroyPos = Vector3.zero;
    private new Collider2D collider;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        destroyPos = player.transform.position - destroyPosOffset;
        warning = Instantiate(warningPrefab, destroyPos, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        if (transform.position.y <= destroyPos.y + destroyPosOffset.y)
        {
            transform.position = destroyPos + destroyPosOffset;
            collider.enabled = true;
            StartCoroutine(destroyTimer());
        }
    }

    IEnumerator destroyTimer()
    {
        gameObject.isStatic = true;
        yield return new WaitForSeconds(0.2f);
        Destroy(warning);
        Destroy(gameObject);
    }
}
