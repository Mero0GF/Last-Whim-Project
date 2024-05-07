using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGridSwordAttack : MonoBehaviour
{
    Transform player;

    [SerializeField] private float speed = 20.0f;

    private float cooldownTimer = 6.0f;

    private float attackTimer = 4.0f;

    [SerializeField] private GameObject warningVertical;
    [SerializeField] private GameObject warningHorizontal;
    [SerializeField] private GameObject projectileSword;

    private GameObject verticalWarning1;
    private GameObject verticalWarning2;

    private GameObject horizontalWarning1;
    private GameObject horizontalWarning2;

    private GameObject verticalSword1;
    private GameObject verticalSword2;

    private GameObject horizontalSword1;
    private GameObject horizontalSword2;

    bool enableAttack = false;

    bool createWarnings = true;
    bool createSwords = true;

    Vector3 currentPlayerPosition;

    Vector3 moveX = new Vector3(1, 0, 0);
    Vector3 moveY = new Vector3(0, -1, 0);


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (!enableAttack && cooldownTimer > 0.0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        else if (!enableAttack && cooldownTimer <= 0.0f)
        {
            enableAttack = true;
            cooldownTimer = 6.0f;
        }

        if (enableAttack)
        {
            attackTimer -= Time.deltaTime;
            
            currentPlayerPosition = player.position;

            if (createWarnings)
            {
                createWarnings = false;
                verticalWarning1 = GameObject.Instantiate(warningVertical, new Vector3(currentPlayerPosition.x + Random.Range(-10.0f, 10.0f), currentPlayerPosition.y, currentPlayerPosition.z), warningVertical.transform.rotation);
                verticalWarning2 = GameObject.Instantiate(warningVertical, new Vector3(currentPlayerPosition.x + Random.Range(-10.0f, 10.0f), currentPlayerPosition.y, currentPlayerPosition.z), warningVertical.transform.rotation);

                horizontalWarning1 = GameObject.Instantiate(warningHorizontal, new Vector3(currentPlayerPosition.x, currentPlayerPosition.y + Random.Range(-10.0f, 10.0f), currentPlayerPosition.z), warningHorizontal.transform.rotation);
                horizontalWarning2 = GameObject.Instantiate(warningHorizontal, new Vector3(currentPlayerPosition.x, currentPlayerPosition.y + Random.Range(-10.0f, 10.0f), currentPlayerPosition.z), warningHorizontal.transform.rotation);
            }

            if (createSwords && attackTimer <= 3.0f)
            {
                createSwords = false;
                verticalSword1 = GameObject.Instantiate(projectileSword, new Vector3(verticalWarning1.transform.position.x, verticalWarning1.transform.position.y + 10, verticalWarning1.transform.position.z), projectileSword.transform.rotation);
                verticalSword2 = GameObject.Instantiate(projectileSword, new Vector3(verticalWarning2.transform.position.x, verticalWarning2.transform.position.y + 10, verticalWarning2.transform.position.z), projectileSword.transform.rotation);

                horizontalSword1 = GameObject.Instantiate(projectileSword, new Vector3(horizontalWarning1.transform.position.x - 10, horizontalWarning1.transform.position.y, horizontalWarning1.transform.position.z), projectileSword.transform.rotation);
                horizontalSword2 = GameObject.Instantiate(projectileSword, new Vector3(horizontalWarning2.transform.position.x - 10, horizontalWarning2.transform.position.y, horizontalWarning2.transform.position.z), projectileSword.transform.rotation);

                Destroy(verticalWarning1);
                Destroy(verticalWarning2);
                Destroy(horizontalWarning1);
                Destroy(horizontalWarning2);

            }

            if (!createSwords && attackTimer <= 3.0f && attackTimer > 0.0f)
            {
                verticalSword1.transform.position += moveY * speed * Time.deltaTime;
                verticalSword2.transform.position += moveY * speed * Time.deltaTime;

                horizontalSword1.transform.position += moveX * speed * Time.deltaTime;
                horizontalSword2.transform.position += moveX * speed * Time.deltaTime;


            }

            if (attackTimer <= 0.0f)
            {
                Destroy(verticalSword1);
                Destroy(verticalSword2);
                Destroy(horizontalSword1);
                Destroy(horizontalSword2);
                
                createSwords = true;
                createWarnings = true;
                enableAttack = false;
            }
        }

        else if (!enableAttack && attackTimer <= 0.0f)
        {
            attackTimer = 4.0f;


        }
    }
}
