using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{
    public int swordNum;
    
    [SerializeField] private float speed = 12.0f;

    private float swordModeTimer = 10.0f;

    private float cooldownTimer = 6.0f;

    private float switchMode;

    private Vector3 normalizeDirection;

    Vector3 targetDirection;

    Quaternion initialRotation;

    Transform target;

    [SerializeField] private GameObject bossGameObject;
    Boss boss;

    PointSword pointSword;

    private int bossPhase;

    bool enableAttack = false;

    //Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        switchMode = swordModeTimer - swordNum - 2.0f;


        target = GameObject.FindGameObjectWithTag("Player").transform;
        bossGameObject = GameObject.FindGameObjectWithTag("Boss");
        boss = bossGameObject.GetComponent<Boss>();
        
        pointSword = GetComponent<PointSword>();
        initialRotation = transform.rotation;
        //initialPosition = bossGameObject.transform.position - transform.position;
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
            swordModeTimer -= Time.deltaTime;

            if (swordModeTimer >= switchMode)
            {
                pointSword.followPlayer = true;
                targetDirection = target.position;
                normalizeDirection = (targetDirection - transform.position).normalized;
            }

            else if (swordModeTimer < switchMode && swordModeTimer > 0.0f)
            {
                transform.SetParent(null);
                pointSword.followPlayer = false;
                SwordFly();
            }

            else
            {
                enableAttack = false;
            }
        }

        else if (!enableAttack && swordModeTimer <= 0.0f)
        {
            transform.rotation = initialRotation;
            transform.position = new Vector3(bossGameObject.transform.position.x + 2 * swordNum - 1 - 2 * bossPhase, bossGameObject.transform.position.y - 2, bossGameObject.transform.position.z);
            transform.SetParent(bossGameObject.transform);

            swordModeTimer = 10.0f;
        }



    }

    public void SwordFly()
    {
        transform.position += normalizeDirection * speed * Time.deltaTime;
    }


}
