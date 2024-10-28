using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ElementalWallBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Rigidbody2D rb;

    public float health;
    public float maxHealth = 1f;
    private float gravity;
    public float[] resistences = new float[4] { 0f, 0f, 0f, 0f };
    [field: SerializeField]
    public UnityEvent<bool> OnDamaged { get; set; }
    [field: SerializeField]
    public UnityEvent<bool> OnDead { get; set; }
    void Start()
    {
        EventSystem.current.onDeath += OnDeath;
        gravity = rb.gravityScale;
        health = maxHealth;
    }

    public void Damage(int enchatment, float damage)
    {
        health -= resistences[enchatment] * damage;
        if (health <= 0)
        {
            OnDead.Invoke(true);
        }
    }

    public void RemovedDamaged()
    {
        OnDamaged.Invoke(false);
    }

    public void RemovedDeath()
    {
        OnDead.Invoke(false);
        enemy.SetActive(false);
    }

    public void OnDeath()
    {
        OnDead.Invoke(false);
        OnDamaged.Invoke(false);
        health = maxHealth;
        rb.gravityScale = gravity;
        enemy.SetActive(true);
    }
}
