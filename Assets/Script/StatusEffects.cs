using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    [SerializeField] private EnemyDamage enemyDamage;

    public enum DebuffsType
    {
        None,
        Burning,
        Slow,
        Chainlight
    }

    private bool isBurning;
    private float burnTicks;
    private float currentBurnDamage;
    public float burnInterval = 0.5f;

    private bool isSlow;
    private float slowTicks;
    private float currentSlowAmount;
    public float slowInterval = 2f;

    private bool[] isChainlight = new bool[3] {false, false, false};
    public float chainInterval = 1f;
    public GameObject chainLightingEffect;
    public GameObject chainLightingCooldown;


    // Start is called before the first frame update
    void Start()
    {
        isBurning = false;
        isSlow = false;
        EventSystem.current.onDeath += OnDeath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleStatusEffect(DebuffsType debufftype, float effectVariable1, float effectVariable2)
    {
        switch (debufftype) 
        {
            case (DebuffsType.None):
                break;
            case (DebuffsType.Burning):
                Burning(effectVariable1, effectVariable2);
                break;
            case (DebuffsType.Slow):
                Slow(effectVariable1, effectVariable2);
                break;
            case (DebuffsType.Chainlight):
                Lighting(effectVariable1, effectVariable2);
                break;
        }
    }

    void Burning(float damage, float ticks)
    {
        if(isBurning)
        {
            if(burnTicks < ticks)
            {
                burnTicks = ticks;
            }

            if(currentBurnDamage < damage)
            {
                currentBurnDamage = damage;
            }
        }
        else
        {
            isBurning = true;
            burnTicks = ticks;
            currentBurnDamage = damage;
            StartCoroutine(BurnDamage());
        }
    }

    void Slow(float slowAmount, float ticks)
    {
        if (isSlow)
        {
            if(slowTicks < ticks)
            {
                slowTicks = ticks;
            }
            if(currentSlowAmount > slowAmount)
            {
                currentSlowAmount = slowAmount;
            }
        }
        else
        {
            slowTicks = ticks;
            isSlow = true;
            currentSlowAmount = slowAmount;
            StartCoroutine(SlowTime());
        }
    }

    void Lighting(float type, float damage)
    {
        int index = (int) type;
        if (isChainlight[index])
        {
            return;
        }
        else
        {
            isChainlight[index] = true;
            enemyDamage.Damage(3, damage, 0, 0, 0);
            Instantiate(chainLightingCooldown, enemyDamage.gameObject.transform);
            chainLightingEffect.GetComponent<ChainlightingScript>().damage = damage;
            chainLightingEffect.GetComponent<ChainlightingScript>().chainAmount = 5;
            Instantiate(chainLightingEffect, enemyDamage.gameObject.transform.position, Quaternion.identity);
            StartCoroutine(LightingCooldown(index));
        }
    }

    IEnumerator BurnDamage()
    {
        while(burnTicks > 0)
        {
            burnTicks--;
            enemyDamage.Damage(2, currentBurnDamage, 0, 0, 0);
            yield return new WaitForSeconds(burnInterval);
        }
        isBurning = false;
    }

    IEnumerator SlowTime()
    {
        while (slowTicks > 0)
        {
            enemyDamage.speedMultiplier = currentSlowAmount;
            slowTicks--;
            yield return new WaitForSeconds(slowInterval);
        }
        enemyDamage.speedMultiplier = 1f;
        isSlow = false;
    }

    IEnumerator LightingCooldown(int index)
    {
        if(index == 0)
        {
            yield return new WaitForSeconds(1f);
        }
        if(index == 1)
        {
            yield return new WaitForSeconds(0.5f);
        }
        if(index == 2)
        {
            yield return new WaitForSeconds(0.1f);
        }
        isChainlight[index] = false;
    }

    public void OnDeath()
    {
        StopAllCoroutines();
        isBurning = false;
        isSlow = false;
        isChainlight[0] = false;
        isChainlight[1] = false;
        isChainlight[2] = false;
    }
}
