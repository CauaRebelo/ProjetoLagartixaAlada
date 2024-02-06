using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool falseDeath;
    public float health;
    public float maxHealth = 60f;
    public float tolerance;
    public bool multipleTolerance = false;
    public bool anger = false;
    public int toleranceStage;
    public bool immune = false;
    public bool trueImmune = false;
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
    [field: SerializeField]
    public UnityEvent<int> OnChangeEnchantment { get; set; }

    void Start()
    {
        originalresistences = resistences;
        firsTime = true;
        falseDeath = true;
        EventSystem.current.onDeath += OnDeath;
        EventSystem.current.onPlayerElementeChange += OnPlayerElementeChange;
        gravity = rb.gravityScale;
        broken = false;
        health = maxHealth;
        healthBar.UpdateResourceBar(health, maxHealth);
        tolerance = maxTolerance;
        toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
    }

    public void Damage(int enchatment, float damage, float tolerancedamage, float knockX, float knockY)
    {
        if(trueImmune)
        {
            return;
        }
        if(!immune)
        {
            health -= resistences[enchatment] * damage;
            healthBar.UpdateResourceBar(health, maxHealth);
            if(multipleTolerance && health <= (maxHealth * 50)/100 && firstImmune)
            {
                ChangeElement(3);
                health = (maxHealth * 50) / 100;
                healthBar.UpdateResourceBar(health, maxHealth);
                tolerance = maxTolerance;
                toleranceBar.UpdateResourceBar(health, maxHealth);
                immune = true;
                broken = false;
                firstImmune = false;
                toleranceStage = 3;
                OnDamaged.Invoke(false);
            }
            if(multipleTolerance && health <= 0 && falseDeath)
            {
                StartCoroutine(FakeDeath());
                return;
            }
            else if(multipleTolerance && health <= 0 && !falseDeath)
            {
                trueImmune = true;
                OnDamaged.Invoke(false);
                OnDead.Invoke(true);
                return;
            }
        }
        if(!multipleTolerance)
        {
            tolerance -= resistences[enchatment] * tolerancedamage;
            toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
        }
        else
        {
            if(tolerancedamage == 0)
            {
                tolerancedamage += damage;
            }
            tolerance -= resistences[enchatment] * tolerancedamage;
            toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
            switch(toleranceStage)
            {
                case 3:
                    if (tolerance <= (maxTolerance * 66) / 100)
                    {
                        tolerance = (maxTolerance * 66)/100;
                        toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
                        ChangeElement(2);
                        toleranceStage--;
                    }
                    break;
                case 2:
                    if(tolerance <= (maxTolerance * 33)/100)
                    {
                        tolerance = (maxTolerance * 33)/100;
                        toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
                        ChangeElement(1);
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
        if(!multipleTolerance)
        {
            enemy.transform.GetChild(0).transform.position = spawnPoint.position;
        }
        health = maxHealth;
        rb.gravityScale = gravity;
        healthBar.gameObject.SetActive(true);
        healthBar.UpdateResourceBar(health, maxHealth);
        broken = false;
        tolerance = maxTolerance;
        resistences = originalresistences;
        Debug.Log("Dying");
        toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
        if(!multipleTolerance)
        {
            enemy.SetActive(true);
        }
        else
        {
            ChangeElement(3);
            firstImmune = true;
            trueImmune = false;
            immune = true;
            falseDeath = true;
            toleranceStage = 3;
            this.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    public void ChangeElement(int val)
    {
        switch(val)
        {
            case 1:
                resistences[0] = 0f;
                resistences[2] = 2f;
                resistences[1] = 0.2f;
                resistences[3] = 0.2f;
                break;
            case 2:
                resistences[0] = 0f;
                resistences[2] = 0.2f;
                resistences[1] = 0.2f;
                resistences[3] = 2f;
                break;
            case 3:
                resistences[0] = 0f;
                resistences[2] = 0.2f;
                resistences[1] = 2f;
                resistences[3] = 0.2f;
                break;
        }
        OnChangeEnchantment?.Invoke(val - 1);
        EventSystem.current.BossElementeChange();
    }

    public void OnPlayerElementeChange()
    {
        if(multipleTolerance && anger)
        {
            int element = GameObject.Find("/MainPlayer/Player").GetComponent<PlayerMovement>().enchantment;
        }
    }

    IEnumerator FakeDeath()
    {
        trueImmune = true;
        OnDead.Invoke(true);
        OnDamaged.Invoke(false);
        health = 0;
        for(int i = 0; health <= (maxHealth/2); i++)
        {
            health += maxHealth / 5;
            healthBar.UpdateResourceBar(health, maxHealth);
            yield return new WaitForSeconds(0.1f);
        }
        trueImmune = false;
        OnDead.Invoke(false);
        ChangeElement(3);
        health = (maxHealth * 50) / 100;
        healthBar.UpdateResourceBar(health, maxHealth);
        tolerance = maxTolerance;
        toleranceBar.UpdateResourceBar(tolerance, maxTolerance);
        immune = true;
        broken = false;
        firstImmune = false;
        toleranceStage = 3;
        falseDeath = false;
    }
}
