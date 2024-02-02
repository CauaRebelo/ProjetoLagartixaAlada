using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttack : MonoBehaviour
{
    [SerializeField] private BulletSpawner bullet;
    [SerializeField] private EnemyDamage enemyDamage;


    public void Shoot()
    {
        bullet.Fire();
    }

    public void Damaged()
    {
        this.gameObject.GetComponent<Animator>().SetBool("Attack", false);
        enemyDamage.broken = true;
    }
    
    public void Death()
    {
        this.gameObject.GetComponent<Animator>().SetBool("Attack", false);
        enemyDamage.RemovedDeath();
    }
}
