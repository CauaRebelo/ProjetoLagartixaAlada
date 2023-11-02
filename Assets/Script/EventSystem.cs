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

    public event Action onDeath;
    public void Death()
    {
        if (onDeath != null)
        {
            onDeath();
        }
    }

    public event Action onPickupCarrot;
    public void PickupCarrot()
    {
        if (onPickupCarrot != null)
        {
            onPickupCarrot();

        }
    }

    public event Action onForceDeath;
    public void ForceDeath()
    {
        if (onForceDeath != null)
        {
            onForceDeath();

        }
    }
}
