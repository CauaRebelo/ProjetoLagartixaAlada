using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttackMelee : MonoBehaviour
{
    [SerializeField] private GameObject attack;
    [SerializeField] private EnemyDamage enemyDamage;
    [SerializeField] private Animator attackAnimator;


    public void Attack()
    {
        attack.gameObject.SetActive(true);
        attackAnimator.SetFloat("attackSpeed", enemyDamage.speedMultiplier);
        attack.GetComponent<Animator>().Play("SwingBasic");
    }

    public void AttackEnd()
    {
        if(attack.gameObject.activeSelf)
        {
            attack.GetComponent<Animator>().Play("MeleeIdle");
        }
        attack.gameObject.SetActive(false);
    }

    public void Damaged()
    {
        this.gameObject.GetComponent<Animator>().SetBool("Attack", false);
        attack.gameObject.SetActive(false);
        enemyDamage.RemovedDamaged();
    }
    
    public void Death()
    {
        this.gameObject.GetComponent<Animator>().SetBool("Attack", false);
        attack.gameObject.SetActive(false);
        enemyDamage.RemovedDeath();
    }
}
