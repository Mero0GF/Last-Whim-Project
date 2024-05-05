using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public Transform player;

    [SerializeField] private GameObject enemySword;

    public GameObject sword1;
    public GameObject sword2;
    public GameObject sword3;
    public GameObject sword4;

    //private float targetTime = 5.0f;

    //public bool swordAttack = false;

    public bool isFlipped = false;

    private void Start()
    {
        sword1 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x-3, transform.position.y-2, transform.position.z), enemySword.transform.rotation);
        sword1.GetComponent<SwordMovement>().swordNum = 1;
        sword1.transform.SetParent(transform);
        sword2 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x - 1, transform.position.y - 2, transform.position.z), enemySword.transform.rotation);
        sword2.GetComponent<SwordMovement>().swordNum = 2;
        sword2.transform.SetParent(transform);
        sword3 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x + 1, transform.position.y - 2, transform.position.z), enemySword.transform.rotation);
        sword3.GetComponent<SwordMovement>().swordNum = 3;
        sword3.transform.SetParent(transform);
        sword4 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x + 3, transform.position.y - 2, transform.position.z), enemySword.transform.rotation);
        sword4.GetComponent<SwordMovement>().swordNum = 4;
        sword4.transform.SetParent(transform);
    }

    /*
    void Update()
    {
        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            timerEnded();
        }

        void timerEnded()
        {
            swordAttack = true;
        }
    }
    */

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;

        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }

        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

}
