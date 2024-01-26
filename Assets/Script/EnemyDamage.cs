using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private ResourceBar healthBar;
    [SerializeField] private ResourceBar toleranceBar;
    [SerializeField] private Rigidbody2D rb;

    public float speedMultiplier = 1f;
    public float health;
    public float maxHealth = 60f;
    public float tolerance;
    public float maxTolerance = 20f;
    private float gravity;
    public bool broken;
    public float[] resistences = new float[4] { 0.1f, 0.2f, 0.2f, 1.5f };
    public float[] originalresistences;
    public float[] brokenresistenes = new float[4] { 1f, 1f, 1f, 2f };
    [field: SerializeField]
    public UnityEvent<bool> OnDamaged { get; set; }
    [field: SerializeField]
    public UnityEvent<bool> OnDead { get; set; }

    void Start()
    {
        EventSystem.current.onDeath += OnDeath;
        originalresistences = resistences;
        gravity = rb.gravityScale;
        broken = false;
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
        if (health <= 0)
        {
            OnDead.Invoke(true);
        }
        else if (tolerance <= 0)
        {
            resistences = brokenresistenes;
            OnDamaged.Invoke(true);
        }
        if(knockX >  0 || knockY > 0)
        {
            rb.velocity = new Vector2(resistences[0] * knockX, resistences[0] * knockY);
        }
    }

    public void RemovedDamaged()
    {
        OnDamaged.Invoke(false);
        broken = true;
    }

    public void RemovedDeath()
    {
        OnDead.Invoke(false);
        enemy.SetActive(false);
    }

    public void OnDeath()
    {
        OnDead.Invoke(false);
        enemy.transform.GetChild(0).transform.position = spawnPoint.position ;
        health = maxHealth;
        rb.gravityScale = gravity;
        healthBar.gameObject.SetActive(true);
        healthBar.UpdateResourceBar(health, maxHealth);
        broken = false;
        tolerance = maxTolerance;
        resistences = originalresistences;
        toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
        enemy.SetActive(true);
    }
}
