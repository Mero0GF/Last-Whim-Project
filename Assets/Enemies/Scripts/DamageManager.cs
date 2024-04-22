using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{

    public int takeDamage(int hp)
    {
        hp = hp - 1;
        return hp;
    }

}
