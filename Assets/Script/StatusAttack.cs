using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatusAttack : MonoBehaviour
{
    
    public StatusEffects.DebuffsType debuff;
    public float effectVariable1;
    public float effectVariable2;

    public float cooldown;

    public bool isReady;

    void Start()
    {
        isReady = true;
    }

    private void OnEnable()
    {
        isReady = true;
    }
    // Update is called once per frame
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if(isReady)
            {
                if(col.gameObject.GetComponent<StatusEffects>() != null)
                {
                    col.gameObject.GetComponent<StatusEffects>().HandleStatusEffect(debuff, effectVariable1, effectVariable2);
                    isReady = false;
                    StartCoroutine(Cooldown());
                }
            }
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }
}
