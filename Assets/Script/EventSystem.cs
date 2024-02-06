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

    public event Action onPlayerElementeChange;
    public void PlayerElementeChange()
    {
        if (onPlayerElementeChange != null)
        {
            onPlayerElementeChange();
        }
    }

    public event Action onPlayerFinishTalking;
    public void PlayerFinishTalking()
    {
        if (onPlayerFinishTalking != null)
        {
            onPlayerFinishTalking();
        }
    }

    public event Action onBossElementeChange;
    public void BossElementeChange()
    {
        if (onBossElementeChange != null)
        {
            onBossElementeChange();
        }
    }

    public event Action onBossDeath;
    public void BossDeath()
    {
        if (onBossDeath != null)
        {
            onBossDeath();
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

    public event Action onRespawn;
    public void Respawn()
    {
        if (onRespawn != null)
        {
            onRespawn();
        }
    }

    public event Action onSkillChange;

    public void SkillChange()
    {
        if (onSkillChange != null)
        {
            onSkillChange();
        }
    }
}
