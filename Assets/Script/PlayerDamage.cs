using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private SpriteRenderer sprite;

    public int health;
    public int maxHealth = 5;
    private Color previousColor;
    private float immuneTime = 1f;

    void Start()
    {
        health = maxHealth;
    }

    public void Damage()
    {
        if(!Info_Player.iframe)
        {
            health--;
            EventSystem.current.PlayerDamage();
            StartCoroutine(Immune());
        }

        if(health <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator Immune()
    {
        Info_Player.iframe = true;
        previousColor = sprite.color;
        sprite.color = Color.red;
        yield return new WaitForSeconds(immuneTime);
        Info_Player.iframe = false;
        sprite.color = previousColor;
    }

    IEnumerator GameOver()
    {
        health = maxHealth;
        player.transform.position = spawnPoint.position;
        //Time.timeScale = 0.1f;
        EventSystem.current.Death();
        yield return new WaitForSecondsRealtime(2f);
        //Time.timeScale = 1f;
    }
}
