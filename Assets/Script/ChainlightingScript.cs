using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainlightingScript : MonoBehaviour
{
    private CircleCollider2D coll;
    public LayerMask enemyLayer;

    public float damage;
    public float chainAmount;

    public GameObject chainLightingEffect;
    public GameObject chainLightingCooldown;

    private GameObject startOBject;
    private GameObject endObject;

    private Animator animator;
    public ParticleSystem particles;

    private int singleSpawns;


    void Start()
    {
        if(chainAmount <= 0)
        {
            Destroy(gameObject);
        }
        coll = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        particles = GetComponent<ParticleSystem>();
        startOBject = gameObject;
        singleSpawns = 1;
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if(col.gameObject.tag == "Enemy" && !col.GetComponentInChildren<EnemyChainCooldown>())
        {
            if(singleSpawns != 0)
            {
                endObject = col.gameObject;
                chainAmount -= 1;
                Instantiate(chainLightingEffect, col.gameObject.transform);
                Instantiate(chainLightingCooldown, col.gameObject.transform);
                col.gameObject.GetComponent<EnemyDamage>().Damage(3, damage, 0, 0, 0);
                animator.StopPlayback();
                coll.enabled = false;
                singleSpawns -= 1;
                particles.Play();
                var emitParms = new ParticleSystem.EmitParams();
                emitParms.position = startOBject.transform.position;
                particles.Emit(emitParms, 1);
                emitParms.position = endObject.transform.position;
                particles.Emit(emitParms, 1);
                emitParms.position = (startOBject.transform.position + endObject.transform.position) /2;
                particles.Emit(emitParms, 1);
                Destroy(gameObject, 1f);
            }
        }

    }
}
