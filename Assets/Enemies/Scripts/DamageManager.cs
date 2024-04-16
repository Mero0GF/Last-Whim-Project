using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [SerializeField] private FloatingSword floatingSword;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("tag: " + collision.tag);
        if ((floatingSword.state == FloatingSword.State.Attack) && (collision.CompareTag("FloatingSword")))
        {
            floatingSword.speed = floatingSword.speed / 2;
            Destroy(gameObject);
        }
    }
}
