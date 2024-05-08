using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelManager")]
public class LevelManager : ScriptableObject
{
    public static LevelManager ActiveLevels {  get; set; }
}
