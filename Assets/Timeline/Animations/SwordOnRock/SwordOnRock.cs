using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordOnRock : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartAnimation()
    {
        animator.SetTrigger("Play");
    }

}
