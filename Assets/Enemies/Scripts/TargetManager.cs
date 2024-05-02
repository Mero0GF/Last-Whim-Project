using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour, IData
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

   
    private bool spawn = true;
    private int hp = 1;
    public DamageManager DamageManager;
    public FloatingSword FloatingSword;

    private void FixedUpdate()
    {
        if (hp <= 0)
        {
            spawn = false;
            Destroy(gameObject);
            Debug.Log("Enemy Killed " + spawn);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FloatingSword") && (FloatingSword.state == FloatingSword.State.Attack))
        {
            hp = DamageManager.takeDamage(hp);
        }
    }

    public void LoadData(GameData data)
    {
        data.spawnEnemy.TryGetValue(id, out spawn);
        if (!spawn)
        {
            Destroy(gameObject);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.spawnEnemy.ContainsKey(id))
        {
            data.spawnEnemy.Remove(id);
        }
        data.spawnEnemy.Add(id, spawn);
    }
}
