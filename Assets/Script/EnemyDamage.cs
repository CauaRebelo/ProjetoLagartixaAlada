using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private ResourceBar healthBar;
    [SerializeField] private ResourceBar toleranceBar;
    [SerializeField] private Rigidbody2D rb;

    public float health;
    public float maxHealth = 60f;
    public float tolerance;
    public float maxTolerance = 20f;
    public float[] resistences = new float[4] { 0.1f, 0.2f, 0.2f, 1.5f };
    public float[] originalresistences;
    public float[] brokenresistenes = new float[4] { 1f, 1f, 1f, 2f };

    void Start()
    {
        EventSystem.current.onDeath += OnDeath;
        originalresistences = resistences;
        health = maxHealth;
        healthBar.UpdateResourceBar(health, maxHealth);
        tolerance = maxTolerance;
        toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
    }

    public void Damage(int enchatment, float damage, float tolerancedamage, float knockX, float knockY)
    {
        health -= resistences[enchatment] * damage;
        healthBar.UpdateResourceBar(health, maxHealth);
        tolerance -= resistences[enchatment] * tolerancedamage;
        toleranceBar.UpdateResourceBar(tolerance, maxTolerance);

        if (tolerance <= 0)
        {
            resistences = brokenresistenes;
        }
        if (health <= 0)
        {
            enemy.SetActive(false);
        }
        rb.velocity = new Vector2(resistences[0] * knockX, resistences[0] * knockY);
    }

    public void OnDeath()
    {
        enemy.transform.position = spawnPoint.position ;
        health = maxHealth;
        healthBar.UpdateResourceBar(health, maxHealth);
        tolerance = maxTolerance;
        resistences = originalresistences;
        toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
        enemy.SetActive(true);
    }
}
