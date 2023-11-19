using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform spawnPoint;

    public float health;
    public float maxHealth = 60f;
    public float tolerance;
    public float maxTolerance = 20f;
    public float[] resistences = new float[4] { 0.1f, 0.2f, 0.2f, 1.5f };
    public float[] brokenresistenes = new float[4] { 1f, 1f, 1f, 2f };

    void Start()
    {
        EventSystem.current.onDeath += OnDeath;
        health = maxHealth;
        tolerance = maxTolerance;
    }

    void Damage(int enchatment, float damage, float tolerancedamage)
    {
        health -= resistences[enchatment] * damage;
        tolerance -= resistences[enchatment] * tolerancedamage;
        
        if(tolerance <= 0)
        {
            resistences = brokenresistenes;
        }
        if (health <= 0)
        {
            enemy.SetActive(false);
        }
    }

    public void OnDeath()
    {
        enemy.transform.position = spawnPoint.position ;
        health = maxHealth;
        tolerance = maxTolerance;
        enemy.SetActive(true);
    }
}
