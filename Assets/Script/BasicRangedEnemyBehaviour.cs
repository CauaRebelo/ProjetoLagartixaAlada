using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BasicRangedEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject attack;
    [SerializeField] private EnemyDamage enemyDamage;

    public List<Transform> waypoints;

    public Vector2 checkPoint = Vector2.zero;
    public int nextPoints = 0;
    int pointChangeValue = 1;
    private float movementSpeed = 3f;
    public float chaseDistance = 7f;
    public float attackDistance = 10f;
    public float attackSpeed = 1f;
    public float attackDowntime = 2f;
    public bool isChasing = false;
    public bool isAttacking = false;
    public bool isAbletoAct = true;
    public int type; //0 Neutral, 1 Ice, 2 Fire, 3 Thunder
    private float gravity;
    [field: SerializeField]
    public UnityEvent<bool> OnAttack { get; set; }
    [field: SerializeField]
    public UnityEvent<float> AttackSpeed { get; set; }
    [field: SerializeField]
    public UnityEvent<int> OnType { get; set; }

    public void Start()
    {
        EventSystem.current.onDeath += OnDeath;
        playerTransform = GameObject.FindWithTag("Player").transform;
        OnType.Invoke(type);
    }

    // Update is called once per frame
    void Update()
    {
        if(isAbletoAct)
        {
            if (enemyDamage.broken)
            {
                rb.gravityScale = 1f;
                if(isAttacking)
                {
                    isAttacking = false;
                    OnAttack.Invoke(false);
                }
                return;
            }
            if (!isAttacking)
            {
                rb.velocity = Vector2.zero;
                //if (isChasing)
                //{
                //    if (Vector2.Distance(transform.position, playerTransform.position) >= chaseDistance)
                //    {
                //        isChasing = false;
                //    }
                //    Chase();
                //}
                //else
                //{
                    //if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
                    //{
                    //    isChasing = true;
                    //}
                    //MoveToNextPoint();
                if (Vector2.Distance(transform.position, playerTransform.position) <= attackDistance)
                {
                    if (rb.transform.position.x > playerTransform.position.x)
                    {
                        rb.transform.localScale = new Vector3(1, 1, 1);
                    }
                    if (rb.transform.position.x < playerTransform.position.x)
                    {
                        rb.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    isAttacking = true;
                    AttackSpeed.Invoke(enemyDamage.speedMultiplier);
                    StartCoroutine(DoAttack());
                }
                else
                {
                    MoveToNextPoint();
                }
            }
        }
    }

    void MoveToNextPoint()
    {
        if (waypoints.Count > 0)
        {
            Transform goalPoint = waypoints[nextPoints];
            if (goalPoint.transform.position.x > rb.transform.position.x)
                rb.transform.localScale = new Vector3(-1, 1, 1);
            else
                rb.transform.localScale = new Vector3(1, 1, 1);
            rb.transform.position = Vector2.MoveTowards(rb.transform.position, goalPoint.position, movementSpeed * enemyDamage.speedMultiplier * Time.deltaTime);
            if (Vector2.Distance(rb.transform.position, goalPoint.position) < 1f)
            {
                if (nextPoints == waypoints.Count - 1)
                    pointChangeValue = -1;
                if (nextPoints == 0)
                    pointChangeValue = 1;
                nextPoints += pointChangeValue;
            }
        }
    }

    //void Chase()
    //{
    //    if (Vector2.Distance(transform.position, playerTransform.position) <= attackDistance)
    //    {
    //        isAttacking = true;
    //        StartCoroutine(DoAttack());
    //    }
        //if (rb.transform.position.x > playerTransform.position.x)
        //{
        //    rb.transform.localScale = new Vector3(1, 1, 1);
        //    rb.transform.position += Vector3.left * movementSpeed * Time.deltaTime;
        //}
        //if (rb.transform.position.x < playertransform.position.x)
        //{
        //    rb.transform.localscale = new vector3(-1, 1, 1);
        //    rb.transform.position += vector3.right * movementspeed * time.deltatime;
        //}
    //}

    IEnumerator DoAttack()
    {
        AttackSpeed.Invoke(enemyDamage.speedMultiplier);
        OnAttack.Invoke(true);
        attack.SetActive(true);
        attack.GetComponent<BulletSpawner>().firingRate = attackSpeed;
        yield return new WaitForSeconds(attackDowntime/enemyDamage.speedMultiplier);
        OnAttack.Invoke(false);
        attack.SetActive(false);
        isAttacking = false;
    }

    public void OnDeath()
    {
        StopAllCoroutines();
        OnAttack.Invoke(false);
        attack.SetActive(false);
        isAttacking = false;
    }
}
