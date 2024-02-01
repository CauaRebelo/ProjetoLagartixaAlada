using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private GameObject player;
    //[SerializeField] private HealthBar healthBar;

    [field: SerializeField]
    public UnityEvent<bool> OnDamage { get; set; }
    [field: SerializeField]
    public UnityEvent<bool> OnDead { get; set; }
    public Transform spawnPoint;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private PlayerReflect playerReflect;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Canvas canvas;

    public bool parrying = false;
    public bool iframe = false;
    public int health;
    public int maxHealth = 5;
    private Color previousColor;
    private float immuneTime = 1f;

    void Start()
    {
        health = maxHealth;
        EventSystem.current.onRespawn += OnRespawn;
        //healthBar.UpdateResourceBar(health, maxHealth);
    }

    public void Damage()
    {
        if (parrying)
        {
            playerReflect.Riposite();
            return;
        }

        if(!iframe)
        {
            health--;
            EventSystem.current.PlayerDamage();
            StartCoroutine(Immune());
            //healthBar.UpdateResourceBar(health, maxHealth);
        }

        if(health <= 0)
        {
            Death();
        }
    }

    public void FallDamage()
    {
        //healthBar.UpdateResourceBar(health, maxHealth);
        RemovedDeath();
    }

    public void Death()
    {
        iframe = true;
        playerMovement.isAbleToAct = false;
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        OnDamage?.Invoke(false);
        OnDead?.Invoke(true);
    }

    public void RemovedDeath()
    {
        playerMovement.sprite.color *= new Color (1f, 1f, 1f, 0f);
        playerMovement.sprite.color += new Color (0, 0, 0, 1f);
        OnDamage?.Invoke(false);
        OnDead.Invoke(false);
        parrying = false;
        canvas.gameObject.SetActive(true);
        iframe = false;
        EventSystem.current.Death();
        playerMovement.canAttack = true;
        playerMovement.canMove = true;
        playerMovement.isAbleToAct = true;
        playerMovement.canDash = true;
        playerMovement.spamAttack = false;
        playerMovement.spamLongAttack = false;
        playerMovement.spamVerticalAttack = false;
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 5f;
        player.SetActive(false);
    }


    public void OnRespawn()
    {
        player.SetActive(true);
        StartCoroutine(GameOver());
    }

    IEnumerator Immune()
    {
        iframe = true;
        playerMovement.OnParry?.Invoke(false);
        playerMovement.OnAttack?.Invoke(false);
        playerMovement.OnLongAttack?.Invoke(false);
        playerMovement.OnVerticalAttack?.Invoke(false);
        OnDamage?.Invoke(true);
        playerMovement.isAbleToAct = false;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        float originalGravity = 5f;
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        yield return new WaitForSeconds(0.3f);
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = originalGravity;
        playerMovement.isAbleToAct = true;
        playerMovement.canMove = true;
        playerMovement.canDash = true;
        OnDamage?.Invoke(false);
        StartCoroutine(Flicker());
        yield return new WaitForSeconds(immuneTime);
        iframe = false;
    }

    IEnumerator Flicker()
    {
        playerMovement.sprite.color *= new Color (1f, 1f, 1f, 0f);
        playerMovement.sprite.color += new Color (0, 0, 0, 1f);
        playerMovement.sprite.color -= new Color (0, 0, 0, 0.6f);
        yield return new WaitForSeconds(immuneTime / 4);
        playerMovement.sprite.color += new Color (0, 0, 0, 0.3f);
        yield return new WaitForSeconds(immuneTime / 4);
        playerMovement.sprite.color -= new Color (0, 0, 0, 0.3f);
        yield return new WaitForSeconds(immuneTime / 4);
        playerMovement.sprite.color += new Color (0, 0, 0, 0.6f);
        yield return new WaitForSeconds(immuneTime / 4);
    }

    IEnumerator GameOver()
    {
        health = maxHealth;
        player.transform.position = spawnPoint.position;
        yield return new WaitForSecondsRealtime(2f);
    }
}
