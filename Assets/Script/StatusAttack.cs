using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusAttack : MonoBehaviour
{
    
    [SerializeField] private StatusEffects.DebuffsType debuff;

    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<StatusEffects>().HandleStatusEffect(debuff);
        }
    }
}
