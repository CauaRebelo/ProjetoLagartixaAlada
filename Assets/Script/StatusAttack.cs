using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusAttack : MonoBehaviour
{
    
    public StatusEffects.DebuffsType debuff;
    public float effectVariable1;
    public float effectVariable2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<StatusEffects>().HandleStatusEffect(debuff, effectVariable1, effectVariable2);
        }
    }
}
