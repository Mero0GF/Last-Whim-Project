using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private int hp = 1;
    public DamageManager DamageManager;
    public FloatingSword FloatingSword;
    public TutorialManager TutorialManager;

    private void Start()
    {
        DamageManager = GetComponent<DamageManager>();
    }

    private void FixedUpdate()
    {
        if (hp <= 0) 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FloatingSword") && (FloatingSword.state == FloatingSword.State.Attack))
        {
            hp = DamageManager.takeDamage(hp);
            TutorialManager.killed += 1;
        }
    }
}
