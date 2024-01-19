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
    public float burnInterval = 0.5f;

    private bool isSlow;
    private float slowTicks;
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
        }
        else
        {
            isBurning = true;
            burnTicks = ticks;
            StartCoroutine(BurnDamage(damage));
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
        }
        else
        {
            slowTicks = ticks;
            isSlow = true;
            StartCoroutine(SlowTime(slowAmount));
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

    IEnumerator BurnDamage(float damage)
    {
        while(burnTicks > 0)
        {
            burnTicks--;
            enemyDamage.Damage(2, damage, 0, 0, 0);
            yield return new WaitForSeconds(burnInterval);
        }
        isBurning = false;
    }

    IEnumerator SlowTime(float slowAmount)
    {
        enemyDamage.speedMultiplier = slowAmount;
        while (slowTicks > 0)
        {
            slowTicks--;
            yield return new WaitForSeconds(slowInterval);
        }
        enemyDamage.speedMultiplier = 1f;
        isSlow = false;
    }

    IEnumerator LightingCooldown(int index)
    {
        while(enemyDamage.gameObject.transform.parent.GetComponentInChildren<EnemyChainCooldown>())
        {
            yield return new WaitForSeconds(0.1f);
        }
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
}
