using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGridSwordAttack : MonoBehaviour
{
    Transform player;

    [SerializeField] private float speed = 20.0f;

    private float cooldownTimer = 6.0f;

    private float attackTimer = 4.0f;

    private float range = 10.0f;

    [SerializeField] private GameObject warningVertical;
    [SerializeField] private GameObject warningHorizontal;
    [SerializeField] private GameObject projectileSword;

    private GameObject verticalWarning1;
    private GameObject verticalWarning2;
    private GameObject verticalWarning3;
    private GameObject verticalWarning4;
    private GameObject verticalWarning5;
    private GameObject verticalWarning6;

    private GameObject horizontalWarning1;
    private GameObject horizontalWarning2;
    private GameObject horizontalWarning3;
    private GameObject horizontalWarning4;
    private GameObject horizontalWarning5;
    private GameObject horizontalWarning6;

    private GameObject verticalSword1;
    private GameObject verticalSword2;
    private GameObject verticalSword3;
    private GameObject verticalSword4;
    private GameObject verticalSword5;
    private GameObject verticalSword6;

    private GameObject horizontalSword1;
    private GameObject horizontalSword2;
    private GameObject horizontalSword3;
    private GameObject horizontalSword4;
    private GameObject horizontalSword5;
    private GameObject horizontalSword6;

    bool enableAttack = false;

    bool createWarnings = true;
    bool createSwords = true;

    Vector3 currentPlayerPosition;

    Vector3 moveX = new Vector3(1, 0, 0);
    Vector3 moveY = new Vector3(0, -1, 0);

    Boss boss;

    private int bossPhase;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        boss = GetComponent<Boss>();

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
            bossPhase = boss.phase;

        }

        if (enableAttack)
        {
            attackTimer -= Time.deltaTime;
            
            currentPlayerPosition = player.position;

            if (createWarnings)
            {
                createWarnings = false;
                verticalWarning1 = GameObject.Instantiate(warningVertical, new Vector3(currentPlayerPosition.x + Random.Range(-range, range), currentPlayerPosition.y, currentPlayerPosition.z), warningVertical.transform.rotation);
                verticalWarning2 = GameObject.Instantiate(warningVertical, new Vector3(currentPlayerPosition.x + Random.Range(-range, range), currentPlayerPosition.y, currentPlayerPosition.z), warningVertical.transform.rotation);

                horizontalWarning1 = GameObject.Instantiate(warningHorizontal, new Vector3(currentPlayerPosition.x, currentPlayerPosition.y + Random.Range(-range, range), currentPlayerPosition.z), warningHorizontal.transform.rotation);
                horizontalWarning2 = GameObject.Instantiate(warningHorizontal, new Vector3(currentPlayerPosition.x, currentPlayerPosition.y + Random.Range(-range, range), currentPlayerPosition.z), warningHorizontal.transform.rotation);

                if(bossPhase >= 2)
                {
                    verticalWarning3 = GameObject.Instantiate(warningVertical, new Vector3(currentPlayerPosition.x + Random.Range(-range, range), currentPlayerPosition.y, currentPlayerPosition.z), warningVertical.transform.rotation);
                    verticalWarning4 = GameObject.Instantiate(warningVertical, new Vector3(currentPlayerPosition.x + Random.Range(-range, range), currentPlayerPosition.y, currentPlayerPosition.z), warningVertical.transform.rotation);

                    horizontalWarning3 = GameObject.Instantiate(warningHorizontal, new Vector3(currentPlayerPosition.x, currentPlayerPosition.y + Random.Range(-range, range), currentPlayerPosition.z), warningHorizontal.transform.rotation);
                    horizontalWarning4 = GameObject.Instantiate(warningHorizontal, new Vector3(currentPlayerPosition.x, currentPlayerPosition.y + Random.Range(-range, range), currentPlayerPosition.z), warningHorizontal.transform.rotation);

                    if(bossPhase == 3)
                    {
                        verticalWarning5 = GameObject.Instantiate(warningVertical, new Vector3(currentPlayerPosition.x + Random.Range(-range, range), currentPlayerPosition.y, currentPlayerPosition.z), warningVertical.transform.rotation);
                        verticalWarning6 = GameObject.Instantiate(warningVertical, new Vector3(currentPlayerPosition.x + Random.Range(-range, range), currentPlayerPosition.y, currentPlayerPosition.z), warningVertical.transform.rotation);

                        horizontalWarning5 = GameObject.Instantiate(warningHorizontal, new Vector3(currentPlayerPosition.x, currentPlayerPosition.y + Random.Range(-range, range), currentPlayerPosition.z), warningHorizontal.transform.rotation);
                        horizontalWarning6 = GameObject.Instantiate(warningHorizontal, new Vector3(currentPlayerPosition.x, currentPlayerPosition.y + Random.Range(-range, range), currentPlayerPosition.z), warningHorizontal.transform.rotation);

                    }

                }

            }

            if (createSwords && attackTimer <= 3.0f)
            {
                createSwords = false;
                
                //projectileSword.transform.rotation = Quaternion.Euler(0, 0, 225);
                verticalSword1 = GameObject.Instantiate(projectileSword, new Vector3(verticalWarning1.transform.position.x, verticalWarning1.transform.position.y + 10, verticalWarning1.transform.position.z), Quaternion.Euler(0, 0, 225));
                verticalSword2 = GameObject.Instantiate(projectileSword, new Vector3(verticalWarning2.transform.position.x, verticalWarning2.transform.position.y + 10, verticalWarning2.transform.position.z), Quaternion.Euler(0, 0, 225));
                
                //projectileSword.transform.rotation = Quaternion.Euler(0, 0, 315);
                horizontalSword1 = GameObject.Instantiate(projectileSword, new Vector3(horizontalWarning1.transform.position.x - 10, horizontalWarning1.transform.position.y, horizontalWarning1.transform.position.z), Quaternion.Euler(0, 0, 315));
                horizontalSword2 = GameObject.Instantiate(projectileSword, new Vector3(horizontalWarning2.transform.position.x - 10, horizontalWarning2.transform.position.y, horizontalWarning2.transform.position.z), Quaternion.Euler(0, 0, 315));

                Destroy(verticalWarning1);
                Destroy(verticalWarning2);
                Destroy(horizontalWarning1);
                Destroy(horizontalWarning2);

                if (bossPhase >= 2)
                {
                    //projectileSword.transform.rotation = Quaternion.Euler(0, 0, 225);
                    verticalSword3 = GameObject.Instantiate(projectileSword, new Vector3(verticalWarning3.transform.position.x, verticalWarning3.transform.position.y + 10, verticalWarning3.transform.position.z), Quaternion.Euler(0, 0, 225));
                    verticalSword4 = GameObject.Instantiate(projectileSword, new Vector3(verticalWarning4.transform.position.x, verticalWarning4.transform.position.y + 10, verticalWarning4.transform.position.z), Quaternion.Euler(0, 0, 225));

                    //projectileSword.transform.rotation = Quaternion.Euler(0, 0, 315);
                    horizontalSword3 = GameObject.Instantiate(projectileSword, new Vector3(horizontalWarning3.transform.position.x - 10, horizontalWarning3.transform.position.y, horizontalWarning3.transform.position.z), Quaternion.Euler(0, 0, 315));
                    horizontalSword4 = GameObject.Instantiate(projectileSword, new Vector3(horizontalWarning4.transform.position.x - 10, horizontalWarning4.transform.position.y, horizontalWarning4.transform.position.z), Quaternion.Euler(0, 0, 315));

                    Destroy(verticalWarning3);
                    Destroy(verticalWarning4);
                    Destroy(horizontalWarning3);
                    Destroy(horizontalWarning4);

                    if (bossPhase == 3)
                    {
                        //projectileSword.transform.rotation = Quaternion.Euler(0, 0, 225);
                        verticalSword5 = GameObject.Instantiate(projectileSword, new Vector3(verticalWarning5.transform.position.x, verticalWarning5.transform.position.y + 10, verticalWarning5.transform.position.z), Quaternion.Euler(0, 0, 225));
                        verticalSword6 = GameObject.Instantiate(projectileSword, new Vector3(verticalWarning6.transform.position.x, verticalWarning6.transform.position.y + 10, verticalWarning6.transform.position.z), Quaternion.Euler(0, 0, 225));

                        //projectileSword.transform.rotation = Quaternion.Euler(0, 0, 315);
                        horizontalSword5 = GameObject.Instantiate(projectileSword, new Vector3(horizontalWarning5.transform.position.x - 10, horizontalWarning5.transform.position.y, horizontalWarning5.transform.position.z), Quaternion.Euler(0, 0, 315));
                        horizontalSword6 = GameObject.Instantiate(projectileSword, new Vector3(horizontalWarning6.transform.position.x - 10, horizontalWarning6.transform.position.y, horizontalWarning6.transform.position.z), Quaternion.Euler(0, 0, 315));

                        Destroy(verticalWarning5);
                        Destroy(verticalWarning6);
                        Destroy(horizontalWarning5);
                        Destroy(horizontalWarning6);

                    }
                }

               

            }

            if (!createSwords && attackTimer <= 3.0f && attackTimer > 0.0f)
            {
                verticalSword1.transform.position += moveY * speed * Time.deltaTime;
                verticalSword2.transform.position += moveY * speed * Time.deltaTime;

                horizontalSword1.transform.position += moveX * speed * Time.deltaTime;
                horizontalSword2.transform.position += moveX * speed * Time.deltaTime;

                if(bossPhase >= 2)
                {
                    verticalSword3.transform.position += moveY * speed * Time.deltaTime;
                    verticalSword4.transform.position += moveY * speed * Time.deltaTime;

                    horizontalSword3.transform.position += moveX * speed * Time.deltaTime;
                    horizontalSword4.transform.position += moveX * speed * Time.deltaTime;

                    if (bossPhase == 3)
                    {
                        verticalSword5.transform.position += moveY * speed * Time.deltaTime;
                        verticalSword6.transform.position += moveY * speed * Time.deltaTime;

                        horizontalSword5.transform.position += moveX * speed * Time.deltaTime;
                        horizontalSword6.transform.position += moveX * speed * Time.deltaTime;
                    }
                }


            }

            if (attackTimer <= 0.0f)
            {
                Destroy(verticalSword1);
                Destroy(verticalSword2);
                Destroy(horizontalSword1);
                Destroy(horizontalSword2);
                
                if(bossPhase >= 2)
                {
                    Destroy(verticalSword3);
                    Destroy(verticalSword4);
                    Destroy(horizontalSword3);
                    Destroy(horizontalSword4);

                    if (bossPhase == 3)
                    {
                        Destroy(verticalSword5);
                        Destroy(verticalSword6);
                        Destroy(horizontalSword5);
                        Destroy(horizontalSword6);
                    }
                }

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
