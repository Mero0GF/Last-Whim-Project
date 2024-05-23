using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{

    public Transform player;

    GameObject playerSword;

    FloatingSword floatingSword;

    [SerializeField] private GameObject enemySword;

    public int maxHealth = 3;

    public int health;

    [SerializeField] private float cooldownTime = 2.0f;

    private float damageCooldown = 0.0f;

    [SerializeField] PersistentDataSO persistentDataSO;

    private GameObject[] sceneBarriers;

    private PlayableDirector playableDirector;

    private GameObject sword1;
    private GameObject sword2;
    private GameObject sword3;
    private GameObject sword4;
    private GameObject sword5;
    private GameObject sword6;

    //private float targetTime = 5.0f;

    //public bool swordAttack = false;

    public bool isFlipped = false;

    public int phase = 1;



    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
        sceneBarriers = new GameObject[1];
        sceneBarriers = GameObject.FindGameObjectsWithTag("Barrier");
        if (persistentDataSO.lastBossDone)
        {
            for (int i = 0; i < sceneBarriers.Length; i++)
            {
                sceneBarriers[i].SetActive(false);
            }
            Destroy(gameObject);
        }

        health = maxHealth;

        playerSword = GameObject.FindGameObjectWithTag("FloatingSword");
        floatingSword = playerSword.GetComponent<FloatingSword>();

        sword1 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x - 1, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 180));
        sword1.GetComponent<SwordMovement>().swordNum = 1;
        sword1.transform.SetParent(transform);
        sword2 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x + 1, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 270));
        sword2.GetComponent<SwordMovement>().swordNum = 2;
        sword2.transform.SetParent(transform);
        
    }

    
    void Update()
    {
        damageCooldown -= Time.deltaTime;

        if (health <= (maxHealth*2)/3 && health > maxHealth/3 && phase == 1)
        {
            phase = 2;
            Destroy(sword1);
            Destroy(sword2);

            sword1 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x - 3, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 180));
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

        else if (health <= maxHealth/3 && health > 0 && phase == 2)
        {
            phase = 3;
            Destroy(sword1);
            Destroy(sword2);
            Destroy(sword3);
            Destroy(sword4);

            sword1 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x - 5, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 180));
            sword1.GetComponent<SwordMovement>().swordNum = 1;
            sword1.transform.SetParent(transform);
            sword2 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x - 3, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 180));
            sword2.GetComponent<SwordMovement>().swordNum = 2;
            sword2.transform.SetParent(transform);
            sword3 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x - 1, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 180));
            sword3.GetComponent<SwordMovement>().swordNum = 3;
            sword3.transform.SetParent(transform);
            sword4 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x + 1, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 270));
            sword4.GetComponent<SwordMovement>().swordNum = 4;
            sword4.transform.SetParent(transform);
            sword5 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x + 3, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 270));
            sword5.GetComponent<SwordMovement>().swordNum = 5;
            sword5.transform.SetParent(transform);
            sword6 = GameObject.Instantiate(enemySword, new Vector3(transform.position.x + 5, transform.position.y - 2, transform.position.z), Quaternion.Euler(0, 0, 270));
            sword6.GetComponent<SwordMovement>().swordNum = 6;
            sword6.transform.SetParent(transform);

        }

        else if (health <= 0)
        {
            for (int i = 0; i < sceneBarriers.Length; i++)
            {
                sceneBarriers[i].SetActive(false);
            }
            persistentDataSO.LastBossKilled();
            GameObject cutsceneObj = GameObject.FindGameObjectWithTag("Finish");
            cutsceneObj.GetComponent<Collider2D>().enabled = true;
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
        
        if (collider.CompareTag("FloatingSword") && damageCooldown <= 0.0f && floatingSword.state == FloatingSword.State.Attack)
        {
            health -= 1;
            damageCooldown = cooldownTime;
        }
        
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
