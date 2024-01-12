using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public Transform spawnPoint;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private PlayerReflect playerReflect;

    public bool parrying = false;
    public bool iframe = false;
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
        }

        if(health <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator Immune()
    {
        iframe = true;
        previousColor = sprite.color;
        sprite.color = Color.red;
        yield return new WaitForSeconds(immuneTime);
        iframe = false;
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
