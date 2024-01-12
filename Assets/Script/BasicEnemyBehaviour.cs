using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehaviour : MonoBehaviour
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
    public float attackDistance = 2f;
    public float attackSpeed = 1f;
    public float attackDowntime = 2f;
    public bool isChasing = false;
    public bool isAttacking = false;

    public void Start()
    {
        EventSystem.current.onDeath += OnDeath;
        playerTransform = GameObject.Find("/MainPlayer/Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAttacking)
        {
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
        attack.SetActive(true);
        attack.GetComponent<Animator>().SetFloat("attackSpeed", attackSpeed);
        yield return new WaitForSeconds(attackDowntime);
        attack.SetActive(false);
        isAttacking = false;
    }

    void OnDeath()
    {
        StopAllCoroutines();
        attack.SetActive(false);
        isAttacking = false;
    }

}