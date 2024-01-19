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
    public Transform spawnPoint;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private PlayerReflect playerReflect;
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
        Death();
    }

    public void Death()
    {
        canvas.gameObject.SetActive(true);
        health = 0;
        Time.timeScale = 1f;
        OnDamage?.Invoke(false);
        iframe = false;
        EventSystem.current.Death();
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
        OnDamage?.Invoke(true);
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 1f;
        OnDamage?.Invoke(false);
        yield return new WaitForSeconds(immuneTime);
        iframe = false;
    }

    IEnumerator GameOver()
    {
        health = maxHealth;
        //healthBar.UpdateResourceBar(health, maxHealth);
        player.transform.position = spawnPoint.position;
        //Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(2f);
        //Time.timeScale = 1f;
    }
}
