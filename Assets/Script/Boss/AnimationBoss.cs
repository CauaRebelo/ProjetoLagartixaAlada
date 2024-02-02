using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBoss : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private EnemyDamage enemyDamage;
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
