using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject enemy;
    [SerializeField] private EnemyDamage enemyDamage;

    public List<Transform> waypoints;

    public Vector2 checkPoint = Vector2.zero;
    public int nextPoints = 0;
    int pointChangeValue = 1;
    private float movementSpeed = 3f;
    public float chaseDistance = 7f;
    public float attackDistance = 2f;
    public float attackDowntime = 2f;
    public int type; //0 Neutral, 1 Ice, 2 Fire, 3 Thunder
    public bool isChasing = false;
    public bool isAttacking = false;
    public bool isAbletoAct = true;
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
            if(!isAttacking)
            {
                if(enemyDamage.speedMultiplier == 1)
                {
                    AttackSpeed.Invoke(enemyDamage.speedMultiplier);
                }
                if (isChasing)
                {
                    if (Vector2.Distance(transform.position, playerTransform.position) >= chaseDistance)
                    {
                        isChasing = false;
                    }
                    Chase();
                }
                else
                {
                    if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
                    {
                        isChasing = true;
                    }
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

    void Chase()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackDistance)
        {
            isAttacking = true;
            StartCoroutine(DoAttack());
        }
        if (rb.transform.position.x > playerTransform.position.x)
        {
            rb.transform.localScale = new Vector3(1, 1, 1);
            rb.transform.position += Vector3.left * movementSpeed * enemyDamage.speedMultiplier * Time.deltaTime;
        }
        if (rb.transform.position.x < playerTransform.position.x)
        {
            rb.transform.localScale = new Vector3(-1, 1, 1);
            rb.transform.position += Vector3.right * movementSpeed * enemyDamage.speedMultiplier * Time.deltaTime;
        }
    }

    IEnumerator DoAttack()
    {
        AttackSpeed.Invoke(enemyDamage.speedMultiplier);
        OnAttack.Invoke(true);
        yield return new WaitForSeconds(attackDowntime/ enemyDamage.speedMultiplier);
        OnAttack.Invoke(false);
        isAttacking = false;
    }

    void OnDeath()
    {
        StopAllCoroutines();
        OnAttack.Invoke(false);
        isAttacking = false;
    }

}