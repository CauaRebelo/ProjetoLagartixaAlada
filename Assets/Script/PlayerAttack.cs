using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    public float attackDamage;
    public float toleranceDamage;
    public float knockX;
    public float knockY;

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
            int knockMultiplier = 1;
            if(!playerMovement.isFacingRight)
            {
                knockMultiplier *= -1;
            }
            col.gameObject.GetComponent<EnemyDamage>().Damage(playerMovement.enchantment, attackDamage * playerMovement.damage, toleranceDamage * playerMovement.damage, knockX * knockMultiplier, knockY);
            playerMovement.isAbleToAct = true;
            playerMovement.canAttack = true;
            playerMovement.hitEnemy = true;
        }
        if(col.gameObject.tag == "Wall")
        {
            col.gameObject.GetComponent<ElementalWallBehaviour>().Damage(playerMovement.enchantment, attackDamage * playerMovement.damage);
            playerMovement.isAbleToAct = true;
            playerMovement.canAttack = true;
            playerMovement.hitEnemy = true;
        }
    }

}
