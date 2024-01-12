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
    private int burnTicks;
    public float burnInterval = 0.5f;

    private bool isSlow;
    private int slowTicks;
    public float slowInterval = 2f;

    //private bool isChainlight;


    // Start is called before the first frame update
    void Start()
    {
        isBurning = false;
        isSlow = false;
        //isChainlight = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleStatusEffect(DebuffsType debufftype)
    {
        switch (debufftype) 
        {
            case (DebuffsType.None):
                break;
            case (DebuffsType.Burning):
                Burning();
                break;
            case (DebuffsType.Slow):
                Slow();
                break;
            case (DebuffsType.Chainlight):
                //Codigo;
                break;
        }
    }

    void Burning()
    {
        if(isBurning)
        {
            burnTicks = 12;
        }
        else
        {
            isBurning = true;
            burnTicks = 12;
            StartCoroutine(BurnDamage());
        }
    }

    void Slow()
    {
        if (isSlow)
        {
            slowTicks = 3;
        }
        else
        {
            slowTicks = 3;
            isSlow = true;
            StartCoroutine(SlowTime());
        }
    }

    IEnumerator BurnDamage()
    {
        while(burnTicks > 0)
        {
            burnTicks--;
            enemyDamage.Damage(2, 2, 0, 0, 0);
            yield return new WaitForSeconds(burnInterval);
        }
        isBurning = false;
    }

    IEnumerator SlowTime()
    {
        enemyDamage.speedMultiplier = 0.4f;
        while (slowTicks > 0)
        {
            slowTicks--;
            yield return new WaitForSeconds(slowInterval);
        }
        enemyDamage.speedMultiplier = 1f;
        isSlow = false;
    }
}
