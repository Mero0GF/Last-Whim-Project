using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public Transform player;

    [SerializeField] private GameObject enemySword;

    public int health = 3;

    [SerializeField] private float cooldownTime = 2.0f;

    private float damageCooldown = 0.0f;

    private GameObject sword1;
    private GameObject sword2;
    private GameObject sword3;
    private GameObject sword4;

    //private float targetTime = 5.0f;

    //public bool swordAttack = false;

    public bool isFlipped = false;

    private void Start()
    {
        sword1 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x-3, transform.position.y-2, transform.position.z), Quaternion.Euler(0, 0, 180));
        sword1.GetComponent<SwordMovement>().swordNum = 1;
        sword1.transform.SetParent(transform);
        sword2 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x - 1, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 180));
        sword2.GetComponent<SwordMovement>().swordNum = 2;
        sword2.transform.SetParent(transform);
        sword3 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x + 1, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 270));
        sword3.GetComponent<SwordMovement>().swordNum = 3;
        sword3.transform.SetParent(transform);
        sword4 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x + 3, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 270));
        sword4.GetComponent<SwordMovement>().swordNum = 4;
        sword4.transform.SetParent(transform);
    }

    
    void Update()
    {
        damageCooldown -= Time.deltaTime;

        if (health == 0)
        {
            Destroy(sword1);
            Destroy(sword2);
            Destroy(sword3);
            Destroy(sword4);
            Destroy(gameObject);

        }
    }
    

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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("FloatingSword") && damageCooldown <= 0.0f)
        {
            health -= 1;
            damageCooldown = cooldownTime;
        }
        
    }

}
