using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    public float attackDamage;
    public float toleranceDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyDamage>().Damage(playerMovement.enchantment, attackDamage * playerMovement.damage, toleranceDamage * playerMovement.damage);
            playerMovement.isAbleToAct = true;
            playerMovement.hitEnemy = true;
        }
    }

}
