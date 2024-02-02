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

    public GameObject soul;

    public float speedMultiplier = 1f;
    public bool firsTime;
    public float health;
    public float maxHealth = 60f;
    public float tolerance;
    public bool multipleTolerance = false;
    public int toleranceStage;
    public bool immune = false;
    public bool firstImmune = false;
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
        originalresistences = resistences;
        firsTime = true;
        EventSystem.current.onDeath += OnDeath;
        gravity = rb.gravityScale;
        broken = false;
        health = maxHealth;
        healthBar.UpdateResourceBar(health, maxHealth);
        tolerance = maxTolerance;
        toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
    }

    public void Damage(int enchatment, float damage, float tolerancedamage, float knockX, float knockY)
    {
        if(!immune)
        {
            health -= resistences[enchatment] * damage;
            healthBar.UpdateResourceBar(health, maxHealth);
            if(multipleTolerance && health <= (maxHealth * 50)/100 && firstImmune)
            {
                tolerance = maxTolerance;
                resistences = originalresistences;
                immune = true;
                broken = false;
                firstImmune = false;
                toleranceStage = 3;
            }
        }
        if(!multipleTolerance)
        {
            tolerance -= resistences[enchatment] * tolerancedamage;
            toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
        }
        else
        {
            tolerance -= resistences[enchatment] * tolerancedamage;
            toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
            switch(toleranceStage)
            {
                case 3:
                    if (tolerance <= (maxTolerance * 66) / 100)
                    {
                        float oldResistence = resistences[2];
                        resistences[2] = resistences[1];
                        resistences[1] = oldResistence;
                        toleranceStage--;
                    }
                    break;
                case 2:
                    if(tolerance <= (maxTolerance * 33)/100)
                    {
                        float oldResistence = resistences[3];
                        resistences[3] = resistences[2];
                        resistences[2] = oldResistence;
                        toleranceStage--;
                    }
                    break;
            }
        }
        if (health <= 0)
        {
            OnDead.Invoke(true);
        }
        else if (tolerance <= 0)
        {
            resistences = brokenresistenes;
            immune = false;
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
        if(firsTime)
        {
            firsTime = false;
            Instantiate(soul, transform.position, Quaternion.identity);
        }
        OnDead.Invoke(false);
        enemy.SetActive(false);
    }

    public void OnDeath()
    {
        OnDead.Invoke(false);
        OnDamaged.Invoke(false);
        enemy.transform.GetChild(0).transform.position = spawnPoint.position ;
        health = maxHealth;
        rb.gravityScale = gravity;
        if(multipleTolerance)
        {
            firstImmune = true;
            immune = true;
            toleranceStage = 3;
        }
        healthBar.gameObject.SetActive(true);
        healthBar.UpdateResourceBar(health, maxHealth);
        broken = false;
        tolerance = maxTolerance;
        resistences = originalresistences;
        Debug.Log("Dying");
        toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
        enemy.SetActive(true);
    }
}
