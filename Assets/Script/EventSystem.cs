using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current;

    void Awake()
    {
        current = this;
    }

    public event Action onPlayerDamage;
    public void PlayerDamage()
    {
        if (onPlayerDamage != null)
        {
            onPlayerDamage();

        }
    }

    public event Action onDeath;
    public void Death()
    {
        if (onDeath != null)
        {
            onDeath();
        }
    }
}
