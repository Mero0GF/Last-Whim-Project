using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BossManager")]
public class BossManager : ScriptableObject
{
    public float speed;
    public float accel;
    public float deaccel;
    public float invincibilityFrameMax = 300;
    public float invincibilityFrameMin = 0;

    public GameObject boss;
    public BossController bossController;

}
